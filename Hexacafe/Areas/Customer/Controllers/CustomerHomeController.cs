using Hexacafe.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace Hexacafe.Areas.Customer.Controllers
{
    public class CustomerHomeController : Controller
    {
        // GET: Customer/Customer
        Guid guid;
        public ActionResult SignUp()
        {
            //Customer signup page 
            //instance of context class
            using (var db = new DataContext())
            {
                // Get List of Data from Main Food Types
                ViewBag.RestaurentCategory = db.MainFoodTypes.ToList();
                return View();
            }
        }
        [HttpPost]
        public ActionResult SignUp(User obj)
        {
            // Check Model Validations
            if (ModelState.IsValid)
            {
                // Instance of Data Context
                using (var db = new DataContext())
                {
                    try
                    {
                        #region Code For Registration
                        // First Check that Email ID is already registered and Verified
                        var checkemail = db.Users.Where(x => x.email == obj.email & x.emailvarified == true).Take(1).Any();
                        // if Already Registered then show message otherwise proceed
                        if (checkemail == true)
                        {
                            return Content("<script>alert('This Email ID is aleray Registered and Verified.');location.href='/Customer/CustomerHome/SignUp';</script>");

                        }
                        else
                        {
                            // Create newUnique identifier
                            guid = Guid.NewGuid();
                            obj.guid = guid;
                            obj.regdate = DateTime.Now.ToShortDateString();
                            // Add Customer/User Details in User Table
                            db.Users.Add(obj);
                            var result = db.SaveChanges();
                            #endregion
                            #region Sending Email on Registered Email ID for Account Verification
                            // Generate Unique ID for Email Verification                       
                            string msg = "Account Verification ";
                            MailMessage mail = new MailMessage();
                            mail.To.Add(obj.email.Trim().Replace("'", "''"));
                            mail.From = new MailAddress("hexacafe20@gmail.com");
                            mail.Subject = "Customer Account Varification ";
                            string Body = "Please click below on given link for your Account Varification <br/> ";
                            Body += "http://localhost:61932/Customer/CustomerHome/AccountVerification?uniqueid=" + guid;

                            Body += "<br /> Regards:" + "<br />" + "Online Restaurent";
                            Body += "<br /> Contact for assistance: 0141-000000";
                            mail.Body = Body;
                            mail.IsBodyHtml = true;
                            // Adding the Creddentials
                            System.Net.NetworkCredential networkcredential = new System.Net.NetworkCredential("hexacafe20@gmail.com", "Hexa@2020");
                            SmtpClient smtp = new SmtpClient();
                            smtp.EnableSsl = false;
                            smtp.UseDefaultCredentials = false;
                            smtp.Credentials = networkcredential;
                            smtp.Port = 587; //25 or use 587 or 465
                            smtp.Host = "smtp.gmail.com";
                            smtp.Send(mail);
                            #endregion
                            // if Successfully Inserted Show Message
                            if (result > 0)
                            {
                                return Content("<script>alert('Your Restaurent has been Successfully Registered. An Account activation link has been sent to your email id , please verify your account');location.href='/'</script>");
                            }
                            return View();
                        }
                    }
                    // If Exception thrown
                    catch (Exception ee)
                    {
                        return Content("<script>alert('Something went Wrong');location.href='/'</script>");

                    }
                }
            }
            else
            {
                return View();
            }
        }
        // User Email Verification
        public ActionResult AccountVerification(string uniqueid)
        {
            // Check Unique code is associated with URL
            if (uniqueid != null)
            {
                // Data Context Instance
                using (var db = new DataContext())
                {
                    try
                    {
                        // Check Unique Identifier value if ok then update emailvarified
                        // Disable Model Validation
                        db.Configuration.ValidateOnSaveEnabled = false;
                        // Check Unique id with database Users Table
                        var checkaccount = db.Users.Where(x => x.guid.ToString() == uniqueid).FirstOrDefault();
                        if (checkaccount != null)
                        {
                            // Update EmailVerified field in Customer Registration
                            checkaccount.emailvarified = true;
                            checkaccount.regdate = DateTime.Now.ToShortDateString();
                            TryUpdateModel(checkaccount);
                            var data2 = db.SaveChanges();
                            // If Account Verification done Successfully
                            if (data2 > 0)
                            {
                                return Content("<script>alert('Your Account is Successfully Verified and Now You may Login');location.href='/Customer/CustomerHome/Login'</script>");

                            }
                            return View();
                        }
                        else
                        {
                            return Content("<script>alert('You have changed something in Activation URL');location.href='/'</script>");

                        }
                    }
                    catch (Exception ee)
                    {
                        return Content("<script>alert('Something went Wrong');location.href='/'</script>");


                    }

                }
            }
            else
            {
                return RedirectToAction("SignUp");
            }
        }
        // Customer Login View
        public ActionResult Login(string itemtype)
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            // Verify model validation
            if (ModelState.IsValid)
            {
                try
                {
                    // Check Email and Password
                    using (var db = new DataContext())
                    {
                        // Compare Username and Password with database table
                        var checklogin = db.Users.Where(x => x.email == email & x.password == password).FirstOrDefault();
                        if (checklogin != null)
                        {
                            // Check Email Verification is done or not
                            var checkverification = db.Users.Where(x => x.email == email & x.password == password & x.emailvarified == true).Take(1).Any();
                            if (checkverification == true)
                            {
                                // CustomerID and Customer Preferences saved
                                Session.Add("CustomerID", checklogin.userid);
                                Session.Add("CustomerPreference", checklogin.userfoodpreference);
                                // If Customer is coming from Add to Cart Page then save his last search item 
                                if (Session["loginissue_in_order"] != null)
                                {
                                    return RedirectToAction("ViewMenu", "Menu", new { itemtype = Session["loginissue_in_order"].ToString() });

                                }
                                else
                                {
                                    return Content("<script>location.href='/'</script>");
                                }
                            }
                            else
                            {
                                return Content("<script>alert('Your Email Verification has not done yet, So please check your email and verify account');location.href='/'</script>");

                            }
                        }
                        else
                        {
                            return Content("<script>alert('Invalid Username or Password');location.href='/Customer/CustomerHome/Login';</script>");

                        }
                    }
                }
                catch (Exception ee)
                {
                    return Content("<script>alert('Something went Wrong');location.href='/'</script>");

                }
            }
            return View();
        }
        // Logout
        public ActionResult CustomerLogout()
        {
            // Remove all Session instances
            Session.RemoveAll();
            Session.Abandon();
            Session.Clear();
            // Redirect to Home Page
            return Content("<script>alert('Successfully Logged Out');location.href='/'</script>");
        }

    }
}