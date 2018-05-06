using InternationalOnlineShopping.Filters;
using InternationalOnlineShopping.Models;
using InternationalOnlineShopping.Repository;
using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;


namespace InternationalOnlineShopping.Controllers
{
    [FrontPageActionFilter]
    public class HomeController : Controller
    {
        #region Other Class references ... // Instance on Unit of Work 
        public GenericUnitOfWork unitOfWork = new GenericUnitOfWork();
        #endregion
        /// <summary>
        /// Home Page
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewBag.FeaturedProducts = unitOfWork.GetRepositoryInstance<Product>().GetListByParameter(i => i.IsFeatured == true && i.IsDelete == false && i.IsActive == true).ToList();
            return View();
        }

        #region Disposing UnitOfWork Context ...
        protected override void Dispose(bool disposing)
        {
            unitOfWork.Dispose();
            base.Dispose(disposing);
        }
        #endregion
    }
}