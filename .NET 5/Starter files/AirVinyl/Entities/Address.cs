using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirVinyl.Entities
{
    // Address is an owned type (no key) - used to be called complex type in EF.
    [Owned]
    public class Address
    {
        [StringLength(200)]
        public string Street { get; set; }

        [StringLength(100)]
        public string City { get; set; }

        [StringLength(10)]
        public string PostalCode { get; set; }

        [StringLength(100)]
        public string Country { get; set; }

        public int RecordStoreId { get; set; }         
    }
}
