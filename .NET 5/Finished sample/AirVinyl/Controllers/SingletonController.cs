using AirVinyl.API.DbContexts;
using AirVinyl.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks; 

namespace AirVinyl.Controllers
{
    [Route("odata")]
    public class SingletonController : ODataController
    {       
        private readonly AirVinylDbContext _airVinylDbContext;

        public SingletonController(AirVinylDbContext airVinylDbContext)
        {
            _airVinylDbContext = airVinylDbContext
                ?? throw new ArgumentNullException(nameof(airVinylDbContext));
        }

        [HttpGet("Tim")]
        public async Task<IActionResult> GetSingletonTim()
        {
            // find Tim - he's got id 5
            var personTim = await _airVinylDbContext.People
                .FirstOrDefaultAsync(p => p.PersonId == 5);
            return Ok(personTim);
        }

        [HttpPatch("Tim")]
        public async Task<IActionResult> PartiallyUpdateTim([FromBody] Delta<Person> patch)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // find Tim
            var currentPerson = await _airVinylDbContext.People
                .FirstOrDefaultAsync(p => p.PersonId == 5);

            // apply the patch, and save the changes
            patch.Patch(currentPerson);
            await _airVinylDbContext.SaveChangesAsync();

            return NoContent();
        }


    }
}
