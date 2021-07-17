using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelListing.DTOModels
{
    public class RequestParams
    {
        const int maxPageSize = 50;  //limit
        public int PageNumber { get; set; } = 1; //defaulting to 1
        private int _pageSize = 10;  //defaulting to 10

        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = (value > maxPageSize) ? maxPageSize : value;
            }
        }
    }
}
