using CompanyApi.Data;
using CompanyApi.Models.DTOs.EmployeeDtos;
using CompanyApi.Models.DTOs.EmployeeDTOs;
using CompanyApi.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

namespace CompanyApi.Services.Employees
{
	public class EmployeeService : IEmployeeService
	{
		private readonly ApplicationDbContext _context;

		public EmployeeService(ApplicationDbContext context)
		{
			_context = context;
		}
		public async Task<string> DeleteAsync(int id)
		{
			var emp = await _context.Employees.FindAsync(id);

			if (emp is null)
				return "Employee not found";

			try
			{
				_context.Employees.Remove(emp);
				await _context.SaveChangesAsync();
				return "Deleted successfully";
			}
			catch (Exception ex)
			{
				return "An error occurred while trying to delete the employee. Please try again.";
			}
		}

		public async Task<IEnumerable<Employee>> GetAllAsync()
		{
			var employees = _context.Employees.ToListAsync();

			return await employees;
		}

		public async Task<Employee?> GetByIdAsync(int id)
		{
			var emp = await _context.Employees.FindAsync(id);

			if (emp is null)
				return null;

			return emp;
		}

		public async Task<Employee?> RegisterAsync(CreateEmployeeDto employee)
		{
			if (employee is null)
				return null;

			if (await _context.Employees.AnyAsync(e => e.Username == employee.Username))
				return null;

			Employee newEmployee = new Employee();

			var hashedPassword = new PasswordHasher<Employee>()
				.HashPassword(newEmployee, employee.Password);

			newEmployee.Name = employee.Name;
			newEmployee.Username = employee.Username;
			newEmployee.HashPassword = hashedPassword;
			newEmployee.Role = employee.Role;
			newEmployee.DepartmentId = employee.DepartmentId;
			newEmployee.HireDate = DateTime.UtcNow;

			await _context.Employees.AddAsync(newEmployee);

			if (await _context.SaveChangesAsync() == 0)
				return null;

			return newEmployee;
		}

		public async Task<Employee?> SignInAsync(SignInDto employee)
		{
			var emp = await _context.Employees.FirstOrDefaultAsync(e => e.Username == employee.Username);

			var passwordHasher = new PasswordHasher<Employee>();
			var verificationResult = passwordHasher.VerifyHashedPassword(emp, emp.HashPassword, employee.Password);

			if (verificationResult == PasswordVerificationResult.Failed)
				return null;

			return emp;
		}

		public async Task<Employee?> UpdateAsync(int id, UpdateEmployeeDto employeeDto)
		{
			var emp = await _context.Employees.FindAsync(id);

			if (emp is null)
				return null;

			if (await _context.Employees.AnyAsync(e => e.Username == emp.Username && e.Id != id) == null)
				return null;

			emp.Username = employeeDto.Username;
			emp.Role = employeeDto.Role;
			emp.HireDate = employeeDto.HireDate;
			emp.DepartmentId = employeeDto.DepartmentId;

			if (!string.IsNullOrEmpty(employeeDto.Password))
			{
				var passwordHasher = new PasswordHasher<Employee>();
				var hashedPassword = passwordHasher.HashPassword(emp, employeeDto.Password);
				emp.HashPassword = hashedPassword;  
			}

			if (await _context.SaveChangesAsync() == 0)
				return null; 

			return emp; 
		}
	}
}
