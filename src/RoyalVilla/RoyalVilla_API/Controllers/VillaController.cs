using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoyalVilla_API.Data;
using RoyalVilla_API.Models;
using RoyalVilla_API.Models.DTO;

namespace RoyalVilla_API.Controllers;

[ApiController]
[Route("api/villa")]
public class VillaController(ApplicationDbContext db, IMapper mapper) : ControllerBase
{
    private readonly ApplicationDbContext _db = db;
    private readonly IMapper _mapper = mapper;

    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<VillaDTO>>>> GetVillas()
    {
        var villas = await _db.Villa.ToListAsync();
        var dtoResponseVilla = _mapper.Map<IEnumerable<VillaDTO>>(villas);

        var response = ApiResponse<IEnumerable<VillaDTO>>.Ok(dtoResponseVilla, "Villas retrieved successfully");

        return Ok(response);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<VillaDTO>> GetVillaById(int id)
    {
        if (id <= 0) return NotFound(ApiResponse<object>.NotFound($"Villa with Id {id} was not found"));

        try
        {
            var villa = await _db.Villa.FindAsync(id);

            if (villa is null) return NotFound(ApiResponse<object>.NotFound($"Villa with Id {id} was not found"));

            return Ok(ApiResponse<VillaDTO>.Ok(_mapper.Map<VillaDTO>(villa), "Records retrieved successfully"));
        }
        catch (Exception ex)
        {
            var errorResponse = ApiResponse<object>.Error(StatusCodes.Status500InternalServerError, $"An error occurred while retrieving villa with Id {id}: {ex.Message}");

            return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
        }
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<VillaCreateDTO>>> CreateVilla(VillaCreateDTO villaDTO)
    {
        if (villaDTO is null) return BadRequest(ApiResponse<object>.BadRequest("Villa data is required"));

        try
        {
            var duplicateVilla = _db.Villa.FirstOrDefault(v => v.Name.ToLower() == villaDTO.Name.ToLower());

            if (duplicateVilla is not null) return Conflict(ApiResponse<VillaCreateDTO>.Conflict($"A villa with the name '{villaDTO.Name}' already exists"));

            var villa = _mapper.Map<Villa>(villaDTO);

            villa.CreatedDate = DateTime.UtcNow;

            await _db.Villa.AddAsync(villa);
            await _db.SaveChangesAsync();

            var response = ApiResponse<VillaDTO>.CreatedAt(_mapper.Map<VillaDTO>(villa), "Villa created successfully");

            return CreatedAtAction(nameof(CreateVilla), new { id = villa.Id }, response);
        }
        catch (Exception ex)
        {
            var errorResponse = ApiResponse<object>.Error(StatusCodes.Status500InternalServerError, $"An error occurred while creating the villa: {ex.Message}");

            return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
        }
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ApiResponse<VillaDTO>>> UpdateVilla(int id, VillaUpdateDTO villaDTO)
    {
        if (villaDTO is null) return BadRequest(ApiResponse<object>.BadRequest("Villa data is required"));
        if (id != villaDTO.Id) return BadRequest(ApiResponse<object>.BadRequest("Villa Id in URL does not match Villa Id in request body"));

        try
        {
            var existingVilla = await _db.Villa.FindAsync(id);

            if (existingVilla is null) return NotFound(ApiResponse<object>.NotFound($"Villa with Id {id} was not found"));

            var duplicateVilla = _db.Villa.FirstOrDefault(v => v.Name.ToLower() == villaDTO.Name.ToLower() && v.Id != id);

            if (duplicateVilla is not null) return Conflict(ApiResponse<VillaCreateDTO>.Conflict($"A villa with the name '{villaDTO.Name}' already exists"));

            _mapper.Map(villaDTO, existingVilla);

            existingVilla.UpdatedDate = DateTime.UtcNow;

            await _db.SaveChangesAsync();

            var response = ApiResponse<VillaDTO>.Ok(_mapper.Map<VillaDTO>(villaDTO), "Villa updated successfully");

            return Ok(villaDTO);
        }
        catch (Exception ex)
        {
            var errorResponse = ApiResponse<object>.Error(StatusCodes.Status500InternalServerError, $"An error occurred while updating the villa: {ex.Message}");

            return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ApiResponse<object>>> DeleteVilla(int id)
    {
        if (id <= 0) return NotFound(ApiResponse<object>.NotFound($"Villa with Id {id} was not found"));

        try
        {
            var existingVilla = await _db.Villa.FindAsync(id);

            if (existingVilla is null) return NotFound(ApiResponse<object>.NotFound($"Villa with Id {id} was not found"));

            _db.Villa.Remove(existingVilla);
            await _db.SaveChangesAsync();

            var response = ApiResponse<object>.NoContent("Villa deleted successfully");

            return Ok(response);
        }
        catch (Exception ex)
        {
            var errorResponse = ApiResponse<object>.Error(StatusCodes.Status500InternalServerError, $"An error occurred while deleting the villa: {ex.Message}");

            return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
        }
    }
}
