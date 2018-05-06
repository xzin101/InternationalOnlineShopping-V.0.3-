using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InternationalOnlineShopping.Models;
using InternationalOnlineShopping.Repository;

namespace InternationalOnlineShopping.Controllers
{
    public class ProductDetailController : Controller
    {
        public GenericUnitOfWork unitOfWork = new GenericUnitOfWork();
        public ActionResult ProductDetail(int pId)
        {
            System.Diagnostics.Debug.WriteLine("---------");
            Product pd = unitOfWork.GetRepositoryInstance<Product>().GetFirstOrDefault(pId);
            ViewBag.SimilarProducts = unitOfWork.GetRepositoryInstance<Product>().GetListByParameter(i => i.CategoryId == pd.CategoryId).ToList();
            return View(pd);
        }
    }
}