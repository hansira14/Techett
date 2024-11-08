using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.Interfaces;
using ASI.Basecode.Services.ServiceModels;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ASI.Basecode.Services.Services
{
    public class AttachmentService : IAttachmentService
    {
        private readonly IAttachmentRepository _attachmentRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly string _baseUploadPath;
        private readonly string _fileServerUrl;

        public AttachmentService(
            IAttachmentRepository attachmentRepository, 
            IMapper mapper,
            IConfiguration configuration)
        {
            _attachmentRepository = attachmentRepository;
            _mapper = mapper;
            _configuration = configuration;
            _fileServerUrl = _configuration["FileServer:Url"] ?? "http://127.0.0.1:8080";
            _baseUploadPath = _configuration["FileServer:UploadPath"] ?? 
                Path.Combine(Directory.GetCurrentDirectory(), "FileStorage");
            
            EnsureDirectoryExists(_baseUploadPath);
        }

        public IEnumerable<AttachmentViewModel> GetTicketAttachments(int ticketId)
        {
            var attachments = _attachmentRepository.GetTicketAttachments(ticketId);
            return _mapper.Map<IEnumerable<AttachmentViewModel>>(attachments);
        }

        public AttachmentViewModel AddAttachment(IFormFile file, int ticketId, int userId)
        {
            // Create organized directory structure
            var ticketPath = Path.Combine(_baseUploadPath, "tickets", ticketId.ToString());
            EnsureDirectoryExists(ticketPath);

            // Create filename with meaningful prefix and original extension
            var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            var safeFileName = Path.GetFileNameWithoutExtension(file.FileName).Replace(" ", "-");
            var extension = Path.GetExtension(file.FileName);
            var fileName = $"{timestamp}_{userId}_{safeFileName}{extension}";
            var filePath = Path.Combine(ticketPath, fileName);

            // Save file
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            // Calculate relative path for storage
            var relativePath = Path.Combine("tickets", ticketId.ToString(), fileName);
            var fileUrl = $"{_fileServerUrl}/{relativePath.Replace("\\", "/")}";

            var attachment = new Attachment
            {
                TicketId = ticketId,
                Source = fileUrl,
                UploadedOn = DateTime.Now,
                UploadedBy = userId,
                Filename = file.FileName,
                Filetype = file.ContentType,
                Filesize = file.Length
            };

            _attachmentRepository.AddAttachment(attachment);
            return _mapper.Map<AttachmentViewModel>(attachment);
        }

        public void DeleteAttachment(int attachmentId)
        {
            var attachment = _attachmentRepository.GetAttachmentById(attachmentId);
            if (attachment != null)
            {
                // Extract the relative path from the source URL
                var uri = new Uri(attachment.Source);
                var relativePath = uri.LocalPath.TrimStart('/');
                
                // Construct the full file path
                var filePath = Path.Combine(_baseUploadPath, relativePath);

                // Delete the physical file if it exists
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                // Delete the database record
                _attachmentRepository.DeleteAttachment(attachment);
            }
        }

        public AttachmentViewModel GetAttachmentById(int attachmentId)
        {
            var attachment = _attachmentRepository.GetAttachmentById(attachmentId);
            return _mapper.Map<AttachmentViewModel>(attachment);
        }

        private void EnsureDirectoryExists(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
    }
}