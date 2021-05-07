//using AirVinyl.API.DbContexts;
//using Microsoft.AspNet.OData;
//using Microsoft.AspNet.OData.Routing;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace AirVinyl.API.Controllers
//{
//    public class VinylRecordsController : ODataController
//    {
//        private readonly AirVinylDbContext _airVinylDbContext;

//        public VinylRecordsController(AirVinylDbContext airVinylDbContext)
//        {
//            _airVinylDbContext = airVinylDbContext 
//                ?? throw new ArgumentNullException(nameof(airVinylDbContext));
//        }

//        [HttpGet]
//        [ODataRoute("VinylRecords")]
//        public IActionResult GetAllVinylRecords()
//        {
//            return Ok(_airVinylDbContext.VinylRecords);
//        }

//        [HttpGet]
//        [ODataRoute("VinylRecords({key})")]
//        public IActionResult GetOneVinylRecord(int key)
//        {
//            var vinylRecord = _airVinylDbContext.VinylRecords
//                .FirstOrDefault(p => p.VinylRecordId == key);

//            if (vinylRecord == null)
//            {
//                return NotFound();
//            }

//            return Ok(vinylRecord);
//        }


//    }
//}
