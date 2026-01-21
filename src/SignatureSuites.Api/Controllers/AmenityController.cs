using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SignatureSuites.Api.Data;
using SignatureSuites.Api.Models;
using SignatureSuites.Api.Models.Dto;
using SignatureSuites.Api.Models.Dto.Amenity;

namespace SignatureSuites.Api.Controllers;

/// <summary>
/// Provides endpoints for managing amenities.
/// </summary>
[ApiController]
[Route("api/amenities")]
public class AmenityController(ApplicationDbContext db, IMapper mapper) : ControllerBase
{
    private readonly ApplicationDbContext _db = db;
    private readonly IMapper _mapper = mapper;

    /// <summary>
    /// Retrieves a list of all amenities.
    /// </summary>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>
    /// A standardized API response containing a collection of amenities.
    /// </returns>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<AmenityDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IEnumerable<AmenityDto>>>> GetAllAmenities(CancellationToken cancellationToken)
    {
        var amenities = await _db.Amenities
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        var amenitiesDto = _mapper.Map<IEnumerable<AmenityDto>>(amenities);
        var response = ApiResponse<IEnumerable<AmenityDto>>.Ok(amenitiesDto, "Amenities retrieved successfully");

        return Ok(response);
    }

    /// <summary>
    /// Retrieves a specific amenity by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the amenity.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>
    /// A standardized API response containing the requested amenity if found.
    /// </returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<AmenityDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<AmenityDto>>> GetAmenityById(int id, CancellationToken cancellationToken)
    {
        if (id <= 0) return BadRequest(ApiResponse<object>.BadRequest("Invalid amenity Id"));

        var amenity = await _db.Amenities
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);

        if (amenity is null) return NotFound(ApiResponse<object>.NotFound($"Amenity with Id {id} was not found"));

        return Ok(ApiResponse<AmenityDto>.Ok(_mapper.Map<AmenityDto>(amenity), "Amenity retrieved successfully"));
    }

    /// <summary>
    /// Creates a new amenity.
    /// </summary>
    /// <param name="amenityCreateDto">The data required to create a new amenity.</param>
    /// <returns>
    /// A standardized API response containing the newly created amenity.
    /// </returns>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ApiResponse<AmenityDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ApiResponse<AmenityDto>>> CreateAmenity(AmenityCreateDto amenityCreateDto)
    {
        if (amenityCreateDto is null) return BadRequest(ApiResponse<object>.BadRequest("Amenity data is required"));

        var amenityExists = await _db.Amenities.AnyAsync(v => v.Name == amenityCreateDto.Name);

        if (amenityExists) return Conflict(ApiResponse<object>.Conflict($"An amenity with the name '{amenityCreateDto.Name}' already exists"));

        var amenity = _mapper.Map<Amenity>(amenityCreateDto);

        await _db.Amenities.AddAsync(amenity);
        await _db.SaveChangesAsync();

        var response = ApiResponse<AmenityDto>.CreatedAt(_mapper.Map<AmenityDto>(amenity), "Amenity created successfully");

        return CreatedAtAction(nameof(GetAmenityById), new { id = amenity.Id }, response);
    }

    /// <summary>
    /// Updates an existing amenity.
    /// </summary>
    /// <param name="id">The unique identifier of the amenity to update.</param>
    /// <param name="amenityUpdateDto">The updated amenity data.</param>
    /// <returns>
    /// A standardized API response containing the updated amenity.
    /// </returns>
    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ApiResponse<AmenityDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ApiResponse<AmenityDto>>> UpdateAmenity(int id, AmenityUpdateDto amenityUpdateDto)
    {
        if (amenityUpdateDto is null) return BadRequest(ApiResponse<object>.BadRequest("Amenity data is required"));
        if (id != amenityUpdateDto.Id) return BadRequest(ApiResponse<object>.BadRequest("Amenity Id in URL does not match Amenity Id in request body"));

        var existingAmenity = await _db.Amenities.FindAsync(id);

        if (existingAmenity is null) return NotFound(ApiResponse<object>.NotFound($"Amenity with Id {id} was not found"));

        var amenityNameExists = await _db.Amenities.AnyAsync(v => v.Name == amenityUpdateDto.Name && v.Id != id);

        if (amenityNameExists) return Conflict(ApiResponse<object>.Conflict($"An amenity with the name '{amenityUpdateDto.Name}' already exists"));

        _mapper.Map(amenityUpdateDto, existingAmenity);

        existingAmenity.UpdatedDate = DateTime.UtcNow;

        await _db.SaveChangesAsync();

        var response = ApiResponse<AmenityDto>.Ok(_mapper.Map<AmenityDto>(existingAmenity), "Amenity updated successfully");

        return Ok(response);
    }

    /// <summary>
    /// Deletes an existing amenity by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the amenity to delete.</param>
    /// <returns>
    /// A standardized API response indicating the result of the deletion.
    /// </returns>
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<object>>> DeleteAmenity(int id)
    {
        if (id <= 0) return BadRequest(ApiResponse<object>.BadRequest("Invalid amenity Id"));

        var existingAmenity = await _db.Amenities.FindAsync(id);

        if (existingAmenity is null) return NotFound(ApiResponse<object>.NotFound($"Amenity with Id {id} was not found"));

        _db.Amenities.Remove(existingAmenity);
        await _db.SaveChangesAsync();

        return NoContent();
    }
}
