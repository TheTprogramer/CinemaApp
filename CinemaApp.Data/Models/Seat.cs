using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaApp.Data.Models
{
    /// <summary>
    /// Представлява седалка.
    /// </summary>
    [Table("seats")]
    public class Seat
    {
        /// <summary>Уникален идентификатор на седалката.</summary>
        [Key]
        [Column("seat_id")]
        public int SeatId { get; set; }

        /// <summary>Места (свързваща таблица).</summary>
        public ICollection<Place> Places { get; set; } = new List<Place>();
    }
}
