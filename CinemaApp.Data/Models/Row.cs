using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaApp.Data.Models
{
    /// <summary>
    /// Представлява ред в зала.
    /// </summary>
    [Table("rowss")]
    public class Row
    {
        /// <summary>Уникален идентификатор на реда.</summary>
        [Key]
        [Column("rows_id")]
        public int RowsId { get; set; }

        /// <summary>Тип на реда (normal / VIP).</summary>
        [Required]
        [MaxLength(30)]
        [Column("rows_type")]
        public string RowsType { get; set; } = string.Empty;

        /// <summary>Места в реда.</summary>
        public ICollection<Place> Places { get; set; } = new List<Place>();
    }
}
