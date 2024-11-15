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
    public class ArticleAttachmentService : IArticleAttachmentService
    {
        private readonly IArticleAttachmentRepository _articleAttachmentRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly string _baseUploadPath;
        private readonly string _fileServerUrl;

        public ArticleAttachmentService(
            IArticleAttachmentRepository articleAttachmentRepository,
            IMapper mapper,
            IConfiguration configuration)
        {
            _articleAttachmentRepository = articleAttachmentRepository;
            _mapper = mapper;
            _configuration = configuration;
            _fileServerUrl = _configuration["FileServer:Url"] ?? "http://127.0.0.1:8080";
            _baseUploadPath = _configuration["FileServer:UploadPath"] ??
                Path.Combine(Directory.GetCurrentDirectory(), "FileStorage");

            EnsureDirectoryExists(_baseUploadPath);
        }

        public IEnumerable<ArticleAttachmentViewModel> GetArticleAttachments(int articleId)
        {
            var attachments = _articleAttachmentRepository.GetArticleAttachmentsByArticleId(articleId);
            return _mapper.Map<IEnumerable<ArticleAttachmentViewModel>>(attachments);
        }

        public ArticleAttachmentViewModel AddAttachment(IFormFile file, int articleId, int userId)
        {
            var articlePath = Path.Combine(_baseUploadPath, "articles", articleId.ToString());
            EnsureDirectoryExists(articlePath);

            var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            var safeFileName = Path.GetFileNameWithoutExtension(file.FileName).Replace(" ", "-");
            var extension = Path.GetExtension(file.FileName);
            var fileName = $"{timestamp}_{userId}_{safeFileName}{extension}";
            var filePath = Path.Combine(articlePath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            var relativePath = Path.Combine("articles", articleId.ToString(), fileName);
            var fileUrl = $"{_fileServerUrl}/{relativePath.Replace("\\", "/")}";

            var attachment = new ArticleAttachment
            {
                ArticleId = articleId,
                Source = fileUrl,
                UploadedOn = DateTime.Now,
                UploadedBy = userId,
                Filename = file.FileName,
                Filetype = file.ContentType,
                Filesize = file.Length
            };

            _articleAttachmentRepository.AddArticleAttachment(attachment);
            return _mapper.Map<ArticleAttachmentViewModel>(attachment);
        }

        public void DeleteAttachment(int attachmentId)
        {
            var attachment = _articleAttachmentRepository.GetArticleAttachmentById(attachmentId);
            if (attachment != null)
            {
                var uri = new Uri(attachment.Source);
                var relativePath = uri.LocalPath.TrimStart('/');
                var filePath = Path.Combine(_baseUploadPath, relativePath);

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                _articleAttachmentRepository.DeleteArticleAttachment(attachment);
            }
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