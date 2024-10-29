using System;
using System.Collections.Generic;

namespace ASI.Basecode.Data.Models
{
    public partial class Preference
    {
        public int PreferenceId { get; set; }
        public int UserId { get; set; }
        public bool? IsEmailingOn { get; set; }
        public string ViewType { get; set; }

        public virtual User User { get; set; }
    }
}
