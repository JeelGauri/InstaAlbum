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
    
    public partial class tblCity
    {
        public int CityID { get; set; }
        public int StateID { get; set; }
        public string CityName { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
    
        public virtual tblState tblState { get; set; }
    }
}
