using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using nzWorks.API.Data;
using nzWorks.API.Models.Domain;
using nzWorks.API.Models.DTO;
using System.Text.RegularExpressions;

namespace nzWorks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;

        public RegionsController(NZWalksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult GetAllRegions()
        {
            var regions = dbContext.Regions.ToList();

            var regionDto = new List<RegionDTO>();
           
                foreach(var region in regions)
            {
                regionDto.Add(new RegionDTO()
                {
                    Id = region.Id,
                    Code = region.Code,
                    Name = region.Name
                });
            }
            //  var regions = new List<Region>
            //  {
            //   new Region
            //  {
            //      Id=Guid.NewGuid(),
            //      Name = "Aukland Region",
            //      Code = "AKL",
            //      RegionImageURL = "data:image/jpeg;base64,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"
            //  },
            //    new Region
            //  {
            //      Id=Guid.NewGuid(),
            //      Name = "Wellington Region",
            //      Code = "WLG",
            //      RegionImageURL = "data:image/jpeg;base64,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"
            //  },
            //};
            return Ok(regionDto);


        }
        //Get single Region
        //get:https://localhost:portNumber/api/regions/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        public IActionResult GetById([FromRoute]Guid id)
        {
            //   var region = dbContext.Regions.Find(id);
            var region = dbContext.Regions.FirstOrDefault(x=>x.Id == id);
            if (region == null) { 
                return NotFound();
            }
            else
            {
                var regionDto = new RegionDTO
                {
                    Id = region.Id,
                    Code = region.Code,
                    Name = region.Name,
                    RegionImageURL = region.RegionImageURL
                };
                return Ok(regionDto);
            }
        }

        //httpPost 
        [HttpPost]
        public IActionResult Create([FromBody] AddRegion addRegion)
        {
            //DTO to Domain model
            var regionmodel = new Region
            {
                Code = addRegion.Code,
                Name = addRegion.Name,
                RegionImageURL = addRegion.RegionImageURL
            };

            //domain model to db
            dbContext.Regions.Add(regionmodel);
            dbContext.SaveChanges();

            //to verify we will again get the saved data info

            var regionDTO = new RegionDTO
            {
                Id = regionmodel.Id,
                Code = regionmodel.Code,
                Name = regionmodel.Name,
                RegionImageURL = regionmodel.RegionImageURL
            };
            return Ok(regionDTO); //200 status 
           // return CreatedAtAction(nameof(GetById), new { id = regionmodel.Id }, regionDTO); // 201 status

        }

        //update region

        [HttpPut]
        [Route("{id:Guid}")]
        public IActionResult Update([FromRoute] Guid id, [FromBody] UpdateRegion updateRegion)
        {
            var regionDtoModel = dbContext.Regions.FirstOrDefault(x => x.Id == id);

            if(regionDtoModel == null)
            {
                return NotFound();
            }

            regionDtoModel.Code = updateRegion.Code;
            regionDtoModel.Name = updateRegion.Name;
            regionDtoModel.RegionImageURL = updateRegion.RegionImageURL;

            dbContext.SaveChanges();

            //domain model to DTO

            var updateRegionDTO = new RegionDTO
            {
                Id = regionDtoModel.Id,
                Code = regionDtoModel.Code,
                Name = regionDtoModel.Name,
                RegionImageURL = regionDtoModel.RegionImageURL
            };
            return Ok(updateRegionDTO);
        }

        //[HttpDelete]

        //[Route("{id:Guid}")]
        //public async Task<IActionResult>  DeleteRegion([FromRoute] Guid id)
        //{
        //    var regionDel = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
        //        if(regionDel == null)
        //    {
        //        return NotFound();
        //    }
        //        dbContext.Regions.Remove(regionDel);
        //      dbContext.SaveChanges();
        //    var delRegionDTO = new RegionDTO
        //    {
        //        Id = regionDel.Id,
        //        Code = regionDel.Code,
        //        Name = regionDel.Name,
        //        RegionImageURL = regionDel.RegionImageURL
        //    };

        //    return Ok(delRegionDTO);
        //}
    }
}
