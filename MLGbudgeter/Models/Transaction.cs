using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MLGbudgeter.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public string Description { get; set; }
        public DateTimeOffset Date { get; set; }
        public decimal Amount { get; set; }
        public bool Type { get; set; }
        public int CategoryId { get; set; }
        public string EnteredById { get; set; }
        public bool Reconciled { get; set; }
        public decimal ReconciledAmount { get; set; }

        public virtual ICollection<Transaction> Transactions { get; set; }
        public virtual Household Household { get; set; }
    }
}