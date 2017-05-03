using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Linq;
using System.Security.Principal;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using System.Collections.Generic;

namespace MLGbudgeter.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        { }
        //public string DisplayName { get; internal set; }
        //public string FirstName { get; internal set; }
        //public string LastName { get; internal set; }
        //public string FullName { get; internal set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);

            // Add custom user claims here
            userIdentity.AddClaim(new Claim("HouseholdId", this.HouseholdId.ToString()));
            return userIdentity;
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DisplayName { get; set; }
        public string FullName { get; set; }
        public int? HouseholdId { get; set; }
        public string InviteEmail { get; set; }

        public virtual Household Household { get; set; }
        public virtual ICollection<Invite> Invites { get; set; }
        public virtual ICollection<PersonalAccount> Accounts { get; set; }
    }

    public static class AuthExtensions
    {
        public static int? GetHouseholdId(this IIdentity user)
        {
            var claimsIdentity = (ClaimsIdentity)user;
            var HouseholdClaim = claimsIdentity.Claims.FirstOrDefault(c => c.Type == "HouseholdId");
            if (HouseholdClaim != null)
                return int.Parse(HouseholdClaim.Value);
            else
                return null;
        }
        public static bool IsInHousehold(this IIdentity user)
        {
            var householdClaim = ((ClaimsIdentity)user).Claims.FirstOrDefault(c => c.Type == "HouseholdId");
            return householdClaim != null && !string.IsNullOrWhiteSpace(householdClaim.Value);
        }
        //This secretly logs a user out, and logs them back in, when they "leave" a household resulting in an immediate identity refresh
        public static async Task RefreshAuthentication(this HttpContextBase context, ApplicationUser user)
        {
            context.GetOwinContext().Authentication.SignOut();
            await context.GetOwinContext().Get<ApplicationSignInManager>().SignInAsync(user, isPersistent: false, rememberBrowser: false);
        }
    }


    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
        public DbSet<Household> Households { get; set; }
        public DbSet<PersonalAccount> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Budget> Budgets { get; set; }
        public DbSet<BudgetItem> BudgetItems { get; set; }
        public DbSet<Invite> Invites { get; set; }
    }
}