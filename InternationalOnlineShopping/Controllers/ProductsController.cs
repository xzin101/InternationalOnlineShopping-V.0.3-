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
using System.IO;

namespace InternationalOnlineShopping.Controllers
{
    public class ProductsController : Controller
    {
      
        public GenericUnitOfWork unitOfWork = new GenericUnitOfWork();
        // GET: Products
        public ActionResult Index()
        {
            if (Session["MemberId"] != null)
            {
                //return View();
                int memberId = Convert.ToInt32(Session["MemberId"]);
                //ViewBag.Categories = new SelectList(unitOfWork.GetRepositoryInstance<Category>().GetAllRecords(),"CategoryId","CategoryName");
                return View(unitOfWork.GetRepositoryInstance<Product>().GetAllRecordsIQueryable().Where(i => i.IsDelete == false && i.MemberId == memberId).ToList());

            }
            else
            {
                return RedirectToAction("Index", "Register");
            }


          
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = unitOfWork.GetRepositoryInstance<Product>().GetFirstOrDefaultByParameter(i => i.ProductId == id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            Product product = new Product();
            product.CategoryList = new SelectList(unitOfWork.GetRepositoryInstance<Category>().GetAllRecordsIQueryable(),"CategoryId","CategoryName");
            return View(product);
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductId,ProductName,CategoryId,IsActive,IsDelete,CreatedDate,ModifiedDate,Description,ProductImage,Price,IsFeatured,MemberId")] Product product, HttpPostedFileBase ImageUpload)
        {
            if(ModelState.IsValid)
            {
                if(ImageUpload != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(ImageUpload.FileName);
                    string extension = Path.GetExtension(ImageUpload.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    product.ProductImage = "~/Content/Images/Products/" + fileName;
                    ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/Content/Images/Products/"),fileName));

                }
                product.MemberId = Convert.ToInt32(Session["MemberId"]);
              
                product.IsDelete = false;
                product.IsActive = true;
                product.CreatedDate = DateTime.Now;
                product.ModifiedDate = DateTime.Now;
                unitOfWork.GetRepositoryInstance<Product>().Add(product);
                // return RedirectToAction("ViewAllProducts");
                TempData["SuccessMessage"] = "Product Added Successfully";
                return RedirectToAction("Index");
            }

            //ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName", product.CategoryId);
           // ViewBag.MemberId = new SelectList(db.Members, "MemberId", "FirstName", product.MemberId);
            return View(product);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            int memberId = Convert.ToInt32(Session["MemberId"]);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = unitOfWork.GetRepositoryInstance<Product>().GetFirstOrDefaultByParameter(i => i.ProductId == id && i.MemberId==memberId && i.IsDelete==false);
            if (product == null)
            {
                return HttpNotFound();
            }
            //ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName", product.CategoryId);
            //ViewBag.MemberId = new SelectList(db.Members, "MemberId", "FirstName", product.MemberId);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductId,ProductName,CategoryId,IsActive,IsDelete,CreatedDate,ModifiedDate,Description,ProductImage,Price,IsFeatured,MemberId")] Product product)
        {
            //  if (ModelState.IsValid)
            //  {
            //      db.Entry(product).State = EntityState.Modified;
            //      db.SaveChanges();
            //      return RedirectToAction("Index");
            //  }
            //  ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName", product.CategoryId);
            //// ViewBag.MemberId = new SelectList(db.Members, "MemberId", "FirstName", product.MemberId);
            //  return View(product);

            if (ModelState.IsValid)
            {
                product.IsDelete = false;
                product.IsActive = true;
                unitOfWork.GetRepositoryInstance<Product>().Update(product);
                unitOfWork.SaveChanges();
                //db.Entry(member).State = EntityState.Modified;
                //db.SaveChanges();
                TempData["SuccessMessage"] = "Updated Successfully";
                return RedirectToAction("Index");
            }
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = unitOfWork.GetRepositoryInstance<Product>().GetFirstOrDefaultByParameter(i => i.ProductId == id);
            product.IsDelete = true;
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = unitOfWork.GetRepositoryInstance<Product>().GetFirstOrDefaultByParameter(i => i.ProductId == id);
            product.IsDelete = true;
            unitOfWork.GetRepositoryInstance<Product>().Update(product);
            unitOfWork.SaveChanges();
            TempData["SuccessMessage"] = "Deleted Successfully";
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
