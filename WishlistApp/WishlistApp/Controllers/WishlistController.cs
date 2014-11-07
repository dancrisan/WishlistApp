using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;
using WishlistApp.Filters;
using WishlistApp.Models;

namespace WishlistApp.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    public class WishlistController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        //[AllowAnonymous]
        public ActionResult ViewUser(UserInfoArgumentModel ureq)
        {
            using (var db = new WishlistContext())
            {
                var userProfile =
                    db.UserProfiles
                    .FirstOrDefault(u => u.UserId == ureq.ID);

                var userName =
                    userProfile == null ? null : userProfile.UserName;

                var jsonModel = new UserJsonModel
                {
                    UserID = ureq.ID,
                    UserName = userName,
                    Wishlists =
                        db.Wishlists
                        .Where(wl => wl.UserId == ureq.ID && wl.IsPublic)
                        .AsEnumerable()
                        .Select(wl => new WishlistJsonModel
                        {
                            ID = wl.WishlistId,
                            Title = wl.WishlistTitle,
                            IsPublic = wl.IsPublic,
                            WishlistItems =
                                wl.WishlistItems
                                .AsEnumerable()
                                .Select(wli => new WishlistItemJsonModel
                                {
                                    ID = wli.WishlistItemId,
                                    Content = wli.WishlistItemContent
                                })
                                .ToArray()
                        })
                        .ToArray()
                };

                return View(jsonModel);
            }
        }

        public ActionResult GetWishlists()
        {
            var userid = WebSecurity.CurrentUserId;

            using (var db = new WishlistContext())
            {
                var jsonModel = new UserJsonModel
                {
                    UserID = userid,
                    UserName = WebSecurity.CurrentUserName,
                    Wishlists =
                        db.Wishlists
                        .Where(wl => wl.UserId == userid)
                        .AsEnumerable()
                        .Select(wl => new WishlistJsonModel
                        {
                            ID = wl.WishlistId,
                            Title = wl.WishlistTitle,
                            IsPublic = wl.IsPublic,
                            WishlistItems =
                                wl.WishlistItems
                                .AsEnumerable()
                                .Select(wli => new WishlistItemJsonModel
                                {
                                    ID = wli.WishlistItemId,
                                    Content = wli.WishlistItemContent
                                })
                                .ToArray()
                        })
                        .ToArray()
                };

                return Json(jsonModel);
            }
        }

        public ActionResult NewWishlist()
        {
            using (var db = new WishlistContext())
            {
                var wl = new Models.Wishlist
                {
                    UserId = WebSecurity.CurrentUserId,
                    WishlistTitle = "New Wishlist"
                };
                db.Wishlists.Add(wl);
                db.SaveChanges();

                return Json(new WishlistJsonModel
                {
                    ID = wl.WishlistId,
                    Title = wl.WishlistTitle,
                    IsPublic = wl.IsPublic,
                    WishlistItems = new WishlistItemJsonModel[] { }
                });
            }
        }

        public ActionResult ChangeWishlist(WishlistArgumentModel model)
        {
            using (var db = new WishlistContext())
            {
                var wl =
                    db.Wishlists
                    .FirstOrDefault(wl2 => wl2.WishlistId == model.ID);

                if (wl == null)
                {
                    return Json(new
                    {
                        Success = false,
                        Exception = "Invalid wishlist id."
                    });
                }
                else if (wl.UserId != WebSecurity.CurrentUserId)
                {
                    return Json(new
                    {
                        Success = false,
                        Exception = "Change requested by an unauthorized user."
                    });
                }
                else
                {
                    wl.WishlistTitle = model.Title;
                    wl.IsPublic = model.IsPublic;
                    db.SaveChanges();
                    return Json(new
                    {
                        Success = true
                    });
                }
            }
        }

        public ActionResult RemoveWishlist(WishlistIDModel wlreq)
        {
            using (var db = new WishlistContext())
            {
                var wl =
                    db.Wishlists
                    .FirstOrDefault(wl2 => wl2.WishlistId == wlreq.ID);

                if (wl == null)
                {
                    return Json(new
                    {
                        Success = false,
                        Exception = "Invalid wishlist id."
                    });
                }
                else if (wl.UserId != WebSecurity.CurrentUserId)
                {
                    return Json(new
                    {
                        Success = false,
                        Exception = "Removal requested by an unauthorized user."
                    });
                }
                else
                {
                    foreach (var wli in wl.WishlistItems.ToList())
                        db.WishlistItems.Remove(wli);
                    db.Wishlists.Remove(wl);
                    db.SaveChanges();

                    return Json(new
                    {
                        Success = true
                    });
                }
            }
        }

        public ActionResult NewWishlistItem(WishlistIDModel wlreq)
        {
            using (var db = new WishlistContext())
            {
                var wl =
                    db.Wishlists
                    .FirstOrDefault(wl2 => wl2.WishlistId == wlreq.ID);

                if (wl == null)
                {
                    return Json(new
                    {
                        Success = false,
                        Exception = "Invalid wishlist id."
                    });
                }
                else if (wl.UserId != WebSecurity.CurrentUserId)
                {
                    return Json(new
                    {
                        Success = false,
                        Exception = "Add requested by an unauthorized user."
                    });
                }
                else
                {
                    var wli = new Models.WishlistItem
                    {
                        WishlistId = wl.WishlistId,
                        WishlistItemContent = "New Wishlist Item"
                    };

                    db.WishlistItems.Add(wli);
                    db.SaveChanges();

                    return Json(new
                    {
                        Success = true,
                        Result = new WishlistItemJsonModel
                        {
                            ID = wli.WishlistItemId,
                            Content = wli.WishlistItemContent
                        }
                    });
                }
            }
        }

        public ActionResult ChangeWishlistItem(WishlistItemArgumentModel wlireq)
        {
            using (var db = new WishlistContext())
            {
                var wli =
                    db.WishlistItems
                    .FirstOrDefault(wli2 => wli2.WishlistItemId == wlireq.ID);

                if (wli == null)
                {
                    return Json(new
                    {
                        Success = false,
                        Exception = "Invalid wishlist item id."
                    });
                }
                else if (wli.Wishlist.UserId != WebSecurity.CurrentUserId)
                {
                    return Json(new
                    {
                        Success = false,
                        Exception = "Change requested by an unauthorized user."
                    });
                }
                else
                {
                    wli.WishlistItemContent = wlireq.Content;
                    db.SaveChanges();

                    return Json(new
                    {
                        Success = true
                    });
                }
            }
        }

        public ActionResult RemoveWishlistItem(WishlistItemIDModel wlireq)
        {
            using (var db = new WishlistContext())
            {
                var wli =
                    db.WishlistItems
                    .FirstOrDefault(wli2 => wli2.WishlistItemId == wlireq.ID);

                if (wli == null)
                {
                    return Json(new
                    {
                        Success = false,
                        Exception = "Invalid wishlist item id."
                    });
                }
                else if (wli.Wishlist.UserId != WebSecurity.CurrentUserId)
                {
                    return Json(new
                    {
                        Success = false,
                        Exception = "Removal requested by an unauthorized user."
                    });
                }
                else
                {
                    db.WishlistItems.Remove(wli);
                    db.SaveChanges();

                    return Json(new
                    {
                        Success = true
                    });
                }
            }
        }
    }
}
