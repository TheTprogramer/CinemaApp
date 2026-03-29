using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaApp.Data.Models
{
    /// <summary>
    /// Представлява филм.
    /// </summary>
    [Table("films")]
    public class Film
    {
        /// <summary>Уникален идентификатор на филма.</summary>
        [Key]
        [Column("film_id")]
        public int FilmId { get; set; }

        /// <summary>Заглавие на филма.</summary>
        [Required]
        [MaxLength(40)]
        [Column("film_name")]
        public string FilmName { get; set; } = string.Empty;

        /// <summary>Жанр на филма.</summary>
        [Required]
        [MaxLength(20)]
        [Column("film_genre")]
        public string FilmGenre { get; set; } = string.Empty;

        /// <summary>Идентификатор на актьора.</summary>
        [Column("actor_id")]
        public int? ActorId { get; set; }

        /// <summary>Времетраене в минути.</summary>
        [Required]
        [Column("film_time")]
        public double FilmTime { get; set; }

        /// <summary>Навигационно свойство към актьор.</summary>
        [ForeignKey(nameof(ActorId))]
        public Actor? Actor { get; set; }

        /// <summary>Прожекции на филма.</summary>
        public ICollection<Project> Projects { get; set; } = new List<Project>();
    }
}
