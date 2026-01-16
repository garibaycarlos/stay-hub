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
    public async Task<ActionResult<ApiResponse<IEnumerable<SuiteDTO>>>> GetProperties()
    {
        var propertiess = await _db.Suite.ToListAsync();
        var dtoResponseSuite = _mapper.Map<IEnumerable<SuiteDTO>>(propertiess);

        var response = ApiResponse<IEnumerable<SuiteDTO>>.Ok(dtoResponseSuite, "Properties retrieved successfully");

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
            var properties = await _db.Suite.FindAsync(id);

            if (properties is null) return NotFound(ApiResponse<object>.NotFound($"Suite with Id {id} was not found"));

            return Ok(ApiResponse<SuiteDTO>.Ok(_mapper.Map<SuiteDTO>(properties), "Records retrieved successfully"));
        }
        catch (Exception ex)
        {
            var errorResponse = ApiResponse<object>.Error(StatusCodes.Status500InternalServerError, $"An error occurred while retrieving properties with Id {id}", ex.Message);

            return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
        }
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ApiResponse<SuiteCreateDTO>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<SuiteCreateDTO>>> CreateSuite(SuiteCreateDTO propertiesDTO)
    {
        if (propertiesDTO is null) return BadRequest(ApiResponse<object>.BadRequest("Suite data is required"));

        try
        {
            var duplicateSuite = _db.Suite.FirstOrDefault(v => v.Name.ToLower() == propertiesDTO.Name.ToLower());

            if (duplicateSuite is not null) return Conflict(ApiResponse<object>.Conflict($"A properties with the name '{propertiesDTO.Name}' already exists"));

            var properties = _mapper.Map<Suite>(propertiesDTO);

            properties.CreatedDate = DateTime.UtcNow;

            await _db.Suite.AddAsync(properties);
            await _db.SaveChangesAsync();

            var response = ApiResponse<SuiteDTO>.CreatedAt(_mapper.Map<SuiteDTO>(properties), "Suite created successfully");

            return CreatedAtAction(nameof(CreateSuite), new { id = properties.Id }, response);
        }
        catch (Exception ex)
        {
            var errorResponse = ApiResponse<object>.Error(StatusCodes.Status500InternalServerError, $"An error occurred while creating the properties", ex.Message);

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
    public async Task<ActionResult<ApiResponse<SuiteDTO>>> UpdateSuite(int id, SuiteUpdateDTO propertiesDTO)
    {
        if (propertiesDTO is null) return BadRequest(ApiResponse<object>.BadRequest("Suite data is required"));
        if (id != propertiesDTO.Id) return BadRequest(ApiResponse<object>.BadRequest("Suite Id in URL does not match Suite Id in request body"));

        try
        {
            var existingSuite = await _db.Suite.FindAsync(id);

            if (existingSuite is null) return NotFound(ApiResponse<object>.NotFound($"Suite with Id {id} was not found"));

            var duplicateSuite = _db.Suite.FirstOrDefault(v => v.Name.ToLower() == propertiesDTO.Name.ToLower() && v.Id != id);

            if (duplicateSuite is not null) return Conflict(ApiResponse<SuiteCreateDTO>.Conflict($"A properties with the name '{propertiesDTO.Name}' already exists"));

            _mapper.Map(propertiesDTO, existingSuite);

            existingSuite.UpdatedDate = DateTime.UtcNow;

            await _db.SaveChangesAsync();

            var response = ApiResponse<SuiteDTO>.Ok(_mapper.Map<SuiteDTO>(propertiesDTO), "Suite updated successfully");

            return Ok(propertiesDTO);
        }
        catch (Exception ex)
        {
            var errorResponse = ApiResponse<object>.Error(StatusCodes.Status500InternalServerError, $"An error occurred while updating the properties", ex.Message);

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
            var errorResponse = ApiResponse<object>.Error(StatusCodes.Status500InternalServerError, $"An error occurred while deleting the properties", ex.Message);

            return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
        }
    }
}
