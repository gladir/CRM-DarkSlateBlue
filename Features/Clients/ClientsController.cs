using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using CRM.Web.Data;
using CRM.Web.Models;

namespace CRM.Web.Features.Clients;

[Authorize]
public class ClientsController : Controller
{
    private readonly ApplicationDbContext _context;

    public ClientsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Clients - Affiche la liste des clients
    public async Task<IActionResult> Index()
    {
        var clients = await _context.Clients
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
        return View(clients);
    }

    // GET: Clients/Details/5 - Affiche les détails d'un client
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var client = await _context.Clients
            .Include(c => c.Communications)
            .FirstOrDefaultAsync(m => m.Id == id);
        
        if (client == null)
        {
            return NotFound();
        }

        return View(client);
    }

    // GET: Clients/Create - Affiche le formulaire de création d'un client
    public IActionResult Create()
    {
        return View();
    }

    // POST: Clients/Create - Traite la création d'un nouveau client
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("CompanyName,ContactPerson,Email,Phone,Address,TaxNumber,CreditLimit")] Client client)
    {
        if (ModelState.IsValid)
        {
            client.CreatedAt = DateTime.UtcNow;
            _context.Add(client);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(client);
    }

    // GET: Clients/Edit/5 - Affiche le formulaire de modification d'un client
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var client = await _context.Clients.FindAsync(id);
        if (client == null)
        {
            return NotFound();
        }
        return View(client);
    }

    // POST: Clients/Edit/5 - Traite la modification d'un client
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,CompanyName,ContactPerson,Email,Phone,Address,TaxNumber,CreditLimit,CreatedAt")] Client client)
    {
        if (id != client.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                client.UpdatedAt = DateTime.UtcNow;
                _context.Update(client);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClientExists(client.Id))
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
        return View(client);
    }

    // GET: Clients/Delete/5 - Affiche la page de confirmation de suppression
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var client = await _context.Clients
            .FirstOrDefaultAsync(m => m.Id == id);
        if (client == null)
        {
            return NotFound();
        }

        return View(client);
    }

    // POST: Clients/Delete/5 - Traite la suppression d'un client
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var client = await _context.Clients.FindAsync(id);
        if (client != null)
        {
            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    private bool ClientExists(int id)
    {
        return _context.Clients.Any(e => e.Id == id);
    }
}
