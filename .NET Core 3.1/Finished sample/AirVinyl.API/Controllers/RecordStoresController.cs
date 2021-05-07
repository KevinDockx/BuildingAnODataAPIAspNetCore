using AirVinyl.API.DbContexts;
using AirVinyl.API.Helpers;
using AirVinyl.Entities;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AirVinyl.API.Controllers
{
    public class RecordStoresController : ODataController
    {
        private readonly AirVinylDbContext _airVinylDbContext;

        public RecordStoresController(AirVinylDbContext airVinylDbContext)
        {
            _airVinylDbContext = airVinylDbContext
                ?? throw new ArgumentNullException(nameof(airVinylDbContext));
        }
          
        [HttpGet]
        [EnableQuery]
        [ODataRoute("RecordStores")]
        public IActionResult GetRecordStores()
        {
            return Ok(_airVinylDbContext.RecordStores);
        }
          
        [HttpGet]
        [EnableQuery]
        [ODataRoute("RecordStores({key})")]
        public IActionResult Get(int key)
        {
            var recordStores = _airVinylDbContext.RecordStores.Where(p => p.RecordStoreId == key);

            if (!recordStores.Any())
            {
                return NotFound();
            }

            return Ok(SingleResult.Create(recordStores));
        }

        [HttpGet]
        [ODataRoute("RecordStores({key})/Tags")]
        [EnableQuery]
        public IActionResult GetRecordStoreTagsProperty(int key)
        {
            // no Include necessary for EF Core - "Tags" isn't a navigation property 
            // in the entity model.  
            var recordStore = _airVinylDbContext.RecordStores
                .FirstOrDefault(p => p.RecordStoreId == key);

            if (recordStore == null)
            {
                return NotFound();
            }

            var collectionPropertyToGet = new Uri(HttpContext.Request.GetEncodedUrl()).Segments.Last();   
            var collectionPropertyValue = recordStore.GetValue(collectionPropertyToGet);

            // return the collection of tags
            return Ok(collectionPropertyValue);
        }


        [HttpGet]
        [ODataRoute("RecordStores({key})/AirVinyl.Functions.IsHighRated(minimumRating={minimumRating})")]
        public async Task<bool> IsHighRated([FromODataUri] int key, int minimumRating)
        { 
            var recordStore = await _airVinylDbContext.RecordStores
                .FirstOrDefaultAsync(p => p.RecordStoreId == key
                    && p.Ratings.Any()
                    && (p.Ratings.Sum(r => r.Value) / p.Ratings.Count) >= minimumRating);

            return (recordStore != null);
        }


        [HttpGet]
        [ODataRoute("RecordStores/AirVinyl.Functions.AreRatedBy(personIds={personIds})")]
        public async Task<IActionResult> AreRatedBy([FromODataUri] IEnumerable<int> personIds)
        { 
            var recordStores = await _airVinylDbContext.RecordStores
                .Where(p => p.Ratings.Any(r => personIds.Contains(r.RatedBy.PersonId)))
                .ToListAsync();

            return Ok(recordStores);
        }

        [HttpGet]
        [ODataRoute("GetHighRatedRecordStores(minimumRating={minimumRating})")]
        public async Task<IActionResult> GetHighRatedRecordStores([FromODataUri] int minimumRating)
        { 
            var recordStores = await _airVinylDbContext.RecordStores
                .Where(p => p.Ratings.Any()
                    && (p.Ratings.Sum(r => r.Value) / p.Ratings.Count) >= minimumRating)
                .ToListAsync();

            return Ok(recordStores);
        }

        [HttpPost]
        [ODataRoute("RecordStores({key})/AirVinyl.Actions.Rate")]
        public async Task<IActionResult> Rate(int key, ODataActionParameters parameters)
        {         
            // get the RecordStore
            var recordStore = await _airVinylDbContext.RecordStores
              .FirstOrDefaultAsync(p => p.RecordStoreId == key);

            if (recordStore == null)
            {
                return NotFound();
            }

            // from the param dictionary, get the rating & personid
        
            if (!parameters.TryGetValue("rating", out object outputFromDictionary))
            {
                return NotFound();
            }

            if (!int.TryParse(outputFromDictionary.ToString(), out int rating))
            {
                return NotFound();
            }

            if (!parameters.TryGetValue("personId", out outputFromDictionary))
            {
                return NotFound();
            }

            if (!int.TryParse(outputFromDictionary.ToString(), out int personId))
            {
                return NotFound();
            }

            // the person must exist
            var person = await _airVinylDbContext.People
            .FirstOrDefaultAsync(p => p.PersonId == personId);

            if (person == null)
            {
                return NotFound();
            }

            // everything checks out, add the rating
            recordStore.Ratings.Add(new Rating() { RatedBy = person, Value = rating });

            // save changes 
            if (await _airVinylDbContext.SaveChangesAsync() > 0)
            {
                // return true
                return Ok(true);
            }
            else
            {

                // Something went wrong - we expect our 
                // action to return false in that case.  
                // The request is still successful, false
                // is a valid response
                return Ok(false);
            }
        }

        [HttpPost]
        [ODataRoute("RecordStores/AirVinyl.Actions.RemoveRatings")]
        public async Task<IActionResult> RemoveRatings(ODataActionParameters parameters)
        { 
            // from the param dictionary, get the personid 
            if (!parameters.TryGetValue("personId", out object outputFromDictionary))
            {
                return NotFound();
            }

            if (!int.TryParse(outputFromDictionary.ToString(), out int personId))
            {
                return NotFound();
            }

            // get the RecordStores that were rated by the person with personId
            var recordStoresRatedByCurrentPerson = await _airVinylDbContext.RecordStores
                .Include("Ratings").Include("Ratings.RatedBy")
                .Where(p => p.Ratings.Any(r => r.RatedBy.PersonId == personId)).ToListAsync();

            // remove those ratings
            foreach (var store in recordStoresRatedByCurrentPerson)
            {
                // get the ratings by the current person
                var ratingsByCurrentPerson = store.Ratings
                    .Where(r => r.RatedBy.PersonId == personId).ToList();

                for (int i = 0; i < ratingsByCurrentPerson.Count(); i++)
                {
                    store.Ratings.Remove(ratingsByCurrentPerson[i]);
                }
            }

            // save changes 
            if (await _airVinylDbContext.SaveChangesAsync() > 0)
            {
                // return true
                return Ok(true);
            }
            else
            {
                // Something went wrong - we expect our 
                // action to return false in that case.  
                // The request is still successful, false
                // is a valid response
                return Ok(false);
            } 
        }


        [HttpPost]
        [ODataRoute("RemoveRecordStoreRatings")] 
        public async Task<IActionResult> RemoveRecordStoreRatings(ODataActionParameters parameters)
        {
            // from the param dictionary, get the personid 
            if (!parameters.TryGetValue("personId", out object outputFromDictionary))
            {
                return NotFound();
            }

            if (!int.TryParse(outputFromDictionary.ToString(), out int personId))
            {
                return NotFound();
            }

            // get the RecordStores that were rated by the person with personId
            var recordStoresRatedByCurrentPerson = await _airVinylDbContext.RecordStores
                .Include("Ratings").Include("Ratings.RatedBy")
                .Where(p => p.Ratings.Any(r => r.RatedBy.PersonId == personId)).ToListAsync();

            // remove those ratings
            foreach (var store in recordStoresRatedByCurrentPerson)
            {
                // get the ratings by the current person
                var ratingsByCurrentPerson = store.Ratings
                    .Where(r => r.RatedBy.PersonId == personId).ToList();

                for (int i = 0; i < ratingsByCurrentPerson.Count(); i++)
                {
                    store.Ratings.Remove(ratingsByCurrentPerson[i]);
                }
            }

            // save changes 
            if (await _airVinylDbContext.SaveChangesAsync() > 0)
            {
                return NoContent();
            }
            else
            {
                // something went wrong
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }


        [HttpGet]
        [ODataRoute("RecordStores/AirVinyl.SpecializedRecordStore")]
        [EnableQuery]
        public IActionResult GetSpecializedRecordStores()
        {
            var specializedStores = _airVinylDbContext.RecordStores 
              .Where(r => r is SpecializedRecordStore).ToList();
             
            return Ok(specializedStores.Select(s => s as SpecializedRecordStore)); 
        }

        [HttpGet]
        [EnableQuery]
        [ODataRoute("RecordStores({key})/AirVinyl.SpecializedRecordStore")]
        public IActionResult GetSpecializedRecordStore(int key)
        {
            var specializedStore = _airVinylDbContext.RecordStores.FirstOrDefault
                (r => r.RecordStoreId == key && r is SpecializedRecordStore);

            if (specializedStore == null)
            {
                return NotFound();
            }

            return Ok(specializedStore as SpecializedRecordStore); 
        }


        [HttpPost]
        [ODataRoute("RecordStores")]
        public async Task<IActionResult> CreateRecordStore([FromBody] RecordStore recordStore)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // add the RecordStore
            _airVinylDbContext.RecordStores.Add(recordStore);
            await _airVinylDbContext.SaveChangesAsync();

            // return the created RecordStore 
            return Created(recordStore);
        }

        [HttpPatch]
        [ODataRoute("RecordStores({key})")]
        [ODataRoute("RecordStores({key})/AirVinyl.SpecializedRecordStore")]
        public async Task<IActionResult> UpdateRecordStorePartially(int key, [FromBody] Delta<RecordStore> patch)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // find a matching record store
            var currentRecordStore = await _airVinylDbContext.RecordStores.FirstOrDefaultAsync(p => p.RecordStoreId == key);

            // if the record store isn't found, return NotFound
            if (currentRecordStore == null)
            {
                return NotFound();
            }

            patch.Patch(currentRecordStore);
            await _airVinylDbContext.SaveChangesAsync();
             
            return NoContent();
        }

        [HttpDelete]
        [ODataRoute("RecordStores({key})")]
        [ODataRoute("RecordStores({key})/AirVinyl.SpecializedRecordStore")]
        public async Task<IActionResult> DeleteRecordStore(int key)
        {
            var currentRecordStore = await _airVinylDbContext.RecordStores.Include("Ratings")
                .FirstOrDefaultAsync(p => p.RecordStoreId == key);

            if (currentRecordStore == null)
            {
                return NotFound();
            }

            currentRecordStore.Ratings.Clear();
            _airVinylDbContext.RecordStores.Remove(currentRecordStore);
            await _airVinylDbContext.SaveChangesAsync();

            return NoContent();
        }

    }
}
