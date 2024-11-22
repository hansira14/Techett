using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Services.ServiceModels
{
    public class PreferenceViewModel
    {
        public int PreferenceId { get; set; }
        public int UserId { get; set; }
        public bool? IsEmailingOn { get; set; }
        public string ViewType { get; set; }
    }
}
