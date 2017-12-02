using DepositMVC.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DepositMVC.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: User
        public ActionResult Index()
        {
            return View();
        }

     

        [HttpPost]

        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(decimal price)
        {

             
            string idUser = IdentityExtensions.GetUserId(User.Identity);

            ApplicationUser user = db.Users.Find(idUser);
            Deposit deposit = db.Deposits.Where(p => p.FromPrice<= price && p.ToPrice>=price).FirstOrDefault();
            if(deposit == null)
            {
                ModelState.AddModelError("Deposit", "Депозита с такой ценой не найдено");
                
            }

            if (user.Balans - price < 0)
            {
                ModelState.AddModelError("Balans", "Не достаточно на счету Денег");
               
            }

            if (ModelState.IsValid)
            {
                user.Balans -= price;
                db.Entry(user).State = EntityState.Modified;
                await db.SaveChangesAsync();
                db.UserDeposits.Add(new UserDeposit() { DepositId = deposit.Id, ApplicationUserId = idUser, Date = DateTime.Now ,Price = price, Days = deposit.Days ,DateEnd = DateTime.Now.AddDays(deposit.Days)});
                await db.SaveChangesAsync();
                IEnumerable<UserDeposit> userDep = await db.UserDeposits.Where(p => p.ApplicationUserId == idUser).Include(p => p.Deposit).ToListAsync();
                return RedirectToAction("Index");
            }

            return View();


        }

        [HttpGet]


        public ActionResult BuyDepositUserGet()
        {
            string idUser = IdentityExtensions.GetUserId(User.Identity);
            return PartialView(db.UserDeposits.Where(p => p.ApplicationUserId == idUser).Include(p => p.Deposit));
        }

        public string GetUserBalans()
        {
            decimal balans = db.Users.Find(IdentityExtensions.GetUserId(User.Identity)).Balans;
            return balans.ToString();
        }
    }
}