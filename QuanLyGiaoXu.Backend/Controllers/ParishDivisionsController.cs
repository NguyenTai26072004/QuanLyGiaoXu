using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuanLyGiaoXu.Backend.DTOs.ParishDivisionDtos;
using QuanLyGiaoXu.Backend.Enums;
using QuanLyGiaoXu.Backend.Services.ParishDivisions;
using System;
using System.Threading.Tasks;

namespace QuanLyGiaoXu.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ParishDivisionsController : ControllerBase
    {
        private readonly IParishDivisionService _parishDivisionService;

        public ParishDivisionsController(IParishDivisionService parishDivisionService)
        {
            _parishDivisionService = parishDivisionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetParishDivisions()
        {
            return Ok(await _parishDivisionService.GetParishDivisionsAsync());
        }

        // POST api/parishdivisions
        [HttpPost]
        [Authorize(Roles = nameof(Roles.Admin))]
        public async Task<IActionResult> CreateParishDivision(CreateUpdateParishDivisionDto dto)
        {
            return Ok(await _parishDivisionService.CreateParishDivisionAsync(dto));
        }

        // PUT api/parishdivisions/5
        [HttpPut("{id}")]
        [Authorize(Roles = nameof(Roles.Admin))]
        public async Task<IActionResult> UpdateParishDivision(int id, CreateUpdateParishDivisionDto dto)
        {
            var result = await _parishDivisionService.UpdateParishDivisionAsync(id, dto);
            return result ? NoContent() : NotFound();
        }

        // DELETE api/parishdivisions/5
        [HttpDelete("{id}")]
        [Authorize(Roles = nameof(Roles.Admin))]
        public async Task<IActionResult> DeleteParishDivision(int id)
        {
            try
            {
                var result = await _parishDivisionService.DeleteParishDivisionAsync(id);
                return result ? NoContent() : NotFound();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}