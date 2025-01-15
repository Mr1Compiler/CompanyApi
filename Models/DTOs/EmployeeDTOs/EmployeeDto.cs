using CompanyApi.Models.Entities;

namespace CompanyApi.Models.DTOs.EmployeeDtos
{
	public class EmployeeDto
	{
		public string Username { set; get; }
		public string? Name { set; get; }
		public string? Role { set; get; }
		public string? DepartmentName { set; get; }
	}
}
