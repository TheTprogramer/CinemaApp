using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaApp.Data.Models
{
    /// <summary>
    /// Представлява билет.
    /// </summary>
    [Table("tickets")]
    public class Ticket
    {
        /// <summary>Уникален идентификатор на билета.</summary>
        [Key]
        [Column("ticket_id")]
        public int TicketId { get; set; }

        /// <summary>Цена на билета.</summary>
        [Required]
        [Column("ticket_price")]
        public decimal TicketPrice { get; set; }

        /// <summary>Места, свързани с билета.</summary>
        public ICollection<Place> Places { get; set; } = new List<Place>();

        /// <summary>Клиенти, закупили билета.</summary>
        public ICollection<Client> Clients { get; set; } = new List<Client>();
    }
}
