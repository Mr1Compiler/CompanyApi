using CompanyApi.Models.Entities;

namespace CompanyApi.Models.DTOs.EmployeeDTOs
{
	public class EmployeeWithTokensDto
	{
		public Employee Employee { get; set; }
		public string AccessToken { get; set; }
		public string RefreshToken { get; set; }
	}
}
