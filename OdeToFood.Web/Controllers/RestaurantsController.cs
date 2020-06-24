using OdeToFood.Data.Models;
using OdeToFood.Data.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OdeToFood.Web.Controllers
{
    public class RestaurantsController : Controller
    {
        private readonly IRestaurantData db;

        // GET: Restaurants
        public RestaurantsController(IRestaurantData db)
        {
            this.db = db;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var model = db.GetAll();
            return View(model);
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            var model = db.Get(id);
            if (model == null)
            {
                return View("NotFound");
            }
            return View(model);
        }

        // 1st of two action methods; this one gets data from the user
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        // 2nd of two action methods; this one processess data from the user
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Restaurant restaurant)
        {
            /*  The Vericom instructor avised removing the following 4 lines of code to prepare for
             *  introducing an easier way to do validation (data annotation).
             *  
            var restaurantName = restaurant.Name.Trim();    // handles the case when the name contains only white space
            if (String.IsNullOrEmpty(restaurantName))
            {       
                ModelState.AddModelError(nameof(restaurant.Name), "The name is required." );
            }
            */
            if (ModelState.IsValid)
            {
                db.Add(restaurant);
                return RedirectToAction("Details", new { id = restaurant.Id });
                // The 2nd argument is an object of an anonymous type that has an 'id' property.
            }
            return View();			// displays the diagnostic
        }

        [HttpGet]
        public ActionResult Edit(int id)    // 1st of action-item pair
        {
            var model = db.Get(id);
            if (model == null)
            {
                return View("NotFound");
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Restaurant restaurant) // 2nd of action-item pair
        {
            if (ModelState.IsValid)
            {
                db.Update(restaurant);
                return RedirectToAction("Details", new { id = restaurant.Id });
                // The 2nd argument is an object of an anonymous type that has an 'id' property.
            }
            return View(restaurant);			// displays the diagnostic
        }

        [HttpGet]
        public ActionResult Delete(int id)  // 1st of action-item pair
        {
            var model = db.Get(id);
            if (model == null)
            {
                return View("NotFound");
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, FormCollection form) // 2nd of action-item pair
        {
            db.Delete(id);
            return RedirectToAction("Index");
        }
    }
}