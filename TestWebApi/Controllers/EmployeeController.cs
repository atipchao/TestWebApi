using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using EmpDB;

namespace TestWebApi.Controllers
{
    // use api
    public class EmployeeController : ApiController
    {
        EmployeeDBEntities _db = new EmployeeDBEntities();

        public IEnumerable<Employee> Get()
        {
            
                return (_db.Employees.ToList());
            
        }
        public Employee Get(int Id)
        {
            return (_db.Employees.Where(s => s.ID == Id).FirstOrDefault());
        }
    }
}
