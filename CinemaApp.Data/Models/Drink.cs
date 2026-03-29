using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaApp.Data.Models
{
    /// <summary>
    /// Представлява напитка от менюто.
    /// </summary>
    [Table("drinks")]
    public class Drink
    {
        /// <summary>Уникален идентификатор на напитката.</summary>
        [Key]
        [Column("drink_id")]
        public int DrinkId { get; set; }

        /// <summary>Наименование на напитката.</summary>
        [Required]
        [MaxLength(20)]
        [Column("drink_title")]
        public string DrinkTitle { get; set; } = string.Empty;

        /// <summary>Идентификатор на типа (размер).</summary>
        [Column("type_id")]
        public int? TypeId { get; set; }

        /// <summary>Навигационно свойство към тип.</summary>
        [ForeignKey(nameof(TypeId))]
        public TypeDrinkFood? Type { get; set; }

        /// <summary>Клиенти, поръчали тази напитка.</summary>
        public ICollection<Client> Clients { get; set; } = new List<Client>();
    }
}
