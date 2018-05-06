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

namespace InternationalOnlineShopping.Controllers
{
    public class CategoriesController : Controller
    {
        public GenericUnitOfWork unitOfWork = new GenericUnitOfWork();
        // GET: Categories
        public ActionResult Index()
        {
            //return View(db.Categories.ToList());
            return View(unitOfWork.GetRepositoryInstance<Category>().GetAllRecordsIQueryable().Where(i => i.IsDelete == false).ToList());

        }

        // GET: Categories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Category category = db.Categories.Find(id);
            Category category = unitOfWork.GetRepositoryInstance<Category>().GetFirstOrDefaultByParameter(i => i.CategoryId == id);

            if(category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // GET: Categories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CategoryId,CategoryName,IsActive,IsDelete")] Category category)
        {
            /*
            if (ModelState.IsValid)
            {
                db.Categories.Add(category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(category);
            */
            if(ModelState.IsValid)
            {

                category.IsDelete = false;
                category.IsActive = true;                
                unitOfWork.GetRepositoryInstance<Category>().Add(category);
                return RedirectToAction("Index");
            }

            return View(category);
        }

        // GET: Categories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Category category = db.Categories.Find(id);
            Category category = unitOfWork.GetRepositoryInstance<Category>().GetFirstOrDefaultByParameter(i => i.CategoryId == id);

            if(category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CategoryId,CategoryName,IsActive,IsDelete")] Category category)
        {
            if (ModelState.IsValid)
            {
                category.IsDelete = false;
                category.IsActive = true;
                unitOfWork.GetRepositoryInstance<Category>().Update(category);
                unitOfWork.SaveChanges();
               // db.Entry(category).State = EntityState.Modified;
               // db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        // GET: Categories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Category category = db.Categories.Find(id);
            Category category = unitOfWork.GetRepositoryInstance<Category>().GetFirstOrDefaultByParameter(i => i.CategoryId == id);
            category.IsDelete = true;
            unitOfWork.GetRepositoryInstance<Category>().Update(category);

            if(category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //   Category category = db.Categories.Find(id);
            //  db.Categories.Remove(category);
            //   db.SaveChanges();
            Category category = unitOfWork.GetRepositoryInstance<Category>().GetFirstOrDefaultByParameter(i => i.CategoryId == id);
            category.IsDelete = true;
            unitOfWork.GetRepositoryInstance<Category>().Update(category);
            unitOfWork.SaveChanges();
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
