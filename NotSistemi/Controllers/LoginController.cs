using NotSistemi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;
using System.Web.UI.WebControls;

namespace NotSistemi.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        [AllowAnonymous]
        public ActionResult Login()
        {

            if (String.IsNullOrEmpty(HttpContext.User.Identity.Name))
            {



                if (Request.Cookies["UserPassword"] == null || Request.Cookies["UserName"] == null)
                {

                }
                else
                {
                    ViewBag.password = Request.Cookies["UserPassword"].Value;
                    ViewBag.username = Request.Cookies["UserName"].Value;
                }



                FormsAuthentication.SignOut();

                return View();
            }
            return Redirect("/Home/Index");

        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(User model)
        {
            if (ModelState.IsValid)
            {
                var apiUrl = "http://192.168.1.107:83/api/users/" + model.USER_NAME;
                Uri url = new Uri(apiUrl);
                WebClient client = new WebClient();
                client.Encoding = System.Text.Encoding.UTF8;
                try
                {
                    string json = client.DownloadString(url);
                    JavaScriptSerializer ser = new JavaScriptSerializer();
                    var user = ser.Deserialize<User>(json);

                    
                    if (model.USER_NAME == user.USER_NAME && model.USER_PASSWORD == user.USER_PASSWORD)
                    {

                        if (model.REMEMBERME == true)
                        {
                            HttpCookie cookieu = new HttpCookie("UserName", model.USER_NAME);
                            HttpCookie cookiep = new HttpCookie("UserPassword", model.USER_PASSWORD);
                            HttpCookie cookiename = new HttpCookie("Name", user.USER_FIRSTNAME + " " + user.USER_LASTNAME);
                            Response.Cookies.Add(cookiename);
                            HttpCookie cookieid = new HttpCookie("UserId", user.USER_ID.ToString());
                            Response.Cookies.Add(cookieid);
                            HttpCookie cookierole = new HttpCookie("Role", user.USER_ROLE);
                            Response.Cookies.Add(cookierole);

                            Response.Cookies.Add(cookieu);
                            Response.Cookies.Add(cookiep);

                        }
                        else
                        {
                            HttpCookie cookiename = new HttpCookie("Name", user.USER_FIRSTNAME+" "+user.USER_LASTNAME);
                            Response.Cookies.Add(cookiename);
                            HttpCookie cookieid = new HttpCookie("UserId", user.USER_ID.ToString());
                            Response.Cookies.Add(cookieid);
                            HttpCookie cookierole = new HttpCookie("Role", user.USER_ROLE);
                            Response.Cookies.Add(cookierole);


                        }
                        FormsAuthentication.SetAuthCookie(model.USER_NAME, true);
                        TempData["id"] = user.USER_ID;
                        TempData["username"] = user.USER_NAME;

                        

                        return RedirectToAction("Index", "Home");

                    }

                    else
                    {
                       
                        ModelState.AddModelError("", "Kullanıcı adı veya şifre hatalı!");
                    }
                }
                catch (Exception)
                {
                   
                }

               


            }
            return View(model);
        }
        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

    }
}