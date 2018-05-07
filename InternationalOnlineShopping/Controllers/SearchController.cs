using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Data;
using InternationalOnlineShopping.Repository;
using InternationalOnlineShopping.Models;
using InternationalOnlineShopping.Filters;

namespace InternationalOnlineShopping.Controllers
{

    [FrontPageActionFilter]
    public class SearchController : Controller
    {

        #region Other Class references ...
        // Instance on Unit of Work
        public GenericUnitOfWork unitOfWork = new GenericUnitOfWork();
        #endregion
        /// <summary>
        /// Search result page
        /// </summary>
        /// <param name="searchKey"></param>
        /// <returns></returns>
        public ActionResult Index(string searchKey = "")
        {
            ViewBag.searchKey = searchKey;
            List<USP_Search_Result> sr = unitOfWork.GetRepositoryInstance<USP_Search_Result>().GetResultBySqlProcedure
            ("USP_Search @searchKey", new SqlParameter("searchKey", SqlDbType.VarChar) { Value = searchKey }).ToList();
            return View(sr);
        }
    }
}