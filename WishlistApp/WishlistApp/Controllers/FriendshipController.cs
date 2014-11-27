using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WishlistApp.Models;

namespace WishlistApp.Controllers
{
    public class FriendshipController : Controller
    {
        public ActionResult SearchUser(string userNameQuery)
        {
            using (var db = new WishlistContext())
            {
                var users =
                    db.UserProfiles
                    .Where(u => u.UserName.Contains(userNameQuery))
                    .AsEnumerable()
                    .Select(u => new UserInfoJsonModel
                    {
                        ID = u.UserId,
                        UserName = u.UserName,
                    });

                return Json(users);
            }
        }

        public ActionResult GetFriends(int userId)
        {
            using (var db = new WishlistContext())
            {
                var upperFriends =
                    from fs in db.Friendships
                    where fs.UserIdLower == userId
                    join u in db.UserProfiles on fs.UserIdUpper equals u.UserId
                    select new { u.UserId, u.UserName };

                var lowerFriends =
                    from fs in db.Friendships
                    where fs.UserIdUpper == userId
                    join u in db.UserProfiles on fs.UserIdLower equals u.UserId
                    select new { u.UserId, u.UserName };

                var friends =
                    upperFriends.Concat(lowerFriends)
                    .AsEnumerable()
                    .Select(u => new UserInfoJsonModel
                    {
                        ID = u.UserId,
                        UserName = u.UserName
                    });

                return Json(friends);
            }
        }
    }
}
