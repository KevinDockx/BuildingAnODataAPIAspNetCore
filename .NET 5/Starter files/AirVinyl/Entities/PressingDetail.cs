using System.ComponentModel.DataAnnotations;

namespace AirVinyl.Entities
{
    public class PressingDetail
    {
        [Key] 
        public int PressingDetailId { get; set; }
 
        [Required]
        public int Grams { get; set; }

        [Required]
        public int Inches { get; set; }

        [Required]
        [StringLength(200)]
        public string Description { get; set; }
    }
}
