using CompanyApi.Data;
using CompanyApi.Models.DTOs.DepartmentDTOs;
using CompanyApi.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;

namespace CompanyApi.Services.Departments
{
	public class DepartmentService : IDepartmentService
	{
		private readonly ApplicationDbContext _context;

		public DepartmentService(ApplicationDbContext context)
		{
			_context = context;
		}
		public async Task<Department?> CreateAsync(CreateDepartmentDto departmentDto)
		{
			if (departmentDto == null)
				return null;

			var existingDepartment = await _context.Departments
					 .FirstOrDefaultAsync(d => d.Name == departmentDto.Name);

			if (existingDepartment != null)
				return null;

			Department dep = new()
			{
				Name = departmentDto.Name,
				Description = departmentDto.Description,
			};

			_context.Departments.Add(dep);

			if (await _context.SaveChangesAsync() <= 0)
				return null;

			return dep;
		}

		public async Task<bool> DeleteAsync(int id)
		{
			var dep = await _context.Departments.FindAsync(id);

			if (dep is null)
				return false;

			_context.Departments.Remove(dep);

			if (await _context.SaveChangesAsync() > 0)
				return true;

			return false;
		}

		public async Task<IEnumerable<Department>> GetAllAsync()
		{
			var Deparments = await _context.Departments.ToListAsync();

			if (Deparments is null)
				return null;

			return Deparments;
		}

		public async Task<Department?> GetByIdAsync(int id)
		{
			var dep = await _context.Departments.FindAsync(id);

			if (dep is null)
				return null;

			return dep;
		}

		public async Task<Department?> UpdateAsync(int id, UpdateDepartmentDto departmentDto)
		{
			var dep = await _context.Departments.FindAsync(id);

			if (dep is null)
				return null;

			if (await _context.Departments.AnyAsync(d => d.Name == departmentDto.Name && d.Id != id))
				return null;

			dep.Name = departmentDto.Name;
			dep.Description = departmentDto.Description;

			if (await _context.SaveChangesAsync() > 0)
				return dep; 

			return null;
		}
	}
}
