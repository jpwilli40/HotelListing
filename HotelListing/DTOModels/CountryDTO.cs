using HotelListing.Controllers.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HotelListing.DTOModels
{
    public class CreateCountryDTO //everything that is necessary to create or update object
    {
        [Required]
        [StringLength(maximumLength: 20, ErrorMessage = "Country Name is too long")]
        public string Name { get; set; }

        [StringLength(maximumLength: 2, ErrorMessage = "Short Country Name is too long")]
        public string ShortName { get; set; }
    }


        public class CountryDTO : CreateCountryDTO //inherits from everything defined
    {
        public int CountryId { get; set; }
        public IList<HotelDTO> Hotels { get; set; }
    }
}
