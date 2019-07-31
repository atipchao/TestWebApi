using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;

using EmpDB;

namespace TestWebApi.Controllers
{
    // use api
    public class EmployeeController : ApiController
    {
        
        EmployeeDBEntities _db = new EmployeeDBEntities();

        [HttpGet]
        //public IEnumerable<Employee> GetSomething()
        //public IEnumerable<Employee> LoadAllEmp()  // Error: "message": "The requested resource does not support http method 'GET'."
        [BasicAuthentication]
        public HttpResponseMessage Get(string gender = "All") 
        {

            string username = Thread.CurrentPrincipal.Identity.Name;

            //return (_db.Employees.ToList());
            //switch (gender.ToLower())
            switch (username.ToLower())
            {
                case "all":
                    return Request.CreateResponse(HttpStatusCode.OK, _db.Employees.ToList());
                case "male":
                    return Request.CreateResponse(HttpStatusCode.OK, _db.Employees.Where(e => e.Gender.ToLower() == "male").ToList());
                case "female":
                    return Request.CreateResponse(HttpStatusCode.OK, _db.Employees.Where(e => e.Gender.ToLower() == "female").ToList());
                default:
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Value for Gender Not found : " + gender.ToLower());

            }
            
        }
        [HttpGet]
        //public Employee Get(int Id)
        //public HttpResponseMessage Get(int Id)
        public HttpResponseMessage LoadEmpById(int Id)
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

        //public void Delete(int id)// this one return 204 No-Content, properly we should return 200 Okay
        // When Item to be deleted is not found, we should return 404 Not found
        public HttpResponseMessage Delete(int id)
        {
            
            try
            {
                var targetRec = _db.Employees.FirstOrDefault(e => e.ID == id);
                if (targetRec == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Not found Emp with ID: " + id);
                }
                _db.Employees.Remove(targetRec);
                _db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch(Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }


        //public void Put(int id, [FromBody] Employee employee)
        //By default for simple parm, WEBAPI try to get the Parameter data from the URI
        //For complex parms, we pass it via [FormBody] 
        //public HttpResponseMessage Put(int id, [FromUri] Employee employee)  ==> you can also force it over the URI
        public HttpResponseMessage Put(int id, [FromBody] Employee employee)
        {
            try
            {
                // First locate the Target Emp
                var targetEmp = _db.Employees.FirstOrDefault(e => e.ID == id);
                if(targetEmp != null)
                {
                    targetEmp.FirstName = employee.FirstName;
                    targetEmp.LastName = employee.LastName;
                    targetEmp.Gender = employee.Gender;
                    targetEmp.Salary = employee.Salary;
                    _db.SaveChanges();
                    var msg = Request.CreateResponse(HttpStatusCode.Created, employee);
                    // Also return Header Location
                    msg.Headers.Location = new Uri(Request.RequestUri + employee.ID.ToString());
                    return msg;

                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Not found Emp with ID: " + id);
                }
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
            
        }
    }
}
