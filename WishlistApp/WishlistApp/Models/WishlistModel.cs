using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WishlistApp.Models
{
    public class WishlistItemIDModel
    {
        public int ID { get; set; }
    }

    public class WishlistItemArgumentModel : WishlistItemIDModel
    {
        public string Content { get; set; }
    }

    public class WishlistItemJsonModel : WishlistItemArgumentModel
    {
    }

    public class WishlistIDModel
    {
        public int ID { get; set; }
    }

    public class WishlistArgumentModel : WishlistIDModel
    {
        public string Title { get; set; }
        public bool IsPublic { get; set; }
    }

    public class WishlistJsonModel : WishlistArgumentModel
    {
        public WishlistItemJsonModel[] WishlistItems { get; set; }
    }

    public class UserJsonModel
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public WishlistJsonModel[] Wishlists { get; set; }
    }
}