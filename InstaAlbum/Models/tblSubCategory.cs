//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace InstaAlbum.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class tblSubCategory
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblSubCategory()
        {
            this.tblGalleries = new HashSet<tblGallery>();
        }
    
        public int SubCategoryID { get; set; }
        public int ParentCatgoryID { get; set; }
        public string SubCategoryName { get; set; }
        public string SubCategoryCoverPhoto { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblGallery> tblGalleries { get; set; }
        public virtual tblParentCategory tblParentCategory { get; set; }
    }
}
