using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaApp.Data.Models
{
    /// <summary>
    /// Представлява храна от менюто.
    /// </summary>
    [Table("foods")]
    public class Food
    {
        /// <summary>Уникален идентификатор на храната.</summary>
        [Key]
        [Column("food_id")]
        public int FoodId { get; set; }

        /// <summary>Наименование на храната.</summary>
        [Required]
        [MaxLength(30)]
        [Column("food_title")]
        public string FoodTitle { get; set; } = string.Empty;

        /// <summary>Идентификатор на типа (размер).</summary>
        [Column("type_id")]
        public int? TypeId { get; set; }

        /// <summary>Навигационно свойство към тип.</summary>
        [ForeignKey(nameof(TypeId))]
        public TypeDrinkFood? Type { get; set; }

        /// <summary>Клиенти, поръчали тази храна.</summary>
        public ICollection<Client> Clients { get; set; } = new List<Client>();
    }
}
