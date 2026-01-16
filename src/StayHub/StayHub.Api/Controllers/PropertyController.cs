using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StayHub.Api.Data;
using StayHub.Api.Models;
using StayHub.Api.Models.DTO;
using StayHub.Api.Models.DTO.Property;

namespace StayHub.Api.Controllers;

[ApiController]
[Route("api/properties")]
[Authorize(Roles = "Admin,Customer")]
public class PropertyController(ApplicationDbContext db, IMapper mapper) : ControllerBase
{
    private readonly ApplicationDbContext _db = db;
    private readonly IMapper _mapper = mapper;

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<PropertyDTO>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<IEnumerable<PropertyDTO>>>> GetProperties()
    {
        var propertiess = await _db.Property.ToListAsync();
        var dtoResponseProperty = _mapper.Map<IEnumerable<PropertyDTO>>(propertiess);

        var response = ApiResponse<IEnumerable<PropertyDTO>>.Ok(dtoResponseProperty, "Properties retrieved successfully");

        return Ok(response);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<PropertyDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<PropertyDTO>> GetPropertyById(int id)
    {
        if (id <= 0) return NotFound(ApiResponse<object>.NotFound($"Property with Id {id} was not found"));

        try
        {
            var properties = await _db.Property.FindAsync(id);

            if (properties is null) return NotFound(ApiResponse<object>.NotFound($"Property with Id {id} was not found"));

            return Ok(ApiResponse<PropertyDTO>.Ok(_mapper.Map<PropertyDTO>(properties), "Records retrieved successfully"));
        }
        catch (Exception ex)
        {
            var errorResponse = ApiResponse<object>.Error(StatusCodes.Status500InternalServerError, $"An error occurred while retrieving properties with Id {id}", ex.Message);

            return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
        }
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ApiResponse<PropertyCreateDTO>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<PropertyCreateDTO>>> CreateProperty(PropertyCreateDTO propertiesDTO)
    {
        if (propertiesDTO is null) return BadRequest(ApiResponse<object>.BadRequest("Property data is required"));

        try
        {
            var duplicateProperty = _db.Property.FirstOrDefault(v => v.Name.ToLower() == propertiesDTO.Name.ToLower());

            if (duplicateProperty is not null) return Conflict(ApiResponse<object>.Conflict($"A properties with the name '{propertiesDTO.Name}' already exists"));

            var properties = _mapper.Map<Property>(propertiesDTO);

            properties.CreatedDate = DateTime.UtcNow;

            await _db.Property.AddAsync(properties);
            await _db.SaveChangesAsync();

            var response = ApiResponse<PropertyDTO>.CreatedAt(_mapper.Map<PropertyDTO>(properties), "Property created successfully");

            return CreatedAtAction(nameof(CreateProperty), new { id = properties.Id }, response);
        }
        catch (Exception ex)
        {
            var errorResponse = ApiResponse<object>.Error(StatusCodes.Status500InternalServerError, $"An error occurred while creating the properties", ex.Message);

            return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
        }
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ApiResponse<PropertyDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<PropertyDTO>>> UpdateProperty(int id, PropertyUpdateDTO propertiesDTO)
    {
        if (propertiesDTO is null) return BadRequest(ApiResponse<object>.BadRequest("Property data is required"));
        if (id != propertiesDTO.Id) return BadRequest(ApiResponse<object>.BadRequest("Property Id in URL does not match Property Id in request body"));

        try
        {
            var existingProperty = await _db.Property.FindAsync(id);

            if (existingProperty is null) return NotFound(ApiResponse<object>.NotFound($"Property with Id {id} was not found"));

            var duplicateProperty = _db.Property.FirstOrDefault(v => v.Name.ToLower() == propertiesDTO.Name.ToLower() && v.Id != id);

            if (duplicateProperty is not null) return Conflict(ApiResponse<PropertyCreateDTO>.Conflict($"A properties with the name '{propertiesDTO.Name}' already exists"));

            _mapper.Map(propertiesDTO, existingProperty);

            existingProperty.UpdatedDate = DateTime.UtcNow;

            await _db.SaveChangesAsync();

            var response = ApiResponse<PropertyDTO>.Ok(_mapper.Map<PropertyDTO>(propertiesDTO), "Property updated successfully");

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
    public async Task<ActionResult<ApiResponse<object>>> DeleteProperty(int id)
    {
        if (id <= 0) return NotFound(ApiResponse<object>.NotFound($"Property with Id {id} was not found"));

        try
        {
            var existingProperty = await _db.Property.FindAsync(id);

            if (existingProperty is null) return NotFound(ApiResponse<object>.NotFound($"Property with Id {id} was not found"));

            _db.Property.Remove(existingProperty);
            await _db.SaveChangesAsync();

            var response = ApiResponse<object>.NoContent("Property deleted successfully");

            return Ok(response);
        }
        catch (Exception ex)
        {
            var errorResponse = ApiResponse<object>.Error(StatusCodes.Status500InternalServerError, $"An error occurred while deleting the properties", ex.Message);

            return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
        }
    }
}
