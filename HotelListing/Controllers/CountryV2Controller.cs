using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelListing.Controllers
{
    //another way to add versioning is in ServiceExtensions --ApiVersionReader,  This way will allow us to keep the Route the same and let the client add a header (api-version) = 2.0
    [ApiVersion("2.0", Deprecated = true)]  //if two controllers have the same route defined below, Version can be specified here.  This will need to be called 
    [Route("api/{v:apiversion}/[controller]")]  //another way to define versioning using a url like "api/2.0/[controller]
    [ApiController]
    public class CountryV2Controller : ControllerBase
    {
    }
}
