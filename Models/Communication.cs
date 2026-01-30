using System.ComponentModel.DataAnnotations;

namespace CRM.Web.Models;

public enum CommunicationType
{
    Email,
    Phone,
    Meeting,
    Note,
    Other
}

public class Communication
{
    public int Id { get; set; }

    [Required]
    [StringLength(200)]
    public string Subject { get; set; } = string.Empty;

    [Required]
    public string Content { get; set; } = string.Empty;

    public CommunicationType Type { get; set; } = CommunicationType.Note;

    public DateTime CommunicationDate { get; set; } = DateTime.UtcNow;

    // Relations optionnelles avec les différentes entités
    public int? ContactId { get; set; }
    public Contact? Contact { get; set; }

    public int? ClientId { get; set; }
    public Client? Client { get; set; }

    public int? SupplierId { get; set; }
    public Supplier? Supplier { get; set; }

    public int? ProspectId { get; set; }
    public Prospect? Prospect { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
