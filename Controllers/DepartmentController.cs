using CompanyApi.Models.DTOs.DepartmentDTOs;
using CompanyApi.Services.Departments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using System.Runtime.InteropServices;
using System.Security;

namespace CompanyApi.Controllers
{
	
	[Authorize(Roles = "Admin,Manager")]
	[Route("api/[controller]")]
	[ApiController]
	public class DepartmentController : ControllerBase
	{
		private readonly IDepartmentService _departmentService;

		public DepartmentController(IDepartmentService departmentService)
		{
			_departmentService = departmentService;
		}

		[HttpGet("GetAll")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult> GetAll()
		{
			var result = await _departmentService.GetAllAsync();

			if (result is null)
				return NotFound();

			return Ok(result);
		}

		[HttpGet("Get")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult> Get(int id)
		{
			var result = await _departmentService.GetByIdAsync(id);

			if (result is null)
				return NotFound("Not Found");

			return Ok(result);
		}

		[HttpPost("Create")]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<ActionResult> Create(CreateDepartmentDto department)
		{
			var reslt = await _departmentService.CreateAsync(department);

			if (reslt is null)
				return BadRequest($"{department.Name} Department is already exists");

			return Ok("Created Successfully");
		}

		[HttpDelete("Delete")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult> Delete(int id)
		{
			var result = await _departmentService.DeleteAsync(id);

			if (!result)
				return NotFound($"Department with {id} is not found");

			return Ok("Deleted successfully");
		}


		[HttpPut("Update")]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult> Update(int id, UpdateDepartmentDto department)
		{
			var result = await _departmentService.UpdateAsync(id, department);
			
			
			if (result == false)
				return NotFound("Department  not exists");

			if (result is null)
				return BadRequest("Something Wrong");
			
			return Ok("Updated Successfully");
		}


	}
}
