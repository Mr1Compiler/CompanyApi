using CompanyApi.Data;
using CompanyApi.Models.DTOs.DepartmentDTOs;
using CompanyApi.Models.DTOs.EmployeeDtos;
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

		public async Task<IEnumerable<DepartmentDto>> GetAllAsync()
		{
			var departments = await _context.Departments
				.Include(d => d.Employees)
				.Select(d => new DepartmentDto
				{
					Name = d.Name,
					Description = d.Description,
					EmployeesNumber = d.Employees.Count(), 
				}).ToListAsync();

			return departments; 
		}

		public async Task<DepartmentDto?> GetByIdAsync(int id)
		{
			var dep = await _context.Departments
				.Include(d => d.Employees)	
				.FirstOrDefaultAsync(d => d.Id == id);

			if (dep is null)
				return null;

			DepartmentDto department = new()
			{
				Name = dep.Name,
				Description = dep.Description,
				EmployeesNumber = dep.Employees.Count(),
			};

			if (department is null)
				return null;

			return  department;
		}

		public async Task<bool?> UpdateAsync(int id, UpdateDepartmentDto departmentDto)
		{
			var dep = await _context.Departments.FindAsync(id);

			if (dep is null)
				return null;

			if (await _context.Departments.AnyAsync(d => d.Name == departmentDto.Name && d.Id != id))
				return false;

			dep.Name = departmentDto.Name;
			dep.Description = departmentDto.Description;

			if (await _context.SaveChangesAsync() == 0)
				return null;

			return true;
		}
	}
}
