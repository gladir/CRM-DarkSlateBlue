using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using CRM.Web.Data;
using CRM.Web.Models;

namespace CRM.Web.Features.Suppliers;

[Authorize]
public class SuppliersController : Controller
{
    private readonly ApplicationDbContext _context;

    public SuppliersController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Suppliers - Affiche la liste des fournisseurs
    public async Task<IActionResult> Index()
    {
        var suppliers = await _context.Suppliers
            .OrderByDescending(s => s.CreatedAt)
            .ToListAsync();
        return View(suppliers);
    }

    // GET: Suppliers/Details/5 - Affiche les détails
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var supplier = await _context.Suppliers
            .Include(s => s.Communications)
            .FirstOrDefaultAsync(m => m.Id == id);
        
        if (supplier == null)
        {
            return NotFound();
        }

        return View(supplier);
    }

    // GET: Suppliers/Create - Affiche le formulaire de création
    public IActionResult Create()
    {
        return View();
    }

    // POST: Suppliers/Create - Traite la création
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("CompanyName,ContactPerson,Email,Phone,Address,TaxNumber,Category")] Supplier supplier)
    {
        if (ModelState.IsValid)
        {
            supplier.CreatedAt = DateTime.UtcNow;
            _context.Add(supplier);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(supplier);
    }

    // GET: Suppliers/Edit/5 - Affiche le formulaire de modification
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var supplier = await _context.Suppliers.FindAsync(id);
        if (supplier == null)
        {
            return NotFound();
        }
        return View(supplier);
    }

    // POST: Suppliers/Edit/5 - Traite la modification
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,CompanyName,ContactPerson,Email,Phone,Address,TaxNumber,Category,CreatedAt")] Supplier supplier)
    {
        if (id != supplier.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                supplier.UpdatedAt = DateTime.UtcNow;
                _context.Update(supplier);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SupplierExists(supplier.Id))
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
        return View(supplier);
    }

    // GET: Suppliers/Delete/5 - Affiche la page de confirmation de suppression
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var supplier = await _context.Suppliers
            .FirstOrDefaultAsync(m => m.Id == id);
        if (supplier == null)
        {
            return NotFound();
        }

        return View(supplier);
    }

    // POST: Suppliers/Delete/5 - Traite la suppression
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var supplier = await _context.Suppliers.FindAsync(id);
        if (supplier != null)
        {
            _context.Suppliers.Remove(supplier);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    private bool SupplierExists(int id)
    {
        return _context.Suppliers.Any(e => e.Id == id);
    }
}
