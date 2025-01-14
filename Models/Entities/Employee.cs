using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace CompanyApi.Models.Entities
{
	public class Employee
	{
		public int Id { get; set; }
		[Required]
		public required string Username { get; set; }
		public required string Password { get; set; }	
		public string? Name { get; set; }
		public string Role { get; set; } = "Employee";
		public DateTime? HireDate { get; set; }
		public int? DepartmentId { get; set; }
		public Department? Department { get; set; }
	}
}
