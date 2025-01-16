using CompanyApi.Models.DTOs.EmployeeDtos;
using CompanyApi.Models.DTOs.EmployeeDTOs;
using CompanyApi.Models.Entities;

namespace CompanyApi.Services.Employees
{
	public interface IEmployeeService
	{
		Task<EmployeeWithTokensDto?> RegisterAsync(CreateEmployeeDto employeeDto);
		Task<EmployeeWithTokensDto?> SignInAsync(SignInDto signInDto);
		Task<Employee?> UpdateAsync(UpdateEmployeeDto employeeDto);
		Task<bool> DeleteAsync(int id);
		Task<(string newAccessToken, string newRefreshToken)> RefreshTokenAsync(string refreshToken);
		Task<EmployeeDto?> GetByIdAsync(int id);
		Task<IEnumerable<EmployeeDto>> GetAllAsync(); 
	}
}
