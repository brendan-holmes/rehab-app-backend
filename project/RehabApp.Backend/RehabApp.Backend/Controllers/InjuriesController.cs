using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RehabApp.Backend.Models;

namespace RehabApp.Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InjuriesController : ControllerBase
{
    private readonly ILogger<InjuriesController> _logger;
    private readonly InjuriesContext _context;

    public InjuriesController(ILogger<InjuriesController> logger, InjuriesContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpGet]
    public async Task<IEnumerable<InjuryDto>> GetAllAsync()
    {
        return await _context.Injuries
            .Select(x => InjuryToDto(x))
            .ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<InjuryDto>> GetInjury(Guid id)
    {
        var injury = await _context.Injuries.FindAsync(id);

        if (injury == null)
        {
            return NotFound();
        }

        return InjuryToDto(injury);
    }

    [HttpPost]
    public async Task<ActionResult<InjuryDto>> PostInjury(InjuryDto injuryDto)
    {
        var injury = new Injury
        {
            CreatedUtc = DateTime.UtcNow,
            UpdatedUtc = DateTime.UtcNow,
            Name = injuryDto.Name
        };

        _context.Injuries.Add(injury);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetInjury),
            new { id = injury.Id },
            InjuryToDto(injury));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<InjuryDto>> PutInjury(Guid id, [FromBody] InjuryDto injuryDto)
    {
        var injury = await _context.Injuries.FindAsync(id);

        if (injury == null)
        {
            return NotFound();
        }

        _context.Injuries.Update(injury);

        injury = new Injury
        {
            Id = injury.Id,
            CreatedUtc = injury.CreatedUtc,
            UpdatedUtc = DateTime.UtcNow,
            Name = injuryDto.Name
        };

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException) when (!InjuryExists(id))
        {
            return NotFound();
        }

        return Ok(InjuryToDto(injury));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteInjury(Guid id)
    {
        var injury = await _context.Injuries.FindAsync(id);
        if (injury == null)
        {
            return NotFound();
        }

        _context.Injuries.Remove(injury);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private static InjuryDto InjuryToDto(Injury injury) =>
        new InjuryDto
        {
            Id = injury.Id,
            CreateUtc = injury.CreatedUtc,
            UpdatedUtc = injury.UpdatedUtc,
            Name = injury.Name
        };

    private bool InjuryExists(Guid id)
    {
        return _context.Injuries.Any(e => e.Id == id);
    }
}
