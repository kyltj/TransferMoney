using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DepositMVC.Models;

namespace DepositMVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class TransferMoneyUsersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: TransferMoneyUsers
        public async Task<ActionResult> Index()
        {
            var transferMoneyUsers = db.TransferMoneyUsers.Include(t => t.ApplicationUser);
            return View(await transferMoneyUsers.ToListAsync());
        }

        // GET: TransferMoneyUsers/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TransferMoneyUser transferMoneyUser = await db.TransferMoneyUsers.FindAsync(id);
            if (transferMoneyUser == null)
            {
                return HttpNotFound();
            }
            return View(transferMoneyUser);
        }

        // GET: TransferMoneyUsers/Create
        public ActionResult Create()
        {
            ViewBag.ApplicationUserId = new SelectList(db.Users, "Id", "Email");
            return View();
        }

        // POST: TransferMoneyUsers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Amount,ApplicationUserId,SetGet,Status,Date")] TransferMoneyUser transferMoneyUser)
        {
            transferMoneyUser.Date = DateTime.Now;
            ApplicationUser user = db.Users.Find(transferMoneyUser.ApplicationUserId);

            if (transferMoneyUser.SetGet == false)
            {
                if (user.Balans - transferMoneyUser.Amount < 0)
                {
                    ModelState.AddModelError("Balans", "У Даного Пользователя Недостаточно Средств");
                }
            }
            if (ModelState.IsValid)
            {

                if (transferMoneyUser.SetGet == false)
                {
                    user.Balans -= transferMoneyUser.Amount;
                    db.Entry(user).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                }
                if (transferMoneyUser.SetGet == true)
                {
                    user.Balans += transferMoneyUser.Amount;
                    db.Entry(user).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                }
                db.TransferMoneyUsers.Add(transferMoneyUser);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ApplicationUserId = new SelectList(db.Users, "Id", "Email", transferMoneyUser.ApplicationUserId);
            return View(transferMoneyUser);
        }

        [HttpPost]

        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AccuralAll()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            IEnumerable<UserDeposit> userDep = await db.UserDeposits.Include(p => p.Deposit).Include(p => p.ApplicationUser).ToListAsync();
            foreach (var item in userDep)
            {
                int diffMinute = DateTime.Now.Subtract(item.Date).Minutes;


                if (diffMinute > 0 && item.Days > 0)
                {
                    if (item.Days - diffMinute >= 0)
                    {
                        item.ApplicationUser.Balans += ((item.Deposit.Accrual * item.Price) / 100) * diffMinute;
                        db.Entry(item.ApplicationUser).State = EntityState.Modified;
                        await db.SaveChangesAsync();
                        item.Days -= diffMinute;
                        item.AccuralYet += ((item.Deposit.Accrual * item.Price) / 100) * diffMinute;
                        db.Entry(item).State = EntityState.Modified;
                        await db.SaveChangesAsync();
                    }

                    if (item.Days == 0)
                    {
                        decimal price = item.Price;
                        item.ApplicationUser.Balans += price;
                        db.Entry(item.ApplicationUser).State = EntityState.Modified;
                        await db.SaveChangesAsync();
                        db.UserDeposits.Remove(item);
                        await db.SaveChangesAsync();
                    }
                    if (item.Days - diffMinute < 0)
                    {
                        item.ApplicationUser.Balans += ((item.Deposit.Accrual * item.Price) / 100) * item.Days;
                        db.Entry(item.ApplicationUser).State = EntityState.Modified;
                        await db.SaveChangesAsync();

                        decimal price = item.Price;
                        item.ApplicationUser.Balans += price;
                        db.Entry(item.ApplicationUser).State = EntityState.Modified;
                        await db.SaveChangesAsync();
                        db.UserDeposits.Remove(item);
                        await db.SaveChangesAsync();
                    }
                }
            }
            return RedirectToAction("Index");
        }

        // GET: TransferMoneyUsers/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TransferMoneyUser transferMoneyUser = await db.TransferMoneyUsers.FindAsync(id);
            if (transferMoneyUser == null)
            {
                return HttpNotFound();
            }
            return View(transferMoneyUser);
        }

        // POST: TransferMoneyUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            TransferMoneyUser transferMoneyUser = await db.TransferMoneyUsers.FindAsync(id);
            db.TransferMoneyUsers.Remove(transferMoneyUser);
            await db.SaveChangesAsync();
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
