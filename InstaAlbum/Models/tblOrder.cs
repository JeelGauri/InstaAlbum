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
    
    public partial class tblOrder
    {
        public int OrderID { get; set; }
        public int PackageID { get; set; }
        public int BranchID { get; set; }
        public int CustomerID { get; set; }
        public decimal TotalAmount { get; set; }
        public string PaymentMode { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
    
        public virtual tblBranch tblBranch { get; set; }
        public virtual tblCustomer tblCustomer { get; set; }
        public virtual tblPackage tblPackage { get; set; }
    }
}
