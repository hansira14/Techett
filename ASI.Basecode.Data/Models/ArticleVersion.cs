using System;
using System.Collections.Generic;

namespace ASI.Basecode.Data.Models
{
    public partial class ArticleVersion
    {
        public int VersionId { get; set; }
        public int ArticleId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime VersionDate { get; set; }
        public int VersionedBy { get; set; }

        public virtual Article Article { get; set; }
        public virtual User VersionedByNavigation { get; set; }
    }
}
