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

    public class WishlistItemJsonModel
    {
        public int ID { get; set; }
        public string Content { get; set; }
    }

    public class WishlistIDModel
    {
        public int ID { get; set; }
    }

    public class WishlistJsonModel
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public bool IsPublic { get; set; }
    }

    public class WishlistFullJsonModel
    {
        public WishlistJsonModel Info { get; set; }
        public WishlistItemJsonModel[] WishlistItems { get; set; }
    }

    public class UserJsonModel
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public WishlistFullJsonModel[] Wishlists { get; set; }
    }
}