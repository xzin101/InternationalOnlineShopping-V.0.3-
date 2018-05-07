using InternationalOnlineShopping.Models;
using InternationalOnlineShopping.Repository;
using InternationalOnlineShopping.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace InternationalOnlineShopping.Controllers
{
    public class AdminController : Controller
    {
      
        #region Other Class references ...
        // Instance on Unit of Work
        private GenericUnitOfWork _unitOfWork = new GenericUnitOfWork();
        // UploadContent uc = new UploadContent();
        #endregion

        #region Admin Login ...
        /// <summary>
        /// Login Page
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        /// 
        [AllowAnonymous]
        public ActionResult Index(string returnUrl)
        {
            LoginViewModel loginModel = new LoginViewModel();

            return View(loginModel);

        }

        /// <summary>
        /// Login
        /// </summary>
        /// <param name="model"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Index(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                string EncryptedPassword = EncryptDecrypt.Encrypt(model.Password, true);
                var user = _unitOfWork.GetRepositoryInstance<Member>().GetFirstOrDefaultByParameter(i => i.EmailId == model.UserEmailId && i.Password == EncryptedPassword && i.IsActive == true && i.IsDelete == false);
                if (user != null)
                {
                    Session["MemberId"] = user.MemberId;
                    Response.Cookies["MemberName"].Value = user.FirstName;
                    var roles = _unitOfWork.GetRepositoryInstance<MemberRole>().GetFirstOrDefaultByParameter(i => i.MemberId == user.MemberId && i.RoleId == model.UserType);
                    if (roles != null)
                    {
                        Response.Cookies["MemberRole"].Value = _unitOfWork.GetRepositoryInstance<Role>().GetFirstOrDefaultByParameter(i => i.RoleId == model.UserType).RoleName;
                    }
                    else
                    {
                        //  ModelState.AddModelError("Password", "Password is wrong");
                        //  return View(model);
                        Redirect(returnUrl != null ? returnUrl : "/admin/dashboard");
                     

                    }
                   
                    return Redirect(returnUrl != null ? returnUrl : "/admin/dashboard");
                }
                else
                {
                    ModelState.AddModelError("Password", "Invalid email address or password");
                }
            }
            return View(model);
        }

        #endregion

        #region Admin Dashboard ...
        /// <summary>
        /// Admin Dashboard
        /// </summary>
        /// <returns></returns>
        public ActionResult Dashboard()
        {
            return View();
        }
        #endregion

    }
}
