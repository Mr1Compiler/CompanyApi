using CompanyApi.Models.DTOs.EmployeeDtos;
using CompanyApi.Models.DTOs.EmployeeDTOs;
using CompanyApi.Models.Entities;

namespace CompanyApi.Services.Employees
{
	public interface IEmployeeService
	{
		Task<Employee?> RegisterAsync(CreateEmployeeDto employeeDto);
		Task<Employee?> SignInAsync(SignInDto signInDto);
		Task<Employee?> UpdateAsync(int id, UpdateEmployeeDto employeeDto);
		Task<bool> DeleteAsync(int id);
		Task<Employee?> GetByIdAsync(int id);
		Task<IEnumerable<Employee>> GetAllAsync(); 
	}
}
