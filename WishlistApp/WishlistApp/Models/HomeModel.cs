using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WishlistApp.Models
{
    public class UserInfoArgumentModel
    {
        public int ID { get; set; }
    }

    public class UserInfoJsonModel : UserInfoArgumentModel
    {
        public string UserName { get; set; }
    }

    public class UserInfoCollectionJsonModel
    {
        public UserInfoJsonModel[] Users { get; set; }
    }
}