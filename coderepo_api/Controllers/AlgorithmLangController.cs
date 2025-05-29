using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using coderepo_api.Data;
using coderepo_api.Models;
using Npgsql;

namespace coderepo_api.Controllers;

[ApiController]
[Route("api/algorithm/lang")]

public class AlgorithmLangController : ControllerBase
{
    private readonly AppDbContext _context;
    public AlgorithmLangController(AppDbContext context)
    {
        _context = context;
    }

    /*GET BY ID*/
    [HttpGet("{algorithmid}")]
    public async Task<IActionResult> GetAlgorithmsLang(int algorithmid)
    {
        var algorithm_lang = await _context.Algorithms.Include(p => p.Langs).Where(p => p.algorithm_id == algorithmid).Select(p => new
        {
            p.algorithm_id,
            p.owner_id,
            p.title,
            p.description,
            LangCount = p.Langs.Count,
            Langs = p.Langs.Select(lang => new {
                lang.lang_id,
                lang.rootlang_path,
                lang.audit_isdeleted
            }
            ).ToList()
            
        }).FirstOrDefaultAsync();

        if (algorithm_lang == null)
        {
            return NotFound();
        }
        return Ok(algorithm_lang);
    }
}