using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaApp.Data.Models
{
    /// <summary>
    /// Представлява тип (размер) на храна или напитка — Малко / Средно / Голямо.
    /// </summary>
    [Table("types_drinks_foods")]
    public class TypeDrinkFood
    {
        /// <summary>Уникален идентификатор на типа.</summary>
        [Key]
        [Column("type_id")]
        public int TypeId { get; set; }

        /// <summary>Наименование на типа (Малко, Средно, Голямо).</summary>
        [Required]
        [MaxLength(10)]
        [Column("type_title")]
        public string TypeTitle { get; set; } = string.Empty;

        /// <summary>Цена за този тип.</summary>
        [Required]
        [Column("type_price")]
        public decimal TypePrice { get; set; }

        /// <summary>Храни с този тип.</summary>
        public ICollection<Food> Foods { get; set; } = new List<Food>();

        /// <summary>Напитки с този тип.</summary>
        public ICollection<Drink> Drinks { get; set; } = new List<Drink>();
    }
}
