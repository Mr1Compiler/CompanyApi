using CompanyApi.Models.DTOs.EmployeeDtos;
using CompanyApi.Models.DTOs.EmployeeDTOs;
using CompanyApi.Models.Entities;
using CompanyApi.Services.Employees;
using CompanyApi.Services.Token;
using Microsoft.AspNetCore.Mvc;

namespace CompanyApi.Controllers
{
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

		[HttpPost("Register")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult> Register(CreateEmployeeDto employeeDto)
		{
			var result = await _employeeService.RegisterAsync(employeeDto);

			if (result == null)
				return BadRequest("Registration failed or username already exists.");

			return Ok("Employee Created Successfully"); // Returns employee and tokens
		}


		[HttpPost("Login")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult> Login(SignInDto employeeDto)
		{
			// Authenticate the user and get employee with tokens
			var employee = await _employeeService.SignInAsync(employeeDto);

			if (employee == null)
				return BadRequest("Invalid username or password!");

			return Ok("Login successful");
		}


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

		[HttpGet("GetAll")]
		public async Task<ActionResult> GetAll()
		{
			var result = await _employeeService.GetAllAsync();

			if (result is null)
				return BadRequest("Something wrong happend!");

			return Ok(result);
		}

	}
}
