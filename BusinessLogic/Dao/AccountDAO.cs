using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using static DataAccess.Models.AccountMember;

namespace BusinessLogic.Dao
{
    public class AccountDAO
    {

        //-------------------------------------
        //Using Singleton Pattern
        private static AccountDAO instance = null;
        private static readonly object instanceLock = new object();
        private AccountDAO() { }
        public static AccountDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new AccountDAO();
                    }
                    return instance;
                }
            }
        }

        //-------------------------------------
        public Boolean Login(string email, string password)
        {
            Boolean result = false;
            AccountMember acc = null;
            try
            {
                var flightManagement = new FlightManagementDBContext();
                acc = flightManagement.AccountMembers.SingleOrDefault(a => a.Email.Equals(email) && a.Password.Equals(password));
                if (acc != null)
                {
                    SessionDataAcc.accs.Add(acc);
                    result = true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return result;
        }

        //-0-----
        public string GetUserRole(AccountMember? account)
        {
            string userRole = string.Empty;
            try
            {
                userRole = account.Role;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while getting user role: " + ex.Message);
            }
            return userRole;
        }
        //111
        public AccountMember GetAccountByEmail(string email)
        {
            AccountMember account = null;
            try
            {
                var flightManagement = new FlightManagementDBContext();
                account = flightManagement.AccountMembers.FirstOrDefault(a => a.Email.Equals(email));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return account;
        }
    }
}
