using Microsoft.AspNet.Identity;
using MLGbudgeter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MLGbudgeter.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            return View();
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

        public ActionResult CreateJoinHousehold()
        {
            HouseholdViewModel vm = new HouseholdViewModel();
            return View(vm);
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
            var user = db.Users.Find(User.Identity.GetUserId(user));
            await ControllerContext.HttpContext.RefreshAuthentication(user);
            return RedirectToAction("Create", "Household");
        }
    }
}