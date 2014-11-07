using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WishlistApp.Models;

namespace WishlistApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }

        public ActionResult GetUserInfos()
        {
            using (var db = new WishlistContext())
            {
                var userInfosJsonModel = new UserInfoCollectionJsonModel
                {
                    Users =
                        db.UserProfiles
                        .AsEnumerable()
                        .Select(u => new UserInfoJsonModel
                        {
                            ID = u.UserId,
                            UserName = u.UserName
                        })
                        .ToArray()
                };

                return Json(userInfosJsonModel);
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
