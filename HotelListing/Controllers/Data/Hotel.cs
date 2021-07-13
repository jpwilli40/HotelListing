using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HotelListing.Controllers.Data
{
    public class Hotel
    {
        public int HotelId { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public double Rating { get; set; }

        [ForeignKey(nameof(CountryKey))]  //key to Country table
        public int CountryId { get; set; }  //hard reference to entry in Country table
        public Country CountryKey { get; set; }  //Allows to gather all details for this instance to avoid messy joins in the future
        
    }
}
