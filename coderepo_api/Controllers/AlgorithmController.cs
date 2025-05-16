using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using coderepo_api.Data;
using coderepo_api.Models;
using Npgsql;
using coderepo_api.Dtos;

namespace coderepo_api.Controllers;

[ApiController]
[Route("api/algorithm")]

public class AlgorithmController : ControllerBase
{
    private readonly AppDbContext _context;
    public AlgorithmController(AppDbContext context)
    {
        _context = context;
    }

    /*GET ALL ALGORITHM*/
    [HttpGet]
    public async Task<IActionResult> GetAlgorithms()
    {
        var algorithm = await _context.Algorithms.Include(i => i.Meta).Select(i => new
        {
            i.algorithm_id,
            i.owner_id,
            i.title,
            i.description,
            Meta = new
            {
                i.Meta.root_path,
                i.Meta.isprivate,
                i.Meta.isflagged,
                i.Meta.isdisabled,
                i.Meta.audit_isdeleted
            }
        }).ToListAsync();
        return Ok(algorithm);
    }

    /*GET BY ID*/
    [HttpGet("{algorithmid}")]
    public async Task<IActionResult> GetAlgorithmsById(int algorithmid)
    {
        var algorithm = await _context.Algorithms.Include(i => i.Meta).Where(i => i.algorithm_id == algorithmid).Select(i => new
        {
            i.algorithm_id,
            i.owner_id,
            i.title,
            i.description,
            Meta = new
            {
                i.Meta.root_path,
                i.Meta.isprivate,
                i.Meta.isflagged,
                i.Meta.isdisabled,
                i.Meta.audit_isdeleted
            }
        }).ToListAsync();

        if (algorithm == null)
        {
            return NotFound();
        }

        return Ok(algorithm);
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateAlgorithm ([FromBody] CreateAlgorithmDto dto)
    {
        try
        {
            using var conn = (NpgsqlConnection)_context.Database.GetDbConnection();
            await conn.OpenAsync();
            using var cmd = new NpgsqlCommand("CALL sp_create_algorithm(@p_title, @p_description, @p_isprivate, @p_userid );", conn);
            cmd.Parameters.AddWithValue("p_title", dto.algorithm_title);
            cmd.Parameters.AddWithValue("p_description", dto.algorithm_description);
            cmd.Parameters.AddWithValue("p_isprivate", dto.algorithm_isprivate);
            cmd.Parameters.AddWithValue("p_userid", dto.dbuser_id);
            await cmd.ExecuteNonQueryAsync();
            return Ok(new { message = "User created successfully" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    } 
    
}