using Microsoft.AspNet.Identity;
using MLGbudgeter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MLGbudgeter.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        [RequireHousehold]
        public ActionResult Index()
        {
            var id = User.Identity.GetHouseholdId();
            
            Household household = db.Households.Find(id);
            if (household == null)
            {
                return HttpNotFound();
            }
            return View(household);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        [Authorize]
        public ActionResult CreateJoinHousehold(Guid? code)
        {
            //If Current USer accessig this page already has a Household, redirect to their dashboard
            if(User.Identity.IsInHousehold())
            {
                return RedirectToAction("Index", "Home");
            }

            HouseholdViewModel vm = new HouseholdViewModel();

            //Determine User has been sent an invite and set property values
            if (code != null && ValidateInvite())
            {
                Invite result = db.Invites.FirstOrDefault(i => i.HHToken == code);
                vm.IsJoinHH = true;
                vm.HHId = result.HouseholdId;
                vm.HHName = result.Household.Name;

                //Set USED flag to true for this invite
                result.HasBeenUsed = true;

                ApplicationUser user = db.Users.Find(User.Identity.GetUserId());
                user.InviteEmail = result.Email;
                db.SaveChanges();
            }

                return View(vm);
        }

        private bool ValidateInvite()
        {
            return true;
        }

        [Authorize]
        [HttpPost]
        public ActionResult CreateHousehold(HouseholdViewModel vm)
        {
            Household hh = new Household();
            hh.Name = vm.HHName;

            vm.Member = db.Users.Find(User.Identity.GetUserId());

            db.Households.Add(hh);
            db.SaveChanges();

            hh.Members.Add(vm.Member);
            db.SaveChanges();

            var user = db.Users.Find(User.Identity.GetUserId());
            
            return RedirectToAction("Index", "Households");
        }

        [Authorize]
        [HttpPost]
        public ActionResult JoinHousehold(HouseholdViewModel vm)
        {
            Household hh = new Household();
            hh.Name = vm.HHName;

            vm.Member = db.Users.Find(User.Identity.GetUserId());

            db.Households.Add(hh);
            db.SaveChanges();

            return RedirectToAction("Index", "Households");
        }

        [HttpPost]
        [RequireHousehold]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> LeaveHousehold()
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            await ControllerContext.HttpContext.RefreshAuthentication(user);
            return RedirectToAction("Index", "Household");
        }
    }
}