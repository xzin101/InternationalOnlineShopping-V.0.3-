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
    public class RegisterController : Controller
    {
        public GenericUnitOfWork unitOfWork = new GenericUnitOfWork();

        // GET: Register
        public ActionResult Index()
        {
            return View();
        }

        // GET: Register/Details/5
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

      

        // GET: Register/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Register/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MemberId,FirstName,LastName,EmailId,Password,IsActive,IsDelete,CreatedOn,ModifiedOn,VendorName,City,State,Country,ZipCode,RegistrationFee,ContactNo")] Member member)
        {

            member.IsDelete = false;
            member.IsActive = true;
            member.Password = EncryptDecrypt.Encrypt(member.Password, true);
            member.CreatedOn = DateTime.Now;
            member.ModifiedOn = DateTime.Now;
            MemberRole memberRole = new MemberRole();
            if (member.RegistrationFee == null && member.FirstName != null && member.LastName != null)
            {
                memberRole.RoleId = 1;
                member.VendorName = "N/A";
                member.RegistrationFee = 0;
                TempData["SuccessMessage"] = "Customer Registered Successfully";
            }
            else
            {
                memberRole.RoleId = 2;
                member.FirstName = "N/A";
                member.LastName = "N/A";
                TempData["SuccessMessage"] = "Vendor Registered Successfully";
            }
            unitOfWork.GetRepositoryInstance<Member>().Add(member);

            memberRole.MemberId = member.MemberId;

            //if (ModelState.IsValid)
            //{



            unitOfWork.GetRepositoryInstance<MemberRole>().Add(memberRole);
           
            return RedirectToAction("Index");


            // }

            // return View();


        }


        #region Member Login ...         
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Member model)
        {
           // string result = "Fail";
            string EncryptedPassword = EncryptDecrypt.Encrypt(model.LoginViewModel.Password, true);
            var user = unitOfWork.GetRepositoryInstance<Member>().GetFirstOrDefaultByParameter(i => i.EmailId == model.LoginViewModel.UserEmailId && i.Password == EncryptedPassword && i.IsDelete == false);
            if (user != null && user.IsActive == true)
            {
                Session["MemberId"] = user.MemberId;
                var roles = unitOfWork.GetRepositoryInstance<MemberRole>().GetFirstOrDefaultByParameter(i => i.MemberId == user.MemberId);
                if (roles != null)
                {
                    // int roleId = unitOfWork.GetRepositoryInstance<Role>().GetFirstOrDefaultByParameter(i => i.RoleId == roles.RoleId).RoleId;
                    Response.Cookies["RoleId"].Value = roles.RoleId.ToString();

                    if (roles.RoleId == 1)
                    {
                        Session["MemberName"] = user.FirstName;
                    }
                    else
                    {
                        Session["MemberName"] = user.VendorName;

                    }
                    //result = "Success";
                }
                else
                {
                    //ViewBag.ErrorMsg = "Invalid Username or Password!";
                    //return View("ErrorTransaction");
                    ModelState.AddModelError("Password","Invalid username or password");
                    return View(model);

                }


                if (roles.RoleId == 2)
                {
                    return RedirectToAction("Index", "Products");
                }
                else if (roles.RoleId == 1)
                {
                    return RedirectToAction("Index", "Home");
                }

                else
                    return RedirectToAction("Dashboard", "Admin");
                    
            }
            else
            {
                ModelState.AddModelError("Password", "Invalid username or password");
                //if (user != null && user.IsActive == false) ModelState.AddModelError("Password", "Your account in not verified");
                //else ModelState.AddModelError("Password", "Invalid username or password");
                TempData["SuccessMessage"] = "Invalid username or password";
                 return View("Index", model);
                //ViewBag.ErrorMsg = "Invalid Username or Password!";
                //return View("ErrorLogin");

            }
          
            //else
            //{
            //    if (user != null && user.IsActive == false) ModelState.AddModelError("Password", "Your account in not verified");
            //    else ModelState.AddModelError("Password", "Invalid username or password");
            //}

            //return View("Index");
            // return PartialView("_Login", model);
        }
        #endregion MemberLogin..



        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Index","Home");
        }
        
        
        
        
        
        // GET: Register/Edit/5
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

        // POST: Register/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MemberId,FirstName,LastName,EmailId,Password,IsActive,IsDelete,CreatedOn,ModifiedOn,VendorName,City,State,Country,ZipCode,RegistrationFee,ContactNo")] Member member)
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

        // GET: Register/Delete/5
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





        // POST: Register/Delete/5
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
            var EmailExist = unitOfWork.GetRepositoryInstance<Member>().GetFirstOrDefaultByParameter(i => i.EmailId == EmailId && i.IsDelete == false);
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
