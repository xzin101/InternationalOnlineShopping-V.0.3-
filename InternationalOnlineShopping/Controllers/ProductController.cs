using InternationalOnlineShopping.Models;
using InternationalOnlineShopping.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InternationalOnlineShopping.Controllers
{
    public class ProductController : Controller
    {
        public GenericUnitOfWork unitOfWork = new GenericUnitOfWork();
        // GET: Product
        public ActionResult Index()
        {
            if (Session["MemberId"] != null)
            {
                return View();

            }
            else
            {
               return RedirectToAction("Index", "Register");
            }
            
        }

        public ActionResult ViewAllProducts()
        {
            int memberId = Convert.ToInt32(Session["MemberId"]);
            return View(unitOfWork.GetRepositoryInstance<Product>().GetAllRecordsIQueryable().Where(i => i.IsDelete == false && i.MemberId==memberId ).ToList());

        }

        public ActionResult AddOrEdit(int id = 0)
        {
            Product product = new Product();
            product.CategoryList = new SelectList(unitOfWork.GetRepositoryInstance<Category>().GetAllRecordsIQueryable(), "CategoryId", "CategoryName");
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddOrEdit(Product product)
        {
            if (ModelState.IsValid)
            {
                if (product.ImageUpload != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(product.ImageUpload.FileName);
                    string extension = Path.GetExtension(product.ImageUpload.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    product.ProductImage = "~/Content/Images/Products/" + fileName;
                    product.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/Content/Images/Products/"), fileName));

               }
                product.MemberId = Convert.ToInt32(Session["MemberId"]);
                product.IsDelete = false;
                product.IsActive = true;
                product.CreatedDate = DateTime.Now;
                product.ModifiedDate = DateTime.Now;
                unitOfWork.GetRepositoryInstance<Product>().Add(product);
                // return RedirectToAction("ViewAllProducts");
                return View("Index");
            }
           
          

           // ViewBag.Categories = new SelectList(unitOfWork.GetRepositoryInstance<Category>().GetAllRecords(), "CategoryId", "CategoryName");
            
            return View(product);
           
        }
    }
}