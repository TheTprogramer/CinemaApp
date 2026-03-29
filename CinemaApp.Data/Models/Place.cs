using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaApp.Data.Models
{
    /// <summary>
    /// Свързваща таблица — зала, ред, седалка и билет.
    /// Composite PK: (seat_id, rows_id, room_id, ticket_id).
    /// </summary>
    [Table("places")]
    public class Place
    {
        /// <summary>Идентификатор на билета.</summary>
        [Column("ticket_id")]
        public int TicketId { get; set; }

        /// <summary>Идентификатор на залата.</summary>
        [Column("room_id")]
        public int RoomId { get; set; }

        /// <summary>Идентификатор на реда.</summary>
        [Column("rows_id")]
        public int RowsId { get; set; }

        /// <summary>Идентификатор на седалката.</summary>
        [Column("seat_id")]
        public int SeatId { get; set; }

        /// <summary>Тип на мястото (normal/comfort, VIP/comfort и др.).</summary>
        [Required]
        [MaxLength(30)]
        [Column("place_type")]
        public string PlaceType { get; set; } = string.Empty;

        /// <summary>Навигационно свойство към билет.</summary>
        [ForeignKey(nameof(TicketId))]
        public Ticket Ticket { get; set; } = null!;

        /// <summary>Навигационно свойство към зала.</summary>
        [ForeignKey(nameof(RoomId))]
        public Room Room { get; set; } = null!;

        /// <summary>Навигационно свойство към ред.</summary>
        [ForeignKey(nameof(RowsId))]
        public Row Row { get; set; } = null!;

        /// <summary>Навигационно свойство към седалка.</summary>
        [ForeignKey(nameof(SeatId))]
        public Seat Seat { get; set; } = null!;
    }
}
