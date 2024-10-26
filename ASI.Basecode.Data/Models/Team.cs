using System;
using System.Collections.Generic;

namespace ASI.Basecode.Data.Models
{
    public partial class Team
    {
        public Team()
        {
            Users = new HashSet<User>();
        }

        public int TeamId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
