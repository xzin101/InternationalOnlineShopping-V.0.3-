using InternationalOnlineShopping.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InternationalOnlineShopping.Models;
using System.Data.SqlClient;
using System.Data;

namespace InternationalOnlineShopping.Controllers
{
    public class SearchController: Controller
    {
        public GenericUnitOfWork unitOfWork = new GenericUnitOfWork();
        /// <summary>
        /// Product Detail
        /// </summary>
        /// <param name="pId"></param>
        /// <returns></returns>
        public ActionResult ProductDetail(int pId)
        {

            System.Diagnostics.Debug.WriteLine("---");
            Product pd = unitOfWork.GetRepositoryInstance<Product>().GetFirstOrDefault(pId);
            ViewBag.SimilarProducts = unitOfWork.GetRepositoryInstance<Product>().GetListByParameter(i => i.CategoryId == pd.CategoryId).ToList();
            return View(pd);
        }
    }
}