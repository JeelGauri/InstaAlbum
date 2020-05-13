using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InstaAlbum.Models
{
    public class ManageSessionAndCookie
    {
        private int CustomerID;
        private string CustomerName;
        private string CustomerEmail;

        public int m_CustomerID
        {
            get
            {
                return CustomerID;
            }
            set
            {
                this.CustomerID = value;
            }
        }


        public string m_CustomerName
        {
            get
            {
                return CustomerName;
            }
            set
            {
                this.CustomerName = value;
            }
        }

        public string m_CustomerEmail
        {
            get
            {
                return CustomerEmail;
            }
            set
            {
                this.CustomerEmail = value;
            }
        }

        ManageSessionAndCookie()
        {
        }
        public void IsUserSessionAlive(int customerId,string customerName,string customerEmail)
        {
            m_CustomerID = customerId;
            m_CustomerName = customerName;
            m_CustomerEmail = customerEmail;
        }
        public void UserStoreSessionCookie(int customerId, string customerName, string customerEmail)
        {
            m_CustomerID = customerId;
            m_CustomerName = customerName;
            m_CustomerEmail = customerEmail;
        }
    }
}