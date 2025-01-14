namespace CompanyApi.Models.DTOs.DepartmentDTOs
{
	public class CreateDepartmentDto
	{
		public required string Name { get; set; } 
		public string? Description { get; set; } 
	}
}
