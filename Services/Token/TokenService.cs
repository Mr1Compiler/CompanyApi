using CompanyApi.Models.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace CompanyApi.Services.Token
{
	public class TokenService : ITokenService
	{
		private readonly IConfiguration _configuration;

		public TokenService(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public async Task<string> GenerateAccessToken(Employee employee)
		{
			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

			var claims = new[]
			{
				new Claim(ClaimTypes.Name, employee.Name),
				new Claim(ClaimTypes.NameIdentifier, employee.Username),
				new Claim(ClaimTypes.Role, employee.Role),
			};

			var token = new JwtSecurityToken(
				issuer: _configuration["Jwt:Issuer"],
				audience: _configuration["Jwt:Audience"],
				claims: claims,
				expires: DateTime.UtcNow.AddMinutes(double.Parse(_configuration["Jwt:AccessTokenExpiration"])),
				signingCredentials: creds
				);

			return await Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
		}

		public async Task<string> GenerateRefreshToken()
		{
			var randomBytes = new byte[32];

			using (var rng = RandomNumberGenerator.Create())
			{
				rng.GetBytes(randomBytes);
			}

			return await Task.FromResult(Convert.ToBase64String(randomBytes));
		}
	}
}
