using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaApp.Data.Models
{
    /// <summary>
    /// Представлява прожекция на филм в зала.
    /// </summary>
    [Table("projects")]
    public class Project
    {
        /// <summary>Уникален идентификатор на прожекцията.</summary>
        [Key]
        [Column("project_id")]
        public int ProjectId { get; set; }

        /// <summary>Ден от седмицата.</summary>
        [Required]
        [MaxLength(10)]
        [Column("day_of_week")]
        public string DayOfWeek { get; set; } = string.Empty;

        /// <summary>Начален час.</summary>
        [Required]
        [MaxLength(7)]
        [Column("start_time")]
        public string StartTime { get; set; } = string.Empty;

        /// <summary>Краен час.</summary>
        [Required]
        [MaxLength(7)]
        [Column("end_time")]
        public string EndTime { get; set; } = string.Empty;

        /// <summary>Идентификатор на залата.</summary>
        [Column("room_id")]
        public int? RoomId { get; set; }

        /// <summary>Идентификатор на филма.</summary>
        [Column("film_id")]
        public int? FilmId { get; set; }

        /// <summary>Навигационно свойство към зала.</summary>
        [ForeignKey(nameof(RoomId))]
        public Room? Room { get; set; }

        /// <summary>Навигационно свойство към филм.</summary>
        [ForeignKey(nameof(FilmId))]
        public Film? Film { get; set; }
    }
}
