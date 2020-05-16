using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InstaAlbum.Models
{
    public class ManageSessionAndCookie
    {
        private static int CustomerID;
        private static string CustomerName;
        private static string CustomerPhNo;

        public int m_CustomerID
        {
            get
            {
                return CustomerID;
            }
            set
            {
                CustomerID = value;
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
                CustomerName = value;
            }
        }

        public string m_CustomerPhNo
        {
            get
            {
                return CustomerPhNo;
            }
            set
            {
                CustomerPhNo = value;
            }
        }

        public ManageSessionAndCookie()
        {
        }
        public static void IsUserSessionAlive(int customerId, string customerName, string customerPhNo)
        {
            ManageSessionAndCookie manageSessionAndCookie = new ManageSessionAndCookie();
            manageSessionAndCookie.m_CustomerID = customerId;
            manageSessionAndCookie.m_CustomerName = customerName;
            manageSessionAndCookie.m_CustomerPhNo = customerPhNo;
        }
        public static void UserStoreSessionCookie(int customerId, string customerName, string customerPhNo)
        {
            ManageSessionAndCookie manageSessionAndCookie = new ManageSessionAndCookie();
            manageSessionAndCookie.m_CustomerID = customerId;
            manageSessionAndCookie.m_CustomerName = customerName;
            manageSessionAndCookie.m_CustomerPhNo = customerPhNo;
        }
    }
}