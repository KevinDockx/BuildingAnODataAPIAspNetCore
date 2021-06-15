using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AirVinyl.API.DbContexts;
using AirVinyl.API.Helpers;
using AirVinyl.Entities;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AirVinyl.API.Controllers
{
    public class PeopleController : ODataController
    {
        private readonly AirVinylDbContext _airVinylDbContext;

        public PeopleController(AirVinylDbContext airVinylDbContext)
        {
            _airVinylDbContext = airVinylDbContext
                ?? throw new ArgumentNullException(nameof(airVinylDbContext));
        }

        [EnableQuery(MaxExpansionDepth = 3, MaxSkip = 10, MaxTop = 5, PageSize = 4)]
        public IActionResult Get()
        {
            return Ok(_airVinylDbContext.People);
        }


        [EnableQuery]
        public IActionResult Get(int key)
        {
            var people = _airVinylDbContext.People.Where(p => p.PersonId == key);

            if (!people.Any())
            {
                return NotFound();
            }

            return Ok(SingleResult.Create(people));
        }


        [HttpPost]
        [ODataRoute("People")]
        public IActionResult CreatePerson([FromBody] Person person)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // add the person to the People collection
            _airVinylDbContext.People.Add(person);
            _airVinylDbContext.SaveChanges();

            // return the created person 
            return Created(person);
        }

        [HttpPut]
        [ODataRoute("People({key})")]
        public IActionResult UpdatePerson(int key, [FromBody] Person person)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var currentPerson = _airVinylDbContext.People
                .FirstOrDefault(p => p.PersonId == key);

            if (currentPerson == null)
            {
                return NotFound();

                // Alternative: if the person isn't found: Upsert.  This must only
                // be used if the responsibility for creating the key isn't at 
                // server-level.  In our case, we're using auto-increment fields,
                // so this isn't allowed - code is for illustration purposes only!
                //if (currentPerson == null)
                //{
                //    // the key from the URI is the key we should use
                //    person.PersonId = key;
                //    _airVinylDbContext.People.Add(person);
                //    _airVinylDbContext.SaveChanges();
                //    return Created(person);
                //}

            }

            person.PersonId = currentPerson.PersonId;
            _airVinylDbContext.Entry(currentPerson).CurrentValues.SetValues(person);
            _airVinylDbContext.SaveChanges();

            return NoContent();
        }

        [HttpPatch]
        [ODataRoute("People({key})")]
        public IActionResult PartiallyUpdatePerson(int key, [FromBody] Delta<Person> patch)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var currentPerson = _airVinylDbContext.People
                .FirstOrDefault(p => p.PersonId == key);

            if (currentPerson == null)
            {
                return NotFound();
            }

            patch.Patch(currentPerson);
            _airVinylDbContext.SaveChanges();
            return NoContent();
        }

        [HttpDelete]
        [ODataRoute("People({key})")]
        public IActionResult DeletePerson(int key)
        {
            var currentPerson = _airVinylDbContext.People
                .FirstOrDefault(p => p.PersonId == key);

            if (currentPerson == null)
            {
                return NotFound();
            }

            _airVinylDbContext.People.Remove(currentPerson);
            _airVinylDbContext.SaveChanges();
            return NoContent();
        }


        [HttpGet]
        [ODataRoute("People({key})/Email")]
        [ODataRoute("People({key})/FirstName")]
        [ODataRoute("People({key})/LastName")]
        [ODataRoute("People({key})/DateOfBirth")]
        [ODataRoute("People({key})/Gender")]
        public IActionResult GetPersonProperty(int key)
        {
            var person = _airVinylDbContext.People
                .FirstOrDefault(p => p.PersonId == key);

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
        [ODataRoute("People({key})/VinylRecords")]
        [EnableQuery]
        public IActionResult GetVinylRecordsForPerson(int key)
        {
            var person = _airVinylDbContext.People.FirstOrDefault(p => p.PersonId == key);
            if (person == null)
            {
                return NotFound();
            }

            return Ok(_airVinylDbContext.VinylRecords.Include("DynamicVinylRecordProperties")
                .Where(v => v.Person.PersonId == key));
        }


        [HttpGet]
        [ODataRoute("People({key})/VinylRecords({vinylRecordKey})")]
        [EnableQuery]
        public IActionResult GetVinylRecordForPerson(int key, int vinylRecordKey)
        {
            var person = _airVinylDbContext.People.FirstOrDefault(p => p.PersonId == key);
            if (person == null)
            {
                return NotFound();
            }

            var vinylRecord = _airVinylDbContext.VinylRecords.Include("DynamicVinylRecordProperties")
                .Where(v => v.Person.PersonId == key
                && v.VinylRecordId == vinylRecordKey);

            if (!vinylRecord.Any())
            {
                return NotFound();
            }

            return Ok(SingleResult.Create(vinylRecord));
        }

        [HttpGet]
        [ODataRoute("People({key})/Email/$value")]
        [ODataRoute("People({key})/FirstName/$value")]
        [ODataRoute("People({key})/LastName/$value")]
        [ODataRoute("People({key})/DateOfBirth/$value")]
        [ODataRoute("People({key})/Gender/$value")]
        public IActionResult GetPersonPropertyRawValue(int key)
        {
            var person = _airVinylDbContext.People
              .FirstOrDefault(p => p.PersonId == key);

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

        [HttpPost]
        [ODataRoute("People({key})/VinylRecords")]
        public async Task<IActionResult> CreateVinylRecordForPerson(int key,
            [FromBody] VinylRecord vinylRecord)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // does the person exist?
            var person = await _airVinylDbContext.People.FirstOrDefaultAsync(p => p.PersonId == key);
            if (person == null)
            {
                return NotFound();
            }

            // link the person to the VinylRecord (also avoids an invalid person 
            // key on the passed-in record - key from the URI wins)
            vinylRecord.Person = person;

            // add the VinylRecord
            _airVinylDbContext.VinylRecords.Add(vinylRecord);
            await _airVinylDbContext.SaveChangesAsync();

            // return the created VinylRecord 
            return Created(vinylRecord);
        }

        [HttpPatch]
        [ODataRoute("People({key})/VinylRecords({vinylRecordKey})")]
        public async Task<IActionResult> PartiallyUpdateVinylRecordForPerson(int key,
            int vinylRecordKey,
            [FromBody] Delta<VinylRecord> patch)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // does the person exist?
            var person = await _airVinylDbContext.People.FirstOrDefaultAsync(p => p.PersonId == key);
            if (person == null)
            {
                return NotFound();
            }

            // find a matching vinyl record  
            var currentVinylRecord = await _airVinylDbContext.VinylRecords.Include("DynamicVinylRecordProperties")
                .FirstOrDefaultAsync(p => p.VinylRecordId == vinylRecordKey && p.Person.PersonId == key);

            // return NotFound if the VinylRecord isn't found
            if (currentVinylRecord == null)
            {
                return NotFound();
            }

            // apply patch
            patch.Patch(currentVinylRecord);
            await _airVinylDbContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete]
        [ODataRoute("People({key})/VinylRecords({vinylRecordKey})")]
        public async Task<IActionResult> DeleteVinylRecordForPerson(int key,
          int vinylRecordKey)
        {
            var currentPerson = await _airVinylDbContext.People.FirstOrDefaultAsync(p => p.PersonId == key);
            if (currentPerson == null)
            {
                return NotFound();
            }

            // find a matching vinyl record  
            var currentVinylRecord = await _airVinylDbContext.VinylRecords
                .FirstOrDefaultAsync(p => p.VinylRecordId == vinylRecordKey && p.Person.PersonId == key);

            if (currentVinylRecord == null)
            {
                return NotFound();
            }

            _airVinylDbContext.VinylRecords.Remove(currentVinylRecord);
            await _airVinylDbContext.SaveChangesAsync();

            // return No Content
            return NoContent();
        }
    }
}
