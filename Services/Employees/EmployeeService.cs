using CompanyApi.Models.DTOs.EmployeeDtos;
using CompanyApi.Models.DTOs.EmployeeDTOs;
using CompanyApi.Models.Entities;

namespace CompanyApi.Services.Employees
{
	public class EmployeeService : IEmployeeService
	{
		public Task<bool> DeleteAsync(int id)
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<Employee>> GetAllAsync()
		{
			throw new NotImplementedException();
		}

		public Task<Employee?> GetByIdAsync(int id)
		{
			throw new NotImplementedException();
		}

		public Task<Employee?> RegisterAsync(CreateEmployeeDto employee)
		{
			throw new NotImplementedException();
		}

		public Task<Employee?> SignInAsync(SignInDto employee)
		{
			throw new NotImplementedException();
		}

		public Task<Employee?> UpdateAsync(int id, UpdateEmployeeDto employeeDto)
		{
			throw new NotImplementedException();
		}
	}
}
