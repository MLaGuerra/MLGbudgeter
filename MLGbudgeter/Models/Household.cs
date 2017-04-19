using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MLGbudgeter.Models
{
    public class Household
    {
        public Household()
        {
            Categories = new HashSet<Category>();
            Members = new HashSet<ApplicationUser>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Category> Categories { get; set; }
        public virtual ICollection<ApplicationUser> Members { get; set; }
}
}
