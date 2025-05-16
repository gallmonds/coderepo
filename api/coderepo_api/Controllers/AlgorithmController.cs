using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using coderepo_api.Data;
using coderepo_api.Models;
using Npgsql;

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
}