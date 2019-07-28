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
        //public Employee Get(int Id)
        public HttpResponseMessage Get(int Id)
        {
            //            return (_db.Employees.Where(s => s.ID == Id).FirstOrDefault());
            var ret = _db.Employees.Where(s => s.ID == Id).FirstOrDefault();
            if (ret != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, ret); // return 200
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Emp Not found for ID: " + Id.ToString()); // return 404  
            }
        }

        //public void Post([FromBody] Employee employee) NOTE, when return type is VOID, return default 204 - no content 
        public HttpResponseMessage Post([FromBody] Employee employee)
        {
            // For POST success (created a new item),  we should return 201 and along with the URI of the newly created Item!
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
                var err =   Request.CreateErrorResponse(HttpStatusCode.BadRequest, e); // Return error with Info
                return err;
            }
        }
    }
}
