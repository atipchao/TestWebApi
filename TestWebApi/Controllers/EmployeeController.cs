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

        //public void Post([FromBody] Employee employee)
        public HttpResponseMessage Post([FromBody] Employee employee)
        {
            // For POST success, we should return 201 and along with the URI of the newly created Item!
            try
            {
                _db.Employees.Add(employee);
                _db.SaveChanges();

                var msg = Request.CreateResponse(HttpStatusCode.Created, employee);
                // Also return Header Location
                msg.Headers.Location = new Uri(Request.RequestUri + employee.ID.ToString());
                return msg;
            }
            catch(Exception e)
            {
                var err =   Request.CreateErrorResponse(HttpStatusCode.BadRequest, e);
                return err;
            }
        }
    }
}
