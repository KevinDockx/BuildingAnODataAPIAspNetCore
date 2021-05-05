//using AirVinyl.API.DbContexts;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.OData.Routing.Controllers;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace AirVinyl.Controllers
//{
//    [Route("odata")]
//    public class VinylRecordsController : ODataController
//    {
//        private readonly AirVinylDbContext _airVinylDbContext;

//        public VinylRecordsController(AirVinylDbContext airVinylDbContext)
//        {
//            _airVinylDbContext = airVinylDbContext
//                ?? throw new ArgumentNullException(nameof(airVinylDbContext));
//        }

//        [HttpGet("VinylRecords")]
//        // Older version
//        //[HttpGet]
//        //[ODataRoute("VinylRecords")]
//        public async Task<IActionResult> GetAllVinylRecords()
//        {
//            return Ok(await _airVinylDbContext.VinylRecords.ToListAsync());
//        }

//        [HttpGet("VinylRecords({key})")]
//        public async Task<IActionResult> GetOneVinylRecord(int key)
//        {
//            var vinylRecord = await _airVinylDbContext.VinylRecords
//                .FirstOrDefaultAsync(p => p.VinylRecordId == key);

//            if (vinylRecord == null)
//            {
//                return NotFound();
//            }

//            return Ok(vinylRecord);
//        }

//    }
//}
