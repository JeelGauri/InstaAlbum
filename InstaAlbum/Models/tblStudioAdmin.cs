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
    
    public partial class tblStudioAdmin
    {
        public int StudioID { get; set; }
        public string StudioName { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNo { get; set; }
        public string StudioLogo { get; set; }
        public string About { get; set; }
        public string OpeningHours { get; set; }
        public string ClosingHours { get; set; }
        public string Map { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
    }
}
