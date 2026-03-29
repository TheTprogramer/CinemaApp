using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaApp.Data.Models
{
    /// <summary>
    /// Представлява зала в киното.
    /// </summary>
    [Table("rooms")]
    public class Room
    {
        /// <summary>Уникален идентификатор на залата.</summary>
        [Key]
        [Column("room_id")]
        public int RoomId { get; set; }

        /// <summary>Тип на залата (2D, 3D, 4DX, ScreenX).</summary>
        [Required]
        [MaxLength(30)]
        [Column("room_type")]
        public string RoomType { get; set; } = string.Empty;

        /// <summary>Места в залата.</summary>
        public ICollection<Place> Places { get; set; } = new List<Place>();

        /// <summary>Прожекции в залата.</summary>
        public ICollection<Project> Projects { get; set; } = new List<Project>();
    }
}
