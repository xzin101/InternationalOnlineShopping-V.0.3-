
using InternationalOnlineShopping.Controllers;
using InternationalOnlineShopping.Models;
using InternationalOnlineShopping.Repository;
using InternationalOnlineShopping.Utility;
using OnlineShopping.Controllers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace InternationalOnlineShopping.Filters
{
    public class FrontPageActionFilter : FilterAttribute, IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            dynamic controller;
            string controllerName = filterContext.RequestContext.HttpContext.Request.RawUrl.Split('/')[1].ToLower();
            switch (controllerName)
            {
                case "home":
                    controller = (HomeController)filterContext.Controller;
                    break;
                //case "search":
                //    controller = (SearchController)filterContext.Controller;
                //    break;
                //case "account":
                //    controller = (AccountController)filterContext.Controller;
                //    break;
                //case "admin":
                //    controller = (AdminController)filterContext.Controller;
                //    break;
                case "shopping":
                controller = (ShoppingController)filterContext.Controller;
                break;
                default:
                    controller = (HomeController)filterContext.Controller;
                    break;
            }

            GenericUnitOfWork unitOfWork = controller.unitOfWork;
            filterContext.Controller.ViewBag.CategoryAndSubCategory = unitOfWork.GetRepositoryInstance<Category>().GetAllRecordsIQueryable().ToList();
        }

        void IActionFilter.OnActionExecuted(ActionExecutedContext filterContext)
        {
            //  throw new System.NotImplementedException();
        }
    }

}