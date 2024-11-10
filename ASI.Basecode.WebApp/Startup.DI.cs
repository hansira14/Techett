﻿using ASI.Basecode.Data;
using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Repositories;
using ASI.Basecode.Services;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.ServiceModels;
using ASI.Basecode.Services.Services;
using ASI.Basecode.WebApp.Authentication;
using ASI.Basecode.WebApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ASI.Basecode.WebApp
{
    // Other services configuration
    internal partial class StartupConfigurer
    {
        /// <summary>
        /// Configures the other services.
        /// </summary>
        private void ConfigureOtherServices()
        {
            // Framework
            this._services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            this._services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();

            // Common
            this._services.AddScoped<TokenProvider>();
            this._services.TryAddSingleton<TokenProviderOptionsFactory>();
            this._services.TryAddSingleton<TokenValidationParametersFactory>();
            this._services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Services
            this._services.TryAddSingleton<TokenValidationParametersFactory>();
            this._services.AddScoped<IUserService, UserService>();
            this._services.AddScoped<IArticleService, ArticleService>();
            this._services.AddScoped<IArticleVersionService, ArticleVersionService>();
            this._services.AddScoped<IAttachmentService, AttachmentService>();
            this._services.AddScoped<ICommentService, CommentService>();
            this._services.AddScoped<IFeedbackService, FeedbackService>();
            this._services.AddScoped<INotificationService, NotificationService>();
            this._services.AddScoped<IPreferenceService, PreferenceService>();
            this._services.AddScoped<ISessionService, SessionService>();
            this._services.AddScoped<ITeamService, TeamService>();
            this._services.AddScoped<ITicketService, TicketService>();
            this._services.AddScoped<IUpdateService, UpdateService>();
            this._services.AddScoped<IAssignmentService, AssignmentService>();
            this._services.AddScoped<IUserAuthorizationService, UserAuthorizationService>();

            // Repositories
            this._services.AddScoped<IUserRepository, UserRepository>();
            this._services.AddScoped<IArticleRepository, ArticleRepository>();
            this._services.AddScoped<IArticleVersionRepository, ArticleVersionRepository>();
            this._services.AddScoped<IAttachmentRepository, AttachmentRepository>();
            this._services.AddScoped<ICommentRepository, CommentRepository>();
            this._services.AddScoped<IFeedbackRepository, FeedbackRepository>();
            this._services.AddScoped<INotificationRepository, NotificationRepository>();
            this._services.AddScoped<IPreferenceRepository, PreferenceRepository>();
            this._services.AddScoped<ISessionRepository, SessionRepository>();
            this._services.AddScoped<ITeamRepository, TeamRepository>();
            this._services.AddScoped<ITicketRepository, TicketRepository>();
            this._services.AddScoped<IUpdateRepository, UpdateRepository>();
            this._services.AddScoped<IAssignmentRepository, AssignmentRepository>();

            // Manager Class
            this._services.AddScoped<SignInManager>();

            this._services.AddHttpClient();
        }
    }
}
