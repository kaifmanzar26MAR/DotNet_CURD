using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using test.Data;
using test.Models;
using test.Models.Entities;

namespace test.Controllers
{
    //*the route will be localhost:port/api/Employees
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;

        public EmployeesController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }


        //*getting the all the employees 
        [HttpGet] //*get request
        public IActionResult GetAllEmployees()
        {
            var allEmployees = dbContext.Employees.ToList();

            return Ok(allEmployees);
        }

        [HttpGet]
        [Route("{id:guid}")] //*specify the id in the route
        public IActionResult GetEmployeeById(Guid id) //*the route and the parameter name should be same
        {
            var employee = dbContext.Employees.Find(id); //*return a null value if not found

            if(employee is null)
            {
                return NotFound(); //*gives a 404 response
            }

            return Ok(employee);
        }

        //*adding a new employee
        [HttpPost]
        public IActionResult AddEmployee(AddEmployeeDTO addEmployeeDTO)
        {
            //*DTO are the thigs from that we are getting the data like body-parser
            
            //*here we are creating the new employee instance
            var employeeEntity = new Employee()
            {
                Name = addEmployeeDTO.Name,
                Email = addEmployeeDTO.Email,
                Phone = addEmployeeDTO.Phone,
                Salary = addEmployeeDTO.Salary,
            };
            
            //*adding the employee in the virtual table like the transection
            dbContext.Employees.Add(employeeEntity);

            //*save the modifications that we have done to reflect in the original table
            dbContext.SaveChanges();


            //*sending the ok response i.e response with status 200
            return Ok(employeeEntity);
        }


        //*Updating the employee table
        [HttpPut]
        [Route("{id:guid}")]
        public IActionResult UpdateEmployee(Guid id, UpdateEmployeeDTO updateData)
        {
            var employee = dbContext.Employees.Find(id);
            if(employee is null)
            {
                return NotFound();
            }

            employee.Name = updateData.Name;
            employee.Email = updateData.Email;
            employee.Phone = updateData.Phone;
            employee.Salary = updateData.Salary;

            dbContext.SaveChanges();

            return Ok(employee);
        }

        //*deleting the employee
        [HttpDelete]
        [Route("{id:guid}")]
        public IActionResult DeleteEmployee(Guid id)
        {
            var employee = dbContext.Employees.Find(id);
            if (employee is null)
            {
                return NotFound();
            }

            dbContext.Employees.Remove(employee);
            dbContext.SaveChanges();

            return Ok(new { message = $"Employee Deleted Successfully with id {id}" });
            //return Ok();

        }
    }
}
