using CompanyApi.Models.DTOs.DepartmentDTOs;
using CompanyApi.Models.Entities;

namespace CompanyApi.Services.Departments
{
	public interface IDepartmentService
	{
		Task<IEnumerable<DepartmentDto>> GetAllAsync();
		Task<DepartmentDto?> GetByIdAsync(int id);
		Task<Department?> CreateAsync(CreateDepartmentDto departmentDto);
		Task<bool?> UpdateAsync(int id, UpdateDepartmentDto departmentDto);
		Task<bool> DeleteAsync(int id); 
	}
}
