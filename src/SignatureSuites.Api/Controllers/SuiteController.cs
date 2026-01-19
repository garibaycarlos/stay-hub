using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SignatureSuites.Api.Data;
using SignatureSuites.Api.Models;
using SignatureSuites.Api.Models.DTO;
using SignatureSuites.Api.Models.DTO.Suite;

namespace SignatureSuites.Api.Controllers;

[ApiController]
[Route("api/suites")]
[Authorize(Roles = "Admin,Customer")]
public class SuiteController(ApplicationDbContext db, IMapper mapper) : ControllerBase
{
    private readonly ApplicationDbContext _db = db;
    private readonly IMapper _mapper = mapper;

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<SuiteDTO>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<IEnumerable<SuiteDTO>>>> GetSuites()
    {
        var suites = await _db.Suite.ToListAsync();
        var dtoResponseSuite = _mapper.Map<IEnumerable<SuiteDTO>>(suites);

        var response = ApiResponse<IEnumerable<SuiteDTO>>.Ok(dtoResponseSuite, "Suites retrieved successfully");

        return Ok(response);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<SuiteDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<SuiteDTO>> GetSuiteById(int id)
    {
        if (id <= 0) return NotFound(ApiResponse<object>.NotFound($"Suite with Id {id} was not found"));

        try
        {
            var suite = await _db.Suite.FindAsync(id);

            if (suite is null) return NotFound(ApiResponse<object>.NotFound($"Suite with Id {id} was not found"));

            return Ok(ApiResponse<SuiteDTO>.Ok(_mapper.Map<SuiteDTO>(suite), "Records retrieved successfully"));
        }
        catch (Exception ex)
        {
            var errorResponse = ApiResponse<object>.Error(StatusCodes.Status500InternalServerError, $"An error occurred while retrieving suite with Id {id}", ex.Message);

            return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
        }
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ApiResponse<SuiteCreateDTO>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<SuiteCreateDTO>>> CreateSuite(SuiteCreateDTO suiteDTO)
    {
        if (suiteDTO is null) return BadRequest(ApiResponse<object>.BadRequest("Suite data is required"));

        try
        {
            var duplicateSuite = _db.Suite.FirstOrDefault(v => v.Name.ToLower() == suiteDTO.Name.ToLower());

            if (duplicateSuite is not null) return Conflict(ApiResponse<object>.Conflict($"A suite with the name '{suiteDTO.Name}' already exists"));

            var suite = _mapper.Map<Suite>(suiteDTO);

            suite.CreatedDate = DateTime.UtcNow;

            await _db.Suite.AddAsync(suite);
            await _db.SaveChangesAsync();

            var response = ApiResponse<SuiteDTO>.CreatedAt(_mapper.Map<SuiteDTO>(suite), "Suite created successfully");

            return CreatedAtAction(nameof(CreateSuite), new { id = suite.Id }, response);
        }
        catch (Exception ex)
        {
            var errorResponse = ApiResponse<object>.Error(StatusCodes.Status500InternalServerError, $"An error occurred while creating the suite", ex.Message);

            return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
        }
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ApiResponse<SuiteDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<SuiteDTO>>> UpdateSuite(int id, SuiteUpdateDTO suiteDTO)
    {
        if (suiteDTO is null) return BadRequest(ApiResponse<object>.BadRequest("Suite data is required"));
        if (id != suiteDTO.Id) return BadRequest(ApiResponse<object>.BadRequest("Suite Id in URL does not match Suite Id in request body"));

        try
        {
            var existingSuite = await _db.Suite.FindAsync(id);

            if (existingSuite is null) return NotFound(ApiResponse<object>.NotFound($"Suite with Id {id} was not found"));

            var duplicateSuite = _db.Suite.FirstOrDefault(v => v.Name.ToLower() == suiteDTO.Name.ToLower() && v.Id != id);

            if (duplicateSuite is not null) return Conflict(ApiResponse<SuiteCreateDTO>.Conflict($"A suite with the name '{suiteDTO.Name}' already exists"));

            _mapper.Map(suiteDTO, existingSuite);

            existingSuite.UpdatedDate = DateTime.UtcNow;

            await _db.SaveChangesAsync();

            var response = ApiResponse<SuiteDTO>.Ok(_mapper.Map<SuiteDTO>(suiteDTO), "Suite updated successfully");

            return Ok(suiteDTO);
        }
        catch (Exception ex)
        {
            var errorResponse = ApiResponse<object>.Error(StatusCodes.Status500InternalServerError, $"An error occurred while updating the suite", ex.Message);

            return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
        }
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<object>>> DeleteSuite(int id)
    {
        if (id <= 0) return NotFound(ApiResponse<object>.NotFound($"Suite with Id {id} was not found"));

        try
        {
            var existingSuite = await _db.Suite.FindAsync(id);

            if (existingSuite is null) return NotFound(ApiResponse<object>.NotFound($"Suite with Id {id} was not found"));

            _db.Suite.Remove(existingSuite);
            await _db.SaveChangesAsync();

            var response = ApiResponse<object>.NoContent("Suite deleted successfully");

            return Ok(response);
        }
        catch (Exception ex)
        {
            var errorResponse = ApiResponse<object>.Error(StatusCodes.Status500InternalServerError, $"An error occurred while deleting the suite", ex.Message);

            return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
        }
    }
}
