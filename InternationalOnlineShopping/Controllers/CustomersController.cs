using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using InternationalOnlineShopping.Models;
using InternationalOnlineShopping.Repository;
using InternationalOnlineShopping.Utility;

namespace InternationalOnlineShopping.Controllers
{
    public class CustomersController : Controller
    {
        private OnlineShoppingEntities db = new OnlineShoppingEntities();
        public GenericUnitOfWork unitOfWork = new GenericUnitOfWork();
        // GET: Members
        public ActionResult Index()
        {
            return View(unitOfWork.GetRepositoryInstance<Member>().GetAllRecordsIQueryable().Where(i => i.IsDelete == false).ToList());
        }

        // GET: Members/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = unitOfWork.GetRepositoryInstance<Member>().GetFirstOrDefaultByParameter(i => i.MemberId == id);
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }

        // GET: Members/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Members/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MemberId,FirstName,LastName,EmailId,Password,IsActive,IsDelete,CreatedOn,ModifiedOn,VendorName")] Member member)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    member.IsDelete = false;
                    member.IsActive = true;
                    member.VendorName = null;
                    member.Password = EncryptDecrypt.Encrypt(member.Password, true);
                    unitOfWork.GetRepositoryInstance<Member>().Add(member);

                    MemberRole memberRole = new MemberRole();
                    memberRole.MemberId = member.MemberId;
                    memberRole.RoleId = 1;
                    unitOfWork.GetRepositoryInstance<MemberRole>().Add(memberRole);
                    return RedirectToAction("Index");
                }

               
            }
            catch (Exception ex)
            {
                throw ex;
            }




            return View("Index");
        }

        // GET: Members/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = unitOfWork.GetRepositoryInstance<Member>().GetFirstOrDefaultByParameter(i => i.MemberId == id);
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }

        // POST: Members/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MemberId,FirstName,LastName,EmailId,Password,IsActive,IsDelete,CreatedOn,ModifiedOn,VendorName")] Member member)
        {
            if (ModelState.IsValid)
            {
                member.IsDelete = false;
                member.IsActive = true;
                unitOfWork.GetRepositoryInstance<Member>().Update(member);
                unitOfWork.SaveChanges();
                //db.Entry(member).State = EntityState.Modified;
                //db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(member);
        }

        // GET: Members/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = unitOfWork.GetRepositoryInstance<Member>().GetFirstOrDefaultByParameter(i => i.MemberId == id);
            member.IsDelete = true;
            unitOfWork.GetRepositoryInstance<Member>().Update(member);
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }

        // POST: Members/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Member member = unitOfWork.GetRepositoryInstance<Member>().GetFirstOrDefaultByParameter(i => i.MemberId == id);
            member.IsDelete = true;
            unitOfWork.GetRepositoryInstance<Member>().Update(member);
            unitOfWork.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                unitOfWork.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
