using Hexacafe.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace Hexacafe.Areas.Restaurent.Controllers
{
    public class RestaurentHomeController : Controller
    {
        Guid guid;

        // GET: Restaurent/RestaurentHome
        [ActionName("SignUp")]
        public ActionResult SignUp()
        {
            using (var db = new DataContext())
            {
                // Get Categories of Restaurent Type
                ViewBag.RestaurentCategory = db.MainFoodTypes.ToList();
                return View();

            }
        }
        [HttpPost]
        public ActionResult SignUp(RestaurentRegistration obj)
        {
            if (ModelState.IsValid)
            {
                using (var db = new DataContext())
                {
                    try
                    {
                        #region Code For Registration
                        // First Check that Email ID is already registered and Verified
                        var checkemail = db.RestaurentRegistrations.Where(x => x.RestaurentEmail == obj.RestaurentEmail & x.emailvarified == true).Take(1).Any();
                        // if Already Registered then show message otherwise proceed
                        if (checkemail == true)
                        {
                            return Content("<script>alert('This Email ID is aleray Registered and Verified.');location.href='/Restuarent/RestaurentHome/SignUp';</script>");

                        }
                        else
                        {
                            // Create newUnique identifier
                            guid = Guid.NewGuid();
                            obj.guid = guid;
                            //obj.Regdate = DateTime.Now.ToShortDateString();
                            db.RestaurentRegistrations.Add(obj);
                            var result = db.SaveChanges();
                            #endregion
                            #region Sending Email on Registered Email ID for Account Verification
                            // Generate Unique ID for Email Verification                       
                            string msg = "Account Verification ";
                            MailMessage mail = new MailMessage();
                            mail.To.Add(obj.RestaurentEmail.Trim().Replace("'", "''"));
                            mail.From = new MailAddress("youremail");
                            mail.Subject = "Online Restaurant: Account Varification Activation";
                            string Body = "Please click below on given link for your Account Varification <br/> ";


                            Body += "<br /> Regards:" + "<br />" + "Online Restaurent";
                            Body += "<br /> Contact for assistance: 0141-000000";
                            mail.Body = Body;
                            mail.IsBodyHtml = true;
                            // Adding the Creddentials
                            System.Net.NetworkCredential networkcredential = new System.Net.NetworkCredential("youremail", "youremailpassword");
                            SmtpClient smtp = new SmtpClient();
                            smtp.EnableSsl = false;
                            smtp.UseDefaultCredentials = false;
                            smtp.Credentials = networkcredential;
                            smtp.Port = 25; //25 or use 587 or 465
                            smtp.Host = "cegrajasthan.org";
                            smtp.Send(mail);
                            #endregion
                            if (result > 0)
                            {
                                return Content("<script>alert('Your Restaurant has been Successfully Registered. An Account activation link has been sent to your email id , please verify your account');location.href='/'</script>");
                            }
                            return View();
                        }
                    }
                    catch (Exception ee)
                    {
                        return Content("<script>alert('Something went Wrong1');location.href='/'</script>");

                    }
                }
            }
            else
            {
                return View();
            }
        }
        public ActionResult AccountVerification(string uniqueid)
        {
            if (uniqueid != null)
            {
                using (var db = new DataContext())
                {
                    try
                    {
                        // Check Unique Identifier value if ok then update emailvarified
                        var checkaccount = db.RestaurentRegistrations.Where(x => x.guid.ToString() == uniqueid).FirstOrDefault();
                        if (checkaccount != null)
                        {
                            // Update EmailVerified field in Restaurent Registration
                            checkaccount.emailvarified = true;
                            checkaccount.Regdate = DateTime.Now.ToShortDateString();
                            ModelState.Clear();
                            TryUpdateModel(checkaccount);
                            // db.Entry(CollegeAllottment_OutofRajasthan).CurrentValues.SetValues(data);
                            var data2 = db.SaveChanges();
                            if (data2 > 0)
                            {
                                return Content("<script>alert('Your Account is Successfully Verified and Now You may Login');location.href='/Restaurent/RestaurentHome/Login';</script>");

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
                        return Content("<script>alert('Something went Wrong2');location.href='/'</script>");

                    }

                }
            }
            else
            {
                return RedirectToAction("SignUp");
            }
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string RestaurentEmail, string password)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Check Email and Password
                    using (var db = new DataContext())
                    {
                        var checklogin = db.RestaurentRegistrations.Where(x => x.RestaurentEmail == RestaurentEmail & x.password == password).FirstOrDefault();
                        if (checklogin != null)
                        {
                            // Check Email Verification is done or not
                            var checkverification = db.RestaurentRegistrations.Where(x => x.RestaurentEmail == RestaurentEmail & x.password == password & x.emailvarified == true).Take(1).Any();
                            if (checkverification == true)
                            {
                                Session.Add("RestaurentID", checklogin.RestaurentId);
                                return RedirectToAction("ControlPanelhome", "ControlPanel");
                            }
                            else
                            {
                                return Content("<script>alert('Your Email Verification has not done yet, So please check your email and verify account');location.href='/'</script>");

                            }
                        }
                        else
                        {
                            return Content("<script>alert('Invalid Username or Password');location.href='/Restaurent/RestaurentHome/Login';</script>");

                        }
                    }
                }
                catch (Exception ee)
                {
                    return Content("<script>alert('Something went Wrong3');location.href='/'</script>");

                }
            }
            return View();
        }
    }
}