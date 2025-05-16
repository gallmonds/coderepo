using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using coderepo_api.Data;
using coderepo_api.Models;
using Npgsql;

namespace coderepo_api.Controllers;

[ApiController]
[Route("api/user")]

public class UserController : ControllerBase
{
    private readonly AppDbContext _context;
    public UserController(AppDbContext context)
    {
        _context = context;
    }

    /*GET ALL USER*/
    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        var user = await _context.Users.ToListAsync();
        return Ok(user);
    }

    /*GET USER BY ID*/

    [HttpGet("{user_id}")]
    public async Task<IActionResult> GetUsersById(int user_id)
    {
        var user = await _context.Users.FindAsync(user_id);
        if (user == null)
        {
            return NotFound();
        }
        return Ok(user);
    }

    

}