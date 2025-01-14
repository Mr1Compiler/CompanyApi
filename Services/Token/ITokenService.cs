using CompanyApi.Models.Entities;
using System.Security.Claims;

namespace CompanyApi.Services.Token
{
	public interface ITokenService
	{
		Task<string> GenerateAccessToken(Employee employee);
		Task<string> GenerateRefreshToken();
	}
}
