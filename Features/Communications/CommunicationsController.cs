using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using CRM.Web.Data;
using CRM.Web.Models;

namespace CRM.Web.Features.Communications;

[Authorize]
public class CommunicationsController : Controller
{
    private readonly ApplicationDbContext _context;

    public CommunicationsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Communications - Affiche la liste des communications
    public async Task<IActionResult> Index()
    {
        var communications = await _context.Communications
            .Include(c => c.Contact)
            .Include(c => c.Client)
            .Include(c => c.Supplier)
            .Include(c => c.Prospect)
            .OrderByDescending(c => c.CommunicationDate)
            .ToListAsync();
        return View(communications);
    }

    // GET: Communications/Details/5 - Affiche les détails
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var communication = await _context.Communications
            .Include(c => c.Contact)
            .Include(c => c.Client)
            .Include(c => c.Supplier)
            .Include(c => c.Prospect)
            .FirstOrDefaultAsync(m => m.Id == id);
        
        if (communication == null)
        {
            return NotFound();
        }

        return View(communication);
    }

    // GET: Communications/Create - Affiche le formulaire de création
    public IActionResult Create(int? contactId, int? clientId, int? supplierId, int? prospectId)
    {
        ViewBag.Contacts = _context.Contacts.ToList();
        ViewBag.Clients = _context.Clients.ToList();
        ViewBag.Suppliers = _context.Suppliers.ToList();
        ViewBag.Prospects = _context.Prospects.ToList();

        var communication = new Communication
        {
            ContactId = contactId,
            ClientId = clientId,
            SupplierId = supplierId,
            ProspectId = prospectId
        };

        return View(communication);
    }

    // POST: Communications/Create - Traite la création
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Subject,Content,Type,CommunicationDate,ContactId,ClientId,SupplierId,ProspectId")] Communication communication)
    {
        if (ModelState.IsValid)
        {
            communication.CreatedAt = DateTime.UtcNow;
            _context.Add(communication);
            await _context.SaveChangesAsync();

            // Redirection vers l'entité associée si spécifiée
            if (communication.ContactId.HasValue)
                return RedirectToAction("Details", "Contacts", new { id = communication.ContactId });
            if (communication.ClientId.HasValue)
                return RedirectToAction("Details", "Clients", new { id = communication.ClientId });
            if (communication.SupplierId.HasValue)
                return RedirectToAction("Details", "Suppliers", new { id = communication.SupplierId });
            if (communication.ProspectId.HasValue)
                return RedirectToAction("Details", "Prospects", new { id = communication.ProspectId });

            return RedirectToAction(nameof(Index));
        }

        ViewBag.Contacts = _context.Contacts.ToList();
        ViewBag.Clients = _context.Clients.ToList();
        ViewBag.Suppliers = _context.Suppliers.ToList();
        ViewBag.Prospects = _context.Prospects.ToList();

        return View(communication);
    }

    // GET: Communications/Delete/5 - Affiche la page de confirmation de suppression
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var communication = await _context.Communications
            .Include(c => c.Contact)
            .Include(c => c.Client)
            .Include(c => c.Supplier)
            .Include(c => c.Prospect)
            .FirstOrDefaultAsync(m => m.Id == id);
        
        if (communication == null)
        {
            return NotFound();
        }

        return View(communication);
    }

    // POST: Communications/Delete/5 - Traite la suppression
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var communication = await _context.Communications.FindAsync(id);
        if (communication != null)
        {
            _context.Communications.Remove(communication);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }
}
