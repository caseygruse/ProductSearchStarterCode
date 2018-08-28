using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProductSearchStarterCode.Models;

namespace ProductSearchStarterCode.Controllers
{
    public class ProductsController : Controller
    {
        private ProductContext db = new ProductContext();
		/// <summary>
		/// Filter/search products.  //use a get request and return to a get request.
		/// but with a partial view of results
		/// </summary>
		/// <returns></returns>
		/// [HttpGet]
		public ActionResult Search(ProductSearchViewModel search)
		{

			if (UserHasNotSearched(search))
			{
				return View(search);
			}
			//string maxPrice = Request.Form["MaxPrice"];
			//if(maxPrice == null{}      // one way but should use model binding like below

			// Select * From products
			// IQueryable results DO NOT Query the database
			IQueryable<Product> query = from p in db.Products select p;
			

			if (search.MaxPrice.HasValue)
			{
				//adds WHERE price < search.MaxPrice
				query = from p in query where p.Price <= search.MaxPrice.Value select p;
			}

			if (search.MinPrice.HasValue)
			{
				//Add minPrice to WHERE clause
				query = from p in query where p.Price >= search.MinPrice.Value select p;
			}
			if(search.ProductName != null)
			{
				query = from p in query where p.Title.Contains(search.ProductName) select p;
			}
			if(search.Category != null)
			{
				query = from p in query where p.Category == search.Category select p;
			}
			//SEND completed query to database and get products.
			search.SearchResults = query.ToList();
			return View(search);
		}

		private bool UserHasNotSearched(ProductSearchViewModel search)
		{
			if (search.MaxPrice == null && search.MinPrice == null && search.ProductName == null && search.Category == null)
			{
				return true;
			}
			else
				return false;
		}


		// GET: Products
		public ActionResult Index()
        {
            return View(db.Products.ToList());
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductId,Title,Price,Category")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(product);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductId,Title,Price,Category")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
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
            Product product = db.Products.Find(id);
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
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
