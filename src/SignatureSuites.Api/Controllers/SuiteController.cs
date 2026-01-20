using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SignatureSuites.Api.Data;
using SignatureSuites.Api.Models;
using SignatureSuites.Api.Models.Dto;
using SignatureSuites.Api.Models.Dto.Suite;

namespace SignatureSuites.Api.Controllers;

/// <summary>
/// Provides endpoints for managing suites and their related data.
/// </summary>
[ApiController]
[Route("api/suites")]
[Authorize(Roles = "Admin,Customer")]
public class SuiteController(ApplicationDbContext db, IMapper mapper) : ControllerBase
{
    private readonly ApplicationDbContext _db = db;
    private readonly IMapper _mapper = mapper;

    /// <summary>
    /// Retrieves a list of all suites including their associated amenities.
    /// </summary>
    /// <returns>
    /// A standardized API response containing a collection of suites.
    /// </returns>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<SuiteDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<IEnumerable<SuiteDto>>>> GetAllSuites()
    {
        var suites = await QuerySuitesWithAmenities()
                .AsNoTracking()
                .ToListAsync();
        var suitesDto = _mapper.Map<IEnumerable<SuiteDto>>(suites);
        var response = ApiResponse<IEnumerable<SuiteDto>>.Ok(suitesDto, "Suites retrieved successfully");

        return Ok(response);
    }

    /// <summary>
    /// Retrieves a specific suite by its unique identifier, including its amenities.
    /// </summary>
    /// <param name="id">The unique identifier of the suite.</param>
    /// <returns>
    /// A standardized API response containing the requested suite if found.
    /// </returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<SuiteDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<SuiteDto>>> GetSuiteById(int id)
    {
        if (id <= 0) return NotFound(ApiResponse<object>.NotFound($"Suite with Id {id} was not found"));

        var suite = await QuerySuitesWithAmenities()
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == id);

        if (suite is null) return NotFound(ApiResponse<object>.NotFound($"Suite with Id {id} was not found"));

        return Ok(ApiResponse<SuiteDto>.Ok(_mapper.Map<SuiteDto>(suite), "Suite retrieved successfully"));
    }

    /// <summary>
    /// Creates a new suite.
    /// </summary>
    /// <param name="suiteCreateDto">The data required to create a new suite.</param>
    /// <returns>
    /// A standardized API response containing the newly created suite.
    /// </returns>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ApiResponse<SuiteCreateDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<SuiteCreateDto>>> CreateSuite(SuiteCreateDto suiteCreateDto)
    {
        if (suiteCreateDto is null) return BadRequest(ApiResponse<object>.BadRequest("Suite data is required"));

        var suiteExists = await _db.Suites.AnyAsync(v => v.Name.ToLower() == suiteCreateDto.Name.ToLower());

        if (suiteExists) return Conflict(ApiResponse<object>.Conflict($"A suite with the name '{suiteCreateDto.Name}' already exists"));

        var suite = _mapper.Map<Suite>(suiteCreateDto);

        await _db.Suites.AddAsync(suite);
        await _db.SaveChangesAsync();

        var response = ApiResponse<SuiteDto>.CreatedAt(_mapper.Map<SuiteDto>(suite), "Suite created successfully");

        return CreatedAtAction(nameof(GetSuiteById), new { id = suite.Id }, response);
    }

    /// <summary>
    /// Updates an existing suite.
    /// </summary>
    /// <param name="id">The unique identifier of the suite to update.</param>
    /// <param name="suiteUpdateDto">The updated suite data.</param>
    /// <returns>
    /// A standardized API response containing the updated suite.
    /// </returns>
    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ApiResponse<SuiteDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<SuiteDto>>> UpdateSuite(int id, SuiteUpdateDto suiteUpdateDto)
    {
        if (suiteUpdateDto is null) return BadRequest(ApiResponse<object>.BadRequest("Suite data is required"));
        if (id != suiteUpdateDto.Id) return BadRequest(ApiResponse<object>.BadRequest("Suite Id in URL does not match Suite Id in request body"));

        var existingSuite = await _db.Suites.FindAsync(id);

        if (existingSuite is null) return NotFound(ApiResponse<object>.NotFound($"Suite with Id {id} was not found"));

        var suiteNameExists = await _db.Suites.AnyAsync(v => v.Name.ToLower() == suiteUpdateDto.Name.ToLower() && v.Id != id);

        if (suiteNameExists) return Conflict(ApiResponse<SuiteCreateDto>.Conflict($"A suite with the name '{suiteUpdateDto.Name}' already exists"));

        _mapper.Map(suiteUpdateDto, existingSuite);

        existingSuite.UpdatedDate = DateTime.UtcNow;

        await _db.SaveChangesAsync();

        var response = ApiResponse<SuiteDto>.Ok(_mapper.Map<SuiteDto>(existingSuite), "Suite updated successfully");

        return Ok(response);
    }

    /// <summary>
    /// Deletes an existing suite by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the suite to delete.</param>
    /// <returns>
    /// A standardized API response indicating the result of the deletion.
    /// </returns>
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<object>>> DeleteSuite(int id)
    {
        if (id <= 0) return BadRequest(ApiResponse<object>.BadRequest("Invalid suite Id"));

        var existingSuite = await _db.Suites.FindAsync(id);

        if (existingSuite is null) return NotFound(ApiResponse<object>.NotFound($"Suite with Id {id} was not found"));

        _db.Suites.Remove(existingSuite);
        await _db.SaveChangesAsync();

        return NoContent();
    }

    private IQueryable<Suite> QuerySuitesWithAmenities()
    {
        return _db.Suites
            .Include(s => s.SuiteAmenities)
            .ThenInclude(sa => sa.Amenity);
    }
}
