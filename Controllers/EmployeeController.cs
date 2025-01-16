using CompanyApi.Models.DTOs.EmployeeDtos;
using CompanyApi.Models.DTOs.EmployeeDTOs;
using CompanyApi.Services.Employees;
using CompanyApi.Services.Token;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CompanyApi.Controllers
{
	[Authorize(Roles = "Employee,Admin,Manager")]
	[Route("api/[controller]")]
	[ApiController]
	public class EmployeeController : ControllerBase
	{
		private readonly IEmployeeService _employeeService;
		private readonly ITokenService _tokenService;
		public EmployeeController(IEmployeeService employeeService, ITokenService tokenService)
		{
			_employeeService = employeeService;
			_tokenService = tokenService;
		}

		[AllowAnonymous]
		[HttpPost("Register")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult> Register(CreateEmployeeDto employeeDto)
		{
			var result = await _employeeService.RegisterAsync(employeeDto);

			if (result == null)
				return BadRequest("Registration failed or username already exists.");

			return Ok(new { result.AccessToken	, result.RefreshToken}); 
		}


		[AllowAnonymous]
		[HttpPost("Login")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult> Login([FromBody] SignInDto employeeDto)
		{
			// Authenticate the user and get employee with tokens
			var employee = await _employeeService.SignInAsync(employeeDto);

			if (employee == null)
				return BadRequest("Invalid username or password!");

			return Ok(new { employee.AccessToken, employee.RefreshToken });
		}


		[Authorize(Roles = "Admin")]
		[HttpPut("Update")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult> Update(UpdateEmployeeDto employee)
		{
			var result = await _employeeService.UpdateAsync(employee);

			if (result is null)
				return BadRequest("Employee not found!");


			return Ok("Updated Successfully");
		}

		[Authorize(Roles ="Manger")]
		[HttpDelete("Delete")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult> Delete(int id)
		{
			var result = await _employeeService.DeleteAsync(id);

			if (!result)
				return BadRequest($"Employee with id {id} cannot be found");

			return Ok("Deleted Successfully");
		}


		[Authorize(Roles = "Admin")]
		[HttpGet("Get")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult> Get(int id)
		{
			var result = await _employeeService.GetByIdAsync(id);

			if (result is null)
				return BadRequest($"Employee with id {id} cannot be found");

			return Ok(result);
		}


		[Authorize(Roles = "Manager")]
		[HttpGet("GetAll")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult> GetAll()
		{
			var result = await _employeeService.GetAllAsync();

			if (result is null)
				return BadRequest("Something wrong happend!");

			return Ok(result);
		}


		[HttpPost("refresh-token")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		public async Task<IActionResult> RefreshToken([FromBody] string refreshToken)
		{
			try
			{
				(string newAccessToken, string newRefreshToken) = await _employeeService.RefreshTokenAsync(refreshToken);
				return Ok(new { AccessToken = newAccessToken, RefreshToken = newRefreshToken });
			}
			catch (UnauthorizedAccessException)
			{
				return Unauthorized("Invalid or expired refresh token");
			}
		}

	}
}
