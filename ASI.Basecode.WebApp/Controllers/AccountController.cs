using ASI.Basecode.Data.Models;
using ASI.Basecode.Services;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.Manager;
using ASI.Basecode.Services.ServiceModels;
using ASI.Basecode.WebApp.Authentication;
using ASI.Basecode.WebApp.Models;
using ASI.Basecode.WebApp.Mvc;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static ASI.Basecode.Resources.Constants.Enums;

namespace ASI.Basecode.WebApp.Controllers
{
    public class AccountController : ControllerBase<AccountController>
    {
        private readonly SessionManager _sessionManager;
        private readonly SignInManager _signInManager;
        private readonly TokenValidationParametersFactory _tokenValidationParametersFactory;
        private readonly TokenProviderOptionsFactory _tokenProviderOptionsFactory;
        private readonly IConfiguration _appConfiguration;
        private readonly IUserService _userService;
        private readonly IUserAuthorizationService _userAuthorizationService;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountController"/> class.
        /// </summary>
        /// <param name="signInManager">The sign in manager.</param>
        /// <param name="localizer">The localizer.</param>
        /// <param name="userService">The user service.</param>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="tokenValidationParametersFactory">The token validation parameters factory.</param>
        /// <param name="tokenProviderOptionsFactory">The token provider options factory.</param>
        public AccountController(
                            SignInManager signInManager,
                            IHttpContextAccessor httpContextAccessor,
                            ILoggerFactory loggerFactory,
                            IConfiguration configuration,
                            IMapper mapper,
                            IUserService userService,
                            IUserAuthorizationService userAuthorizationService,
                            TokenValidationParametersFactory tokenValidationParametersFactory,
                            TokenProviderOptionsFactory tokenProviderOptionsFactory) : base(httpContextAccessor, loggerFactory, configuration, mapper)
        {
            this._sessionManager = new SessionManager(this._session);
            this._signInManager = signInManager;
            this._tokenProviderOptionsFactory = tokenProviderOptionsFactory;
            this._tokenValidationParametersFactory = tokenValidationParametersFactory;
            this._appConfiguration = configuration;
            this._userService = userService;
            this._userAuthorizationService = userAuthorizationService;
            this._mapper = mapper;
        }

        /// <summary>
        /// Login Method
        /// </summary>
        /// <returns>Created response view</returns>
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            TempData["returnUrl"] = System.Net.WebUtility.UrlDecode(HttpContext.Request.Query["ReturnUrl"]);
            this._sessionManager.Clear();
            this._session.SetString("SessionId", Guid.NewGuid().ToString());
            return this.View();
        }

        /// <summary>
        /// Authenticate user and signs the user in when successful.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="returnUrl">The return URL.</param>
        /// <returns> Created response view </returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
        {
            this._session.SetString("HasSession", "Exist");

            User user = null;

            var loginResult = _userService.AuthenticateUser(model.Email, model.Password, ref user);
            if (loginResult == LoginResult.Success)
            {
                // Authentication OK
                await this._signInManager.SignInAsync(user);
                
                // Set session variables
                this._session.SetString("UserName", string.Join(" ", user.Fname, user.Lname));
                this._session.SetString("FirstName", user.Fname);
                this._session.SetString("LastName", user.Lname);
                this._session.SetString("ProfilePictureUrl", user.ProfilePicUrl ?? ""); 

                return RedirectToAction("Index", "Home");
            }
            else
            {
                // Authentication failed
                TempData["ErrorMessage"] = "Incorrect UserId or Password";
                return View();
            }
        }

        /// <summary>
        /// Sign Out current account and return login view.
        /// </summary>
        /// <returns>Created response view</returns>
        [AllowAnonymous]
        public async Task<IActionResult> SignOutUser()
        {
            await this._signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        [Authorize(Policy = "RequireAdminRole")]
        public IActionResult Users()
        {
            var users = _userService.GetAllUsers();
            return View(users);
        }

        [HttpGet]
        public IActionResult ViewUser(int id)
        {
            var user = _userService.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost]
        [Authorize(Policy = "RequireAdminRole")]
        public IActionResult CreateUser(UserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);
                return Json(new { success = false, message = string.Join(", ", errors) });
            }

            try
            {
                if (!_userAuthorizationService.CanAssignRole(model.Role))
                {
                    return Json(new { success = false, message = "You don't have permission to assign this role." });
                }

                _userService.AddUser(model, int.Parse(UserId));
                return Json(new { success = true, message = "User created successfully!" });
            }
            catch (InvalidDataException ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = Resources.Messages.Errors.ServerError });
            }
        }

        [HttpPost]
        [Authorize(Policy = "RequireAdminRole")]
        public IActionResult EditUser(EditUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);
                return Json(new { success = false, message = string.Join(", ", errors) });
            }

            try
            {
                var existingUser = _userService.GetUserById(model.UserId);
                if (existingUser == null)
                {
                    return Json(new { success = false, message = "User not found" });
                }

                if (!_userAuthorizationService.CanModifyUser(existingUser.Role))
                {
                    return Json(new { success = false, message = "You don't have permission to modify this user." });
                }

                if (!_userAuthorizationService.CanAssignRole(model.Role))
                {
                    return Json(new { success = false, message = "You don't have permission to assign this role." });
                }

                if (model.UserId == int.Parse(UserId))
                {
                    if (model.Role != existingUser.Role)
                    {
                        return Json(new { success = false, message = "You cannot modify your own role." });
                    }
                }

                var userViewModel = new UserViewModel
                {
                    UserId = model.UserId,
                    Fname = model.Fname,
                    Lname = model.Lname,
                    Email = model.Email,
                    Role = model.Role,
                    IsActive = model.IsActive,
                    Password = string.IsNullOrEmpty(model.Password) ? null : model.Password,
                    ConfirmPassword = string.IsNullOrEmpty(model.ConfirmPassword) ? null : model.ConfirmPassword
                };

                _userService.UpdateUser(userViewModel, model.UserId);
                return Json(new { success = true });
            }
            catch (InvalidDataException ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = Resources.Messages.Errors.ServerError });
            }

        }

        [HttpDelete]
        [Authorize(Policy = "RequireAdminRole")]
        [Route("Account/DeleteUser/{id}")]
        public IActionResult DeleteUser(int id)
        {
            try
            {
                var userToDelete = _userService.GetUserById(id);
                if (userToDelete == null)
                {
                    return Json(new { success = false, message = "User not found" });
                }

                if (!_userAuthorizationService.CanModifyUser(userToDelete.Role))
                {
                    return Json(new { success = false, message = "You don't have permission to delete this user." });
                }

                _userService.DeleteUser(id);
                return Json(new { success = true });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
            catch
            {
                return Json(new { success = false, message = Resources.Messages.Errors.ServerError });
            }
        }

        [HttpGet]
        [Route("Account/GetUserDetails/{id}")]
        public IActionResult GetUserDetails(int id)
        {
            try
            {
                var user = _userService.GetUserById(id);
                if (user == null)
                {
                    return NotFound();
                }
                return Json(user);
            }
            catch
            {
                return Json(new { success = false, message = Resources.Messages.Errors.ServerError });
            }
        }
        [HttpGet]
        public IActionResult Settings(int? id = null)
        {
            try
            {
                int userId;

                if (id.HasValue)
                {
                    userId = id.Value;
                }
                else
                {
                    var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId");
                    if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out userId))
                    {
                        return RedirectToAction("Login");
                    }
                }

                var profile = _userService.GetUserProfile(userId);
                if (profile == null)
                {
                    return NotFound();
                }
                return View(profile);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching user profile");
                return View("Error");
            }
        }
        public IActionResult AboutUs()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Profile(int? id = null)
        {
            try
            {
                int userId;
                
                if (id.HasValue)
                {
                    userId = id.Value;
                }
                else
                {
                    var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId");
                    if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out userId))
                    {
                        return RedirectToAction("Login");
                    }
                }

                var profile = _userService.GetUserProfile(userId);
                if (profile == null)
                {
                    return NotFound();
                }
                return View(profile);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching user profile");
                return View("Error");
            }
        }


        [HttpGet]
        [Authorize(Policy = "RequireAdminRole")]
        public IActionResult Agents()
        {
            try
            {
                var agents = _userService.GetAllAgents();
                return View(agents);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching agents");
                return View("Error", new ErrorViewModel 
                { 
                    RequestId = HttpContext.TraceIdentifier
                });
            }
        }

        [HttpPost]
        public IActionResult UpdateProfilePicture(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                    return Json(new { success = false, message = "No file uploaded" });

                var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value);
                
                var fileName = $"profile_{userId}_{DateTime.Now.Ticks}{Path.GetExtension(file.FileName)}";
                var uploadPath = Path.Combine(_configuration["FileServer:UploadPath"], "profiles");
                
                if (!Directory.Exists(uploadPath))
                    Directory.CreateDirectory(uploadPath);

                var filePath = Path.Combine(uploadPath, fileName);
                
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
                var relativeFilePath = $"/files/profiles/{fileName}";
                _userService.UpdateProfilePicture(userId, relativeFilePath);

                return Json(new { success = true, profilePictureUrl = relativeFilePath });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating profile picture");
                return Json(new { success = false, message = "Error updating profile picture" });
            }
        }

        [HttpGet]
        public IActionResult SearchUsers(string searchTerm, int page = 1, int pageSize = 12, string[] roles = null)
        {
            var query = _userService.GetAllUsers();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                searchTerm = searchTerm.ToLower();
                query = query.Where(u => 
                    u.Fname.ToLower().Contains(searchTerm) ||
                    u.Lname.ToLower().Contains(searchTerm) ||
                    u.Email.ToLower().Contains(searchTerm));
            }

            if (roles != null && roles.Length > 0)
            {
                query = query.Where(u => roles.Contains(u.Role));
            }

            var totalUsers = query.Count();
            var users = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return Json(new { 
                users, 
                currentPage = page, 
                pageSize, 
                totalUsers 
            });
        }

        [HttpGet]
        [Authorize(Policy = "RequireAdminRole")]
        public IActionResult SearchAgents(string searchTerm, int page = 1, int pageSize = 10)
        {
            try
            {
                var query = _userService.GetAllAgents().AsQueryable();

                if (!string.IsNullOrEmpty(searchTerm))
                {
                    searchTerm = searchTerm.ToLower();
                    query = query.Where(a => 
                        a.Fname.ToLower().Contains(searchTerm) ||
                        a.Lname.ToLower().Contains(searchTerm) ||
                        a.Email.ToLower().Contains(searchTerm));
                }

                var totalAgents = query.Count();
                var agents = query
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                return Json(new { 
                    agents, 
                    currentPage = page, 
                    pageSize, 
                    totalAgents 
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while searching agents");
                return Json(new { success = false, message = "Error occurred while searching agents" });
            }
        }
    }
}

