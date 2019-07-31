using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EmpDB;

namespace TestWebApi
{
    public class EmpSecurity
    {
        
        public static bool Login(string username, string password)
        {
            EmployeeDBEntities _db = new EmployeeDBEntities();
            return _db.Users.Any(u => u.Username == username && u.Password == password);
        }
    }
}