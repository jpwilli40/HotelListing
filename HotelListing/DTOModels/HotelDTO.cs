using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HotelListing.DTOModels
{
    public class CreateHotelDTO
    {
        [Required]
        [StringLength(maximumLength: 20, ErrorMessage = "Hotel Name is too long")]
        public string Name { get; set; }
        
        [Required]
        [StringLength(maximumLength: 20, ErrorMessage = "Address Name is too long")]
        public string Address { get; set; }

        [Required]
        [Range(1, 5)]
        public double Rating { get; set; }

        [Required]
        public int CountryId { get; set; }
    }

    public class HotelDTO : CreateHotelDTO
    {
        public int HotelId { get; set; }
        public CountryDTO Country { get; set; }
    }
}
