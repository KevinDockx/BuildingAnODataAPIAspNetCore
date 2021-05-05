using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AirVinyl.Entities
{
    public class Person
    {
        [Key]
        public int PersonId { get; set; }
        
        [StringLength(100)]    
        public string Email { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTimeOffset DateOfBirth { get; set; }

        [Required]
        public Gender Gender { get; set; }

        public int NumberOfRecordsOnWishList { get; set; }

        public decimal AmountOfCashToSpend { get; set; }

        public ICollection<VinylRecord> VinylRecords { get; set; } = new List<VinylRecord>();
    }
}
