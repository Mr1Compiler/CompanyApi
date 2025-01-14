using CompanyApi.Models.Entities;

namespace CompanyApi.Models.DTOs.EmployeeDtos
{
	public class CreateEmployeeDto
	{
		public required string Username { get; set; }
		public required string Password { get; set; }
		public string Role { get; set; } = "Employee";
		public DateTime? HireDate { get; set; } = DateTime.UtcNow;
		public int DepartmentId { get; set; }
	}
}
