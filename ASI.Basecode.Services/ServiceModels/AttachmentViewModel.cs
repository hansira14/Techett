using System;

namespace ASI.Basecode.Services.ServiceModels
{
    public class AttachmentViewModel
    {
        public int AttachmentId { get; set; }
        public int TicketId { get; set; }
        public string Source { get; set; }
        public DateTime UploadedOn { get; set; }
        public int UploadedBy { get; set; }
        public string UploadedByName { get; set; }
        public string Filename { get; set; }
        public string Filetype { get; set; }
        public long Filesize { get; set; }
    }
} 