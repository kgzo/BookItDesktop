//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BookItDesktop
{
    using System;
    using System.Collections.Generic;
    
    public partial class BookmarkTags
    {
        public int Id { get; set; }
        public Nullable<int> Bookmark_BookmarkID { get; set; }
        public Nullable<int> Tag_TagID { get; set; }
    
        public virtual Bookmarks Bookmarks { get; set; }
        public virtual Tags Tags { get; set; }
    }
}