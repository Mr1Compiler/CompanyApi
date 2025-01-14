using CompanyApi.Models.DTOs.DepartmentDTOs;
using CompanyApi.Models.Entities;

namespace CompanyApi.Services.Departments
{
	public interface IDepartmentService
	{
		Task<IEnumerable<Department>> GetAllAsync();
		Task<Department?> GetByIdAsync(int id);
		Task<Department?> CreateAsync(CreateDepartmentDto departmentDto);
		Task<Department?> UpdateAsync(int id, UpdateDepartmentDto departmentDto);
		Task<bool> DeleteAsync(int id); 
	}
}
