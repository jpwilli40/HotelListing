using AutoMapper;
using HotelListing.DTOModels;
using HotelListing.IRepository;
using HotelListing.Controllers.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CountryController> _logger;
        private readonly IMapper _mapper;


        public CountryController(IUnitOfWork unitOfWork, ILogger<CountryController> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCountries()
        {
            try
            {
                var countries = await _unitOfWork.Countries.GetAll();
                var results = _mapper.Map<IList<CountryDTO>>(countries);  //maps country objects to country DTOs.  By doing this we can manipulate the CountryDTO objects before displaying to user without modifying the actual Country object
                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(GetCountries)}");
                return StatusCode(500, "Internal Server Error. Please Try Again Later.");
            }
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCountry(int id) //prameter should match same name as parameter in HttpGet annotation
        {
            try
            {
                var country = await _unitOfWork.Countries.Get(q => q.CountryId == id, include: q => q.Include(x => x.Hotels));  //gets the country by country id where the parameter of this method matches the q.CountryId from the list of Countries, also lists out all of the hotels associated with that country
                var result = _mapper.Map<CountryDTO>(country);  //maps country objects to country DTOs.  By doing this we can manipulate the CountryDTO objects before displaying to user without modifying the actual Country object
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(GetCountry)}");
                return StatusCode(500, "Internal Server Error. Please Try Again Later .");
            }
        }
    }
}
