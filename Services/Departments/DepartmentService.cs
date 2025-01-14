using CompanyApi.Models.DTOs.DepartmentDTOs;
using CompanyApi.Models.Entities;

namespace CompanyApi.Services.Departments
{
	public class DepartmentService : IDepartmentService
	{
		public Task<Department?> CreateAsync(CreateDepartmentDto departmentDto)
		{
			throw new NotImplementedException();
		}

		public Task<bool> DeleteAsync(int id)
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<Department>> GetAllAsync()
		{
			throw new NotImplementedException();
		}

		public Task<Department?> GetByIdAsync(int id)
		{
			throw new NotImplementedException();
		}

		public Task<Department?> UpdateAsync(int id, UpdateDepartmentDto departmentDto)
		{
			throw new NotImplementedException();
		}
	}
}
