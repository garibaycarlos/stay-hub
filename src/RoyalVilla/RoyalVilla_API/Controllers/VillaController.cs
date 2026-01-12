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
    public async Task<ActionResult<IEnumerable<VillaDTO>>> GetVillas()
    {
        var villas = await _db.Villa.ToListAsync();

        return Ok(_mapper.Map<List<VillaDTO>>(villas));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<VillaDTO>> GetVillaById(int id)
    {
        if (id <= 0) return BadRequest("Villa Id must be greater than 0");

        try
        {
            var villa = await _db.Villa.FindAsync(id);

            if (villa is null) return NotFound($"Villa with Id {id} was not found");

            return Ok(_mapper.Map<VillaDTO>(villa));
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while retrieving villa with Id {id}: {ex.Message}");
        }
    }

    [HttpPost]
    public async Task<ActionResult<VillaCreateDTO>> CreateVilla(VillaDTO villaDTO)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            var duplicateVilla = _db.Villa.FirstOrDefault(v => v.Name.ToLower() == villaDTO.Name.ToLower());

            if (duplicateVilla is not null) return Conflict($"A villa with the name '{villaDTO.Name}' already exists");

            var villa = _mapper.Map<Villa>(villaDTO);

            villa.CreatedDate = DateTime.UtcNow;

            await _db.Villa.AddAsync(villa);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(CreateVilla), new { id = villa.Id }, _mapper.Map<VillaDTO>(villa));
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while creating the villa: {ex.Message}");
        }
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<VillaUpdateDTO>> UpdateVilla(int id, VillaUpdateDTO villaDTO)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        if (id != villaDTO.Id) return BadRequest("Villa Id mismatch");

        try
        {
            var existingVilla = await _db.Villa.FindAsync(id);

            if (existingVilla is null) return NotFound($"Villa with Id {id} was not found");

            var duplicateVilla = _db.Villa.FirstOrDefault(v => v.Name.ToLower() == villaDTO.Name.ToLower() && v.Id != id);

            if (duplicateVilla is not null) return Conflict($"A villa with the name '{villaDTO.Name}' already exists");

            _mapper.Map(villaDTO, existingVilla);

            existingVilla.UpdatedDate = DateTime.UtcNow;

            await _db.SaveChangesAsync();

            return Ok(villaDTO);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while updating the villa: {ex.Message}");
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteVilla(int id)
    {
        if (id <= 0) return BadRequest("Villa Id must be greater than 0");

        try
        {
            var existingVilla = await _db.Villa.FindAsync(id);

            if (existingVilla is null) return NotFound($"Villa with Id {id} was not found");

            _db.Villa.Remove(existingVilla);
            await _db.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while deleting the villa: {ex.Message}");
        }
    }
}
