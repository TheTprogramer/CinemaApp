using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaApp.Data.Models
{
    /// <summary>
    /// Представлява клиент, закупил билет с храна и напитка.
    /// </summary>
    [Table("clients")]
    public class Client
    {
        /// <summary>Уникален идентификатор на клиента.</summary>
        [Key]
        [Column("client_id")]
        public int ClientId { get; set; }

        /// <summary>Ден от седмицата, в който е дошъл клиентът.</summary>
        [Required]
        [MaxLength(10)]
        [Column("day_of_week")]
        public string DayOfWeek { get; set; } = string.Empty;

        /// <summary>Идентификатор на билета.</summary>
        [Column("ticket_id")]
        public int? TicketId { get; set; }

        /// <summary>Идентификатор на храната.</summary>
        [Column("food_id")]
        public int? FoodId { get; set; }

        /// <summary>Идентификатор на напитката.</summary>
        [Column("drink_id")]
        public int? DrinkId { get; set; }

        /// <summary>Навигационно свойство към билет.</summary>
        [ForeignKey(nameof(TicketId))]
        public Ticket? Ticket { get; set; }

        /// <summary>Навигационно свойство към храна.</summary>
        [ForeignKey(nameof(FoodId))]
        public Food? Food { get; set; }

        /// <summary>Навигационно свойство към напитка.</summary>
        [ForeignKey(nameof(DrinkId))]
        public Drink? Drink { get; set; }
    }
}
