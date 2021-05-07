using AirVinyl.API.DbContexts;
using AirVinyl.API.Helpers;
using AirVinyl.Entities;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AirVinyl.API.Controllers
{
    public class SingletonController : ODataController
    {
        private readonly AirVinylDbContext _airVinylDbContext;

        public SingletonController(AirVinylDbContext airVinylDbContext)
        {
            _airVinylDbContext = airVinylDbContext
                ?? throw new ArgumentNullException(nameof(airVinylDbContext));
        }

        [HttpGet]
        [ODataRoute("Tim")]
        public async Task<IActionResult> GetSingletonTim()
        {
            // find Tim - he's got id 6
            var personTim = await _airVinylDbContext.People.FirstOrDefaultAsync(p => p.PersonId == 5);
            return Ok(personTim);
        }

        [HttpGet]
        [ODataRoute("Tim/Email")]
        [ODataRoute("Tim/FirstName")]
        [ODataRoute("Tim/LastName")]
        [ODataRoute("Tim/DateOfBirth")]
        [ODataRoute("Tim/Gender")]
        public async Task<IActionResult> GetPropertyOfTim()
        {
            // find Tim
            var person = await _airVinylDbContext.People.FirstOrDefaultAsync(p => p.PersonId == 5);
            if (person == null)
            {
                return NotFound();
            }

            var propertyToGet = new Uri(HttpContext.Request.GetEncodedUrl()).Segments.Last();
            if (!person.HasProperty(propertyToGet))
            {
                return NotFound();
            }

            var propertyValue = person.GetValue(propertyToGet);

            if (propertyValue == null)
            {
                // null = no content
                return NoContent();
            }

            return Ok(propertyValue);
        }
         
        [HttpGet]
        [ODataRoute("Tim/Email/$value")]
        [ODataRoute("Tim/FirstName/$value")]
        [ODataRoute("Tim/LastName/$value")]
        [ODataRoute("Tim/DateOfBirth/$value")]
        [ODataRoute("Tim/Gender/$value")]
        public async Task<IActionResult> GetRawPropertyOfTim()
        {
            var person = await _airVinylDbContext.People.FirstOrDefaultAsync(p => p.PersonId == 5);

            if (person == null)
            {
                return NotFound();
            }

            var url = HttpContext.Request.GetEncodedUrl();
            var propertyToGet = new Uri(url).Segments[^2].TrimEnd('/');

            if (!person.HasProperty(propertyToGet))
            {
                return NotFound();
            }

            var propertyValue = person.GetValue(propertyToGet);

            if (propertyValue == null)
            {
                // null = no content
                return NoContent();
            }

            return Ok(propertyValue.ToString());
        }

        [HttpGet]
        [ODataRoute("Tim/VinylRecords")] 
        public async Task<IActionResult> GetVinylRecordsForTim()
        {
            var person = await _airVinylDbContext.People.FirstOrDefaultAsync(p => p.PersonId == 5);
            if (person == null)
            {
                return NotFound();
            }

            return Ok(_airVinylDbContext.VinylRecords.Where(v => v.Person.PersonId == 5));
        }

        [HttpPatch]
        [ODataRoute("Tim")]
        public async Task<IActionResult> PartiallyUpdateTim([FromBody] Delta<Person> patch)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // find Tim
            var currentPerson = await _airVinylDbContext.People.FirstOrDefaultAsync(p => p.PersonId == 5);

            // apply the patch, and save the changes
            patch.Patch(currentPerson);
            await _airVinylDbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
