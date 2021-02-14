using Hexacafe.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hexacafe.Areas.Restaurent.Controllers
{
    public class ControlPanelController : Controller
    {
        // GET: Restaurent/ControlPanel
        int RestaurentID;
        public ActionResult ControlPanelhome()
        {
            // Check Restaurent Owner has logged in or not
            if (checkloggedin() == true)
            {

                return View();
            }
            else
            {
                return RedirectToAction("Login", "RestaurentHome");
            }
        }
        // Method to Check Restaurent Logged in or not
        private bool checkloggedin()
        {
            if (Session["RestaurentID"] != null)
            {
                RestaurentID = Convert.ToInt32(Session["RestaurentID"].ToString());
                return true;
            }
            else
            {
                return false;

            }
        }
        public ActionResult AddFoodCategory()
        {
            // Check Restaurant admin Logged in 
            if (checkloggedin() == true)
            {
                try
                {
                    // Create instance of Data Context
                    using (var db = new DataContext())
                    {
                        // Get Previous Filled Categories
                        var checkpreviousfilleditem = db.FoodCategories.Where(x => x.RestaurentID == RestaurentID).ToList();
                        if (checkpreviousfilleditem != null)
                        {

                            ViewBag.FoodMainID = db.MainFoodTypes.ToList();
                            ViewBag.PreviousFoodCategories = checkpreviousfilleditem;
                            return View();
                        }
                        else
                        {
                            // Get Main Types of Food 
                            ViewBag.RestaurentCategory = db.MainFoodTypes.ToList();
                            return View();
                        }
                    }
                }
                catch (Exception ee)
                {
                    return Content("<script>alert('Something went Wrong');location.href='/'</script>");

                }

            }
            else
            {
                return RedirectToAction("Login", "RestaurentHome");
            }
        }
        public ActionResult AddFoodItem()
        {
            if (checkloggedin() == true)
            {
                try
                {
                    using (var db = new DataContext())
                    {
                        // Get Previous Filled Categories
                        var checkpreviousfilleditem = db.FoodCategories.Where(x => x.RestaurentID == RestaurentID).ToList();
                        if (checkpreviousfilleditem != null)
                        {
                            ViewBag.FoodMainID = db.MainFoodTypes.ToList();
                            // Get Previous Filled Food Items
                            var previousfooditems = db.MenuItems.Where(x => x.restaurentid == RestaurentID).ToList();
                            ViewBag.FoodItemsList = previousfooditems;
                            //ViewBag.PreviousFoodCategories = checkpreviousfilleditem;
                            return View();
                        }
                        else
                        {
                            ViewBag.FoodMainID = db.MainFoodTypes.ToList();
                            return View();
                        }
                    }
                }
                catch (Exception ee)
                {
                    return Content("<script>alert('Something went Wrong');location.href='/'</script>");

                }

            }
            else
            {
                return RedirectToAction("Login", "RestaurentHome");
            }
        }
        [HttpPost]
        public ActionResult AddFoodItem(MenuItem obj, HttpPostedFileBase itempic)
        {
            if (ModelState.IsValid)
            {
                string guidname = Guid.NewGuid().ToString();
                string filename = string.Empty;
                string filepath = string.Empty;

                filename = guidname + itempic.FileName;
                string ext = Path.GetExtension(filename);
                if (ext == ".jpg" || ext == ".png")
                {
                    using (var db = new DataContext())
                    {
                        filepath = Server.MapPath("~//Files//");
                        itempic.SaveAs(filepath + filename);
                        obj.itempic = filename;
                        int RestaurentID = Convert.ToInt32(Session["RestaurentID"].ToString());
                        obj.restaurentid = RestaurentID;
                        db.MenuItems.Add(obj);
                        int result = db.SaveChanges();
                        if (result > 0)
                        {
                            return Content("<script>alert('Menu Item added Successfully');location.href='/Restaurent/ControlPanel/AddFoodItem'</script>");
                        }
                    }
                }
                else
                {
                    return Content("<script>alert('You may upload only jpg and png files only');location.href='/Restaurent/ControlPanel/AddFoodItem'</script>");
                }
            }
            else
            {
                return View();
            }
            return View();
        }
        public ActionResult ViewOrders()
        {
            if (checkloggedin() == true)
            {
                try
                {
                    using (var db = new DataContext())
                    {
                        // Get Previous Filled Categories
                        int RestaurentID = Convert.ToInt32(Session["RestaurentID"]);
                        var custdata = (from c in db.MenuItems join d in db.Orders on c.menuitemid equals d.MenuItemID join RR in db.RestaurentRegistrations on d.RestaurentID equals RR.RestaurentId join u in db.Users on d.OrderByUser equals u.userid where RR.RestaurentId == RestaurentID select new CustomerVM { MenuItem = c, Order = d, RestaurentRegistration = RR, User = u }).ToList();
                        return View(custdata);
                    }
                }
                catch (Exception ee)
                {
                    return Content("<script>alert('Something went Wrong');location.href='/'</script>");

                }

            }
            else
            {
                return RedirectToAction("Login", "RestaurentHome");
            }
        }
    }
}