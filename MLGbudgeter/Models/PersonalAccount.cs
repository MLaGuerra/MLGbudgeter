using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Util;

namespace MLGbudgeter.Models
{
    public class PersonalAccount
    {
        public PersonalAccount()
        {
            Transactions = new HashSet<Transaction>();
        }
        public int Id { get; set; }
        public int HouseholdId { get; set; }
        public string Name { get; set; }
        public decimal Balance { get; set; }
        public decimal ReconciledBalance { get; set; }

        public virtual ICollection<Transaction> Transactions { get; set; }
        public virtual Household Household { get; set; }
    }
}