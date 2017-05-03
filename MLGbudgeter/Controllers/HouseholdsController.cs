using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MLGbudgeter.Models;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace MLGbudgeter.Controllers
{
    public class HouseholdsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Households
        public ActionResult Index()
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            Household Household = new Household();
            Household = db.Households.FirstOrDefault(h => h.Members.Any(hm => hm.Id== user.Id));
            return View(Household);
        }

        public ActionResult Invite()
        {
            return View();
        }

        // POST: Emails Household Invitation & 
        //       Also Saves Invite Info into Db
        [HttpPost]
        public async Task<ActionResult> Invite(string email)
        {
            var code = Guid.NewGuid();
            var callbackUrl = Url.Action("CreateJoinHousehold", "Households", new { code = code }, protocol: Request.Url.Scheme);

            EmailService ems = new EmailService();
            IdentityMessage msg = new IdentityMessage();

            msg.Body = "Hey! Join my household... Click <a href=\"" + callbackUrl + "\">here</a>";
            msg.Destination = email;
            msg.Subject = "Invite to Household";

            await ems.SendAsync(msg);

            //Create Records in Invite Page
            Invite model = new Invite();
            model.Email = email;
            model.HHToken = code;
            model.HouseholdId = User.Identity.GetHouseholdId().Value;
            model.InviteDate = DateTimeOffset.Now;
            model.InvitedById = User.Identity.GetUserId();

            db.Invites.Add(model);
            db.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        public ActionResult CreateJoinHousehold(Guid? code)
        {
            if (code != null)
            {
                bool var = db.Invites.Any(i => i.HHToken == code);
                Invite result = db.Invites.FirstOrDefault(i => i.HHToken == code);
            }


            HouseholdViewModel model = new HouseholdViewModel();

            if (User.Identity.IsInHousehold())
                return RedirectToAction("Index");

            return View();
        }
        // GET: Households/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Household household = db.Households.Find(id);
            if (household == null)
            {
                return HttpNotFound();
            }
            return View(household);
        }

        // GET: Households/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Households/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] Household household)
        {
            if (ModelState.IsValid)
            {
                db.Households.Add(household);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(household);
        }

        // GET: Households/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Household household = db.Households.Find(id);
            if (household == null)
            {
                return HttpNotFound();
            }
            return View(household);
        }

        // POST: Households/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] Household household)
        {
            if (ModelState.IsValid)
            {
                db.Entry(household).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(household);
        }

        // GET: Households/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Household household = db.Households.Find(id);
            if (household == null)
            {
                return HttpNotFound();
            }
            return View(household);
        }

        // POST: Households/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Household household = db.Households.Find(id);
            db.Households.Remove(household);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
