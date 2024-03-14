using bdaAPI.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase
{
    private readonly MyDbContext _context;

    public AccountController(MyDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Account>>> Get()
    {
        return await _context.Account.ToListAsync();
    }

    //[Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult<Account>> GetAccount(int id)
    {
        var Account = await _context.Account.FindAsync(id);

        if (Account == null)
        {
            return NotFound();
        }

        return Account;
    }

    [HttpPost]
    public async Task<ActionResult<Account>> PostAccount(Account Account)
    {
        _context.Account.Add(Account);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetAccount", new { id = Account.ID }, Account);
    }
}