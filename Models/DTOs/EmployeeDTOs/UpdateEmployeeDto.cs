namespace CompanyApi.Models.DTOs.EmployeeDtos
{
	public class UpdateEmployeeDto
	{
		public string? Username { get; set; }
		public string? Password { get; set; }
		public string? Name { get; set; }
		public string? Role { get; set; }
		public DateTime? HireDate { get; set; }
		public int? DepartmentId { get; set; }
	}
}
