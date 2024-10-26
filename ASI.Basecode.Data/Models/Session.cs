using System;
using System.Collections.Generic;

namespace ASI.Basecode.Data.Models
{
    public partial class Session
    {
        public int SessionId { get; set; }
        public int UserId { get; set; }
        public string Token { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ExpiresOn { get; set; }

        public virtual User User { get; set; }
    }
}
