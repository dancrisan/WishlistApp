//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WishlistApp.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Wishlist
    {
        public Wishlist()
        {
            this.WishlistItems = new HashSet<WishlistItem>();
        }
    
        public int WishlistId { get; set; }
        public string WishlistTitle { get; set; }
        public int UserId { get; set; }
        public bool IsPublic { get; set; }
    
        public virtual UserProfile UserProfile { get; set; }
        public virtual ICollection<WishlistItem> WishlistItems { get; set; }
    }
}
