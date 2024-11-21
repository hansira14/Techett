using System;
using System.Collections.Generic;

namespace ASI.Basecode.Data.Models
{
    public partial class ArticleAttachment
    {
        public int AttachmentId { get; set; }
        public int ArticleId { get; set; }
        public string Source { get; set; }
        public DateTime UploadedOn { get; set; }
        public int UploadedBy { get; set; }
        public string Filename { get; set; }
        public string Filetype { get; set; }
        public long Filesize { get; set; }

        public virtual Article Article { get; set; }
        public virtual User UploadedByNavigation { get; set; }
    }
}
