using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using CRM.Web.Data;
using CRM.Web.Models;

namespace CRM.Web.Features.Prospects;

[Authorize]
public class ProspectsController : Controller
{
    private readonly ApplicationDbContext _context;

    public ProspectsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Prospects
    public async Task<IActionResult> Index()
    {
        var prospects = await _context.Prospects
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
        return View(prospects);
    }

    // GET: Prospects/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var prospect = await _context.Prospects
            .Include(p => p.Communications)
            .FirstOrDefaultAsync(m => m.Id == id);
        
        if (prospect == null)
        {
            return NotFound();
        }

        return View(prospect);
    }

    // GET: Prospects/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Prospects/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("CompanyName,ContactPerson,Email,Phone,Address,Status,EstimatedValue,Notes")] Prospect prospect)
    {
        if (ModelState.IsValid)
        {
            prospect.CreatedAt = DateTime.UtcNow;
            _context.Add(prospect);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(prospect);
    }

    // GET: Prospects/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var prospect = await _context.Prospects.FindAsync(id);
        if (prospect == null)
        {
            return NotFound();
        }
        return View(prospect);
    }

    // POST: Prospects/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,CompanyName,ContactPerson,Email,Phone,Address,Status,EstimatedValue,Notes,CreatedAt")] Prospect prospect)
    {
        if (id != prospect.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                prospect.UpdatedAt = DateTime.UtcNow;
                _context.Update(prospect);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProspectExists(prospect.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(prospect);
    }

    // GET: Prospects/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var prospect = await _context.Prospects
            .FirstOrDefaultAsync(m => m.Id == id);
        if (prospect == null)
        {
            return NotFound();
        }

        return View(prospect);
    }

    // POST: Prospects/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var prospect = await _context.Prospects.FindAsync(id);
        if (prospect != null)
        {
            _context.Prospects.Remove(prospect);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    private bool ProspectExists(int id)
    {
        return _context.Prospects.Any(e => e.Id == id);
    }
}
