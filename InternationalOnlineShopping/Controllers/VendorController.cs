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
    public class VendorController : Controller
    {
        
        public GenericUnitOfWork unitOfWork = new GenericUnitOfWork();
        
        // GET: Vendor
        public ActionResult Index()
        {
           
            return View(unitOfWork.GetRepositoryInstance<Member>().GetAllRecordsIQueryable().Where(i => i.IsDelete == false).ToList());
        }

        // GET: Vendor/Details/5
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

        // GET: Vendor/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Vendor/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( Member member)
        {
            //if (ModelState.IsValid)
            //{
            //    member.IsDelete = false;
            //    member.IsActive = true;
            //    member.Password = EncryptDecrypt.Encrypt(member.Password, true);
            //    unitOfWork.GetRepositoryInstance<Member>().Add(member);
               
            //    MemberRole memberRole = new MemberRole();
            //    memberRole.MemberId = member.MemberId;
            //    memberRole.RoleId = 2;
            //    unitOfWork.GetRepositoryInstance<MemberRole>().Add(memberRole);
            //    TempData["SuccessMessage"] = "Vendor Registered Successfully";
            //     return RedirectToAction("Index","Login");
               
            //}
          
             return View(member);
        }

        // GET: Vendor/Edit/5
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

        // POST: Vendor/Edit/5
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
                TempData["SuccessMessage"] = "Updated Successfully";
                return RedirectToAction("Index");
            }
            return View(member);
        }

        // GET: Vendor/Delete/5
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

        // POST: Vendor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Member member = unitOfWork.GetRepositoryInstance<Member>().GetFirstOrDefaultByParameter(i => i.MemberId == id);
            member.IsDelete = true;
            unitOfWork.GetRepositoryInstance<Member>().Update(member);
            unitOfWork.SaveChanges();
            TempData["SuccessMessage"] = "Deleted Successfully";
            return RedirectToAction("Index");
        }

        public JsonResult IsUserNameAvailable(string EmailId)
        {
            var EmailExist= unitOfWork.GetRepositoryInstance<Member>().GetFirstOrDefaultByParameter(i => i.EmailId == EmailId && i.IsDelete == false);
            return EmailExist == null ? Json(true, JsonRequestBehavior.AllowGet) : Json(false, JsonRequestBehavior.AllowGet);
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
