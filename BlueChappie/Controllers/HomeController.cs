using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static BlueChappie.Models.BlueChappieModels;

namespace BlueChappie.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

           // HttpContext context = HttpContext.Session();
           // if (Request.QueryString["location"] != null)
           // {
           //     //this.ctrlImages1.Location = Request.QueryString["location"];
           //     context.Session["location"] = Request.QueryString["location"];
           // }
           // if (Request.QueryString["user"] != null)
           // {
           //     context.Session["user"] = Request.QueryString["user"];
           //   //  Response.Redirect("Default.aspx");
           // }
           //// if (context.Session["location"] != null) { this.ctrlImages1.Location = context.Session["location"].ToString(); }
           // if (context.Session["user"] != null) { setuser(context.Session["user"].ToString()); }

            return View();
        }
        protected void setuser(string userId)
        {
            //clsMainProgram cls = new clsMainProgram();
            //user usr = cls.getUserDetails(userId);
            //this.hvUserEmailAddress.Value = usr.emailaddress;
            //this.hvUserId.Value = usr.userId;
            ////this.lblUserEmail.Text = usr.emailaddress;
            //this.lblUserClick.Visible = false;
            //this.lblUserEmail.Visible = true;
            //this.ctlLocations1.UserId = usr.userId;
            //this.ctlUserLocations1.UserId = usr.userId;

        }

    }
}
