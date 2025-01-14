﻿namespace CompanyApi.Models.Entities
{
	public class Department
	{
		public int Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public string? Description { get; set; }
		public ICollection<Employee>? Employees {  get; set; } 
	}
}
