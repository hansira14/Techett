using System;
using System.Collections.Generic;
using System.Linq;
using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.ServiceModels;
using ASI.Basecode.Services.Services;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using RestSharp;
using System.Threading.Tasks;

namespace ASI.Basecode.Services.Services;

public class NotificationService : INotificationService
{
    private readonly INotificationRepository _notificationRepository;
    private readonly IUserService _userService;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;
    private readonly string _brevoApiKey;
    private readonly string _senderEmail;

    public NotificationService(
        INotificationRepository notificationRepository,
        IUserService userService,
        IMapper mapper,
        IConfiguration configuration)
    {
        _notificationRepository = notificationRepository;
        _userService = userService;
        _mapper = mapper;
        _configuration = configuration;
        _brevoApiKey = configuration.GetValue<string>("EmailSettings:BrevoApiKey");
        _senderEmail = configuration.GetValue<string>("EmailSettings:SenderEmail");
    }

    public async Task CreateNotification(int ticketId, string message, string type, int userId, int agentId)
    {
        var notification = new Notification
        {
            TicketId = ticketId,
            Message = message,
            CreatedOn = DateTime.Now,
            IsSent = false,
            Type = type,
            IsRead = false,
            UserId = userId,
            AgentId = agentId
        };

        _notificationRepository.AddNotification(notification);

        // Get user email and send notification
        var user = _userService.GetUserById(userId);
        if (user != null && !string.IsNullOrEmpty(user.Email))
        {
            var emailSent = await SendEmailNotification(user.Email, "Ticket Update Notification", message);
            if (emailSent)
            {
                notification.IsSent = true;
                notification.SentOn = DateTime.Now;
                _notificationRepository.UpdateNotification(notification);
            }
        }
    }

    public IEnumerable<NotificationViewModel> GetUserNotifications(int userId)
    {
        var notifications = _notificationRepository.GetAllNotifications()
            .Where(n => n.UserId == userId || n.AgentId == userId)
            .OrderByDescending(n => n.CreatedOn)
            .Take(50);

        return _mapper.Map<IEnumerable<NotificationViewModel>>(notifications);
    }

    public void MarkAsRead(int notificationId)
    {
        var notification = _notificationRepository.GetNotificationById(notificationId);
        if (notification != null)
        {
            notification.IsRead = true;
            _notificationRepository.UpdateNotification(notification);
        }
    }

    public async Task<bool> SendEmailNotification(string recipientEmail, string subject, string message)
    {
        var client = new RestClient("https://api.brevo.com/v3/smtp/email");
        var request = new RestRequest("", Method.Post);
        
        request.AddHeader("accept", "application/json");
        request.AddHeader("api-key", _brevoApiKey);
        request.AddHeader("content-type", "application/json");

        var emailData = new
        {
            sender = new { email = _senderEmail, name = "Techett Helpdesk" },
            to = new[] { new { email = recipientEmail } },
            subject = subject,
            htmlContent = message
        };

        request.AddJsonBody(emailData);

        try
        {
            var response = await client.ExecuteAsync(request);
            return response.IsSuccessful;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to send email: {ex.Message}");
            return false;
        }
    }
}