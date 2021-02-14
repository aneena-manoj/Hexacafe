using Hexacafe.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hexacafe.Areas.Restaurent.Controllers
{
    public class AjaxCallController : Controller
    {
        // GET: Restaurent/AjaxCall
        // Ajax Request for Add Food Categories
        public JsonResult InsertFoodCategory(string CategoryName, string FoodMainID)
        {
            try
            {
                // Instance of Data Context
                using (var db = new DataContext())
                {
                    if (Session["RestaurentID"] != null)
                    {
                        // Check Same CategoryName and Food Main id does not exists
                        int foodmainid = Convert.ToInt32(FoodMainID);
                        int RestaurentID = Convert.ToInt32(Session["RestaurentID"].ToString());

                        // Check Above Categories is not added by Same Restaurant
                        var checkdata = db.FoodCategories.Where(x => x.CategoryName == CategoryName & x.FoodMainID == foodmainid & x.RestaurentID == RestaurentID).Take(1).Any();
                        // if nOt added
                        if (checkdata != true)
                        {
                            // Create Instance of Food Category Model
                            FoodCategory obj = new FoodCategory
                            {
                                CategoryName = CategoryName,
                                FoodMainID = foodmainid,
                                RestaurentID = RestaurentID

                            };
                            // Add Reference of Food Category
                            db.FoodCategories.Add(obj);
                            var result = db.SaveChanges();
                            // if Successfully Added
                            if (result > 0)
                            {
                                return Json("added", JsonRequestBehavior.AllowGet);
                            }
                            else
                            {
                                return Json("notadded", JsonRequestBehavior.AllowGet);
                            }
                        }
                        else
                        {
                            return Json("sameitem", JsonRequestBehavior.AllowGet);

                        }

                    }
                    else
                    {
                        return Json("sessionissue", JsonRequestBehavior.AllowGet);

                    }
                }
            }
            catch (Exception ee)
            {
                return Json("exception", JsonRequestBehavior.AllowGet);

            }
        }
        // Get Last Added Food Categories and per Food Main ID
        public JsonResult GetFoodCategory(string FoodMainID)
        {
            try
            {
                // Check User is Logged in or not
                if (Session["RestaurentID"] != null)
                {
                    using (var db = new DataContext())
                    {
                        // Check Same CategoryName and Food Main id does not exists
                        int foodmainid = Convert.ToInt32(FoodMainID);
                        int RestaurentID = Convert.ToInt32(Session["RestaurentID"].ToString());
                        // Fetch Food Categories
                        var returnfoodcategory = db.FoodCategories.Where(x => x.RestaurentID == RestaurentID & x.FoodMainID == foodmainid).ToList();
                        if (returnfoodcategory != null)
                        {

                            return Json(returnfoodcategory.ToList(), JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            //return Json("NoItem", JsonRequestBehavior.AllowGet);
                            return Json("", JsonRequestBehavior.AllowGet);
                        }

                    }

                    //}
                }
                else
                {
                    //return Json("sessionissue", JsonRequestBehavior.AllowGet);
                    return Json("", JsonRequestBehavior.AllowGet);

                }
            }
            catch (Exception ee)
            {
                return Json("", JsonRequestBehavior.AllowGet);

            }
        }
    }
}