using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaApp.Data.Models
{
    /// <summary>
    /// Представлява актьор.
    /// </summary>
    [Table("actors")]
    public class Actor
    {
        /// <summary>Уникален идентификатор на актьора.</summary>
        [Key]
        [Column("actor_id")]
        public int ActorId { get; set; }

        /// <summary>Собствено име.</summary>
        [Required]
        [MaxLength(30)]
        [Column("first_name")]
        public string FirstName { get; set; } = string.Empty;

        /// <summary>Фамилно име.</summary>
        [Required]
        [MaxLength(30)]
        [Column("last_name")]
        public string LastName { get; set; } = string.Empty;

        /// <summary>Възраст.</summary>
        [Required]
        [Column("age")]
        public int Age { get; set; }

        /// <summary>Филми, в които участва актьорът.</summary>
        public ICollection<Film> Films { get; set; } = new List<Film>();
    }
}
