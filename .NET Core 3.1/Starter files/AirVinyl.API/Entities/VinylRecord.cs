using System.ComponentModel.DataAnnotations;

namespace AirVinyl.Entities
{
    public class VinylRecord
    {
        [Key]
        public int VinylRecordId { get; set; }

        [StringLength(150)]
        [Required]
        public string Title { get; set; }

        [StringLength(150)]
        [Required]
        public string Artist { get; set; }

        [StringLength(50)]
        public string CatalogNumber { get; set; }

        public int? Year { get; set; }

        public PressingDetail PressingDetail { get; set; }

        public int PressingDetailId { get; set; }

        public virtual Person Person { get; set; }

        public int PersonId { get; set; } 
    }
}
