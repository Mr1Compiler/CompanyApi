using CompanyApi.Data;
using CompanyApi.Models.DTOs.EmployeeDtos;
using CompanyApi.Models.DTOs.EmployeeDTOs;
using CompanyApi.Models.Entities;
using CompanyApi.Services.Token;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CompanyApi.Services.Employees
{
	public class EmployeeService : IEmployeeService
	{
		private readonly ApplicationDbContext _context;
		private readonly ITokenService _tokenService;

		public EmployeeService(ApplicationDbContext context, ITokenService tokenService)
		{
			_context = context;
			_tokenService = tokenService;
		}
		public async Task<bool> DeleteAsync(int id)
		{
			var emp = await _context.Employees.FindAsync(id);

			if (emp is null)
				return false;

			_context.Employees.Remove(emp);

			if (await _context.SaveChangesAsync() == 0)
				return false;

			return true;
		}

		public async Task<IEnumerable<EmployeeDto>> GetAllAsync()
		{
			var employees = await _context.Employees
				.Include(e => e.Department)
		   .Select(e => new EmployeeDto
		   {
			   Username = e.Username,
			   Name = e.Name,
			   Role = e.Role,
			   DepartmentName = e.Department != null ? e.Department.Name : null,
		   })
		   .ToListAsync();


			return employees;
		}

		public async Task<EmployeeDto?> GetByIdAsync(int id)
		{
			var emp = await _context.Employees.FindAsync(id);

			if (emp is null)
				return null;

			EmployeeDto employee = new()
			{
				Name = emp.Name,
				Role = emp.Role,
				Username = emp.Username
			};

			var depInfo = await _context.Departments.FindAsync(emp.DepartmentId);

			if (depInfo is not null)
				employee.DepartmentName = depInfo.Name;

			return employee;
		}

		public async Task<EmployeeWithTokensDto?> RegisterAsync(CreateEmployeeDto employeeDto)
		{
			if (employeeDto is null)
				return null;

			if (await _context.Employees.AnyAsync(e => e.Username == employeeDto.Username))
				return null;

			Employee newEmployee = new Employee
			{
				Name = employeeDto.Name,
				Username = employeeDto.Username,
				Role = employeeDto.Role,
				DepartmentId = employeeDto.DepartmentId,
				HireDate = DateTime.UtcNow
			};

			// Hash the password
			var hashedPassword = new PasswordHasher<Employee>().HashPassword(newEmployee, employeeDto.Password);
			newEmployee.HashPassword = hashedPassword;


			// Generate tokens
			var accessToken = await _tokenService.GenerateAccessToken(newEmployee);
			var refreshToken = await _tokenService.GenerateRefreshToken();

			// Assign tokens to the employee
			newEmployee.RefreshToken = refreshToken;
			newEmployee.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7); // Example expiry time


			await _context.SaveChangesAsync();

			// Add the employee to the database
			await _context.Employees.AddAsync(newEmployee);
			if (await _context.SaveChangesAsync() == 0)
				return null;

			return new EmployeeWithTokensDto
			{
				Username = newEmployee.Username,
				AccessToken = accessToken,
				RefreshToken = refreshToken
			};
		}


		public async Task<EmployeeWithTokensDto?> SignInAsync(SignInDto signInEmployee)
		{
			var  emp = await _context.Employees.FirstOrDefaultAsync(e => e.Username == signInEmployee.Username);

			if(emp == null) 
				return null;

			var passwordHasher = new PasswordHasher<Employee>();
			var verificationResult = passwordHasher.VerifyHashedPassword(emp, emp.HashPassword, signInEmployee.Password);

			if (verificationResult == PasswordVerificationResult.Failed)
				return null;

			// Generate tokens
			var accessToken = await _tokenService.GenerateAccessToken(emp);
			var refreshToken = await _tokenService.GenerateRefreshToken();

			
			// Assign tokens to the employee
			emp.RefreshToken = refreshToken;
			emp.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7); // Example expiry time
			
			// Save changes to the database
			_context.Employees.Update(emp);
			await _context.SaveChangesAsync();

			return new EmployeeWithTokensDto() 
			{ 
				Username = emp.Username,
				AccessToken = accessToken,
				RefreshToken = refreshToken
			};
		}

		public async Task<(string newAccessToken, string newRefreshToken)> RefreshTokenAsync(string refreshToken)
		{
			var employee = await _context.Employees.FirstOrDefaultAsync(e => e.RefreshToken == refreshToken);

			if (employee == null || employee.RefreshTokenExpiryTime <= DateTime.UtcNow)
				throw new UnauthorizedAccessException("Invalid or expired refresh token");

			var newAccessToken = await _tokenService.GenerateAccessToken(employee);
			var newRefreshToken = await _tokenService.GenerateRefreshToken();

			// Update the employee's refresh token and expiry time
			employee.RefreshToken = newRefreshToken;
			employee.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7); // Adjust as needed

			_context.Employees.Update(employee);
			await _context.SaveChangesAsync();

			return (newAccessToken, newRefreshToken);
		}

		public async Task<Employee?> UpdateAsync(UpdateEmployeeDto employeeDto)
		{
			var emp = await _context.Employees.FirstOrDefaultAsync(e => e.Username == employeeDto.Username);

			if (emp is null)
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

			// Generate tokens
			var accessToken = await _tokenService.GenerateAccessToken(emp);
			var refreshToken = await _tokenService.GenerateRefreshToken();

			// Assign tokens to the employee
			emp.RefreshToken = refreshToken;
			emp.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7); // Example expiry time


			if (await _context.SaveChangesAsync() == 0)
				return null;

			return emp;
		}
	}
}
