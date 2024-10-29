using System;
using System.Collections.Generic;

namespace ASI.Basecode.Data.Models
{
    public partial class Article
    {
        public Article()
        {
            ArticleVersions = new HashSet<ArticleVersion>();
        }

        public int ArticleId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }

        public virtual User CreatedByNavigation { get; set; }
        public virtual ICollection<ArticleVersion> ArticleVersions { get; set; }
    }
}
