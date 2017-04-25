using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace MLGbudgeter.Models
{
    public class HouseholdViewModel
    {
        public int? HHId { get; set; }
        public string HHName { get; set;}
        public string InviteToken { get; set; }
        public ApplicationUser Member { get; set; }
    }
}