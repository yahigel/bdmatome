using bdaAPI.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("[controller]")]
public class AnalyticalPersonController : ControllerBase
{
    private readonly MyDbContext _context;

    public AnalyticalPersonController(MyDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AnalyticalPerson>>> Get()
    {
        return await _context.AnalyticalPerson.ToListAsync();
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult<AnalyticalPerson>> GetAnalyticalPerson(int id)
    {
        var analyticalPerson = await _context.AnalyticalPerson.FindAsync(id);

        if (analyticalPerson == null)
        {
            return NotFound();
        }

        return analyticalPerson;
    }

    [HttpPost]
    public async Task<ActionResult<AnalyticalPerson>> PostAnalyticalPerson(AnalyticalPerson analyticalPerson)
    {
        _context.AnalyticalPerson.Add(analyticalPerson);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetAnalyticalPerson", new { id = analyticalPerson.ID }, analyticalPerson);
    }
}