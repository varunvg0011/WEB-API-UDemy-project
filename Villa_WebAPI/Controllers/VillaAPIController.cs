using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Villa_WebAPI.Data;
using Villa_WebAPI.Logging;
using Villa_WebAPI.Models;
using Villa_WebAPI.Models.DTO;

namespace Villa_WebAPI.Controllers
{
    [Route("api/VillaAPI")]
    //this below attribute helps to bind data annotations with model entities
    //without this, out datat annotations will not work
    [ApiController]
    public class VillaAPIController : ControllerBase
    {
        //for serilog
        //private readonly ILogging _logger;

        //for default logger
        //private readonly ILogger<VillaAPIController> _logger;

        //serilog
        //public VillaAPIController(ILogging logger)
        //{
        //    _logger = logger;
        //}

        //default logger
        //public VillaAPIController(ILogger<VillaAPIController> logger)
        //{
        //    _logger = logger;
        //}

        private readonly ApplicationDbContext _db;

        public VillaAPIController( ApplicationDbContext db)
        {

        }

        //Why use ActionResult?
        //ActionResult is defined from the interface IActionResult
        //and helps us to return any type of datatype and also the status code
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(VillaDTO))]
        public ActionResult<IEnumerable<VillaDTO>> GetVillas()
        {
            //default logger
            //_logger.LogInformation("Getting all Villas");

            //serilog
            //_logger.Log("Getting all Villas", "");
            return Ok(VillaStore.villaList);
        }


        [HttpGet("{id:int}",Name ="GetVilla")]
        //[ProducesResponseType(200, Type=typeof(VillaDTO))]
        //[ProducesResponseType(400)]
        //[ProducesResponseType(404)]
        //more understandable way:
        /*Also, we are sending typeOf here as we have only defined
         ActionResult in the method return type instead of 
        ActionResult<T>*/
        [ProducesResponseType(StatusCodes.Status200OK, Type=typeof(VillaDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult GetVilla(int id)
        {
            if (id == 0)
            {
                //default logger
                //_logger.LogError("Get Villa Error with id" + id);

                //serilog
                //_logger.Log("Get Villa Error with id " + id, "error");
                return BadRequest();
            }
            var villa = VillaStore.villaList.FirstOrDefault(i => i.id == id);
            if(villa == null)
            {
                return NotFound();
            }
            return Ok(villa);
        }



        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type=typeof(VillaDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult CreateVilla([FromBody]VillaDTO villaDTO)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            if(VillaStore.villaList.FirstOrDefault(u=>u.Name.ToLower() == villaDTO.Name.ToLower()) != null)
            {
                ModelState.AddModelError("CustomError", "Villa already Exists!");
                return BadRequest(ModelState);
            }

            //sending null object not allowed
            if(villaDTO == null)
            {
                return BadRequest(villaDTO);
            }
            //if > 0 that means it is not a create request
            if (villaDTO.id > 0)
            {
                //we are returning a custom status code that is not
                //one of our pre-defined 

                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            villaDTO.id = VillaStore.villaList.OrderByDescending(o => o.id).FirstOrDefault().id + 1;
            VillaStore.villaList.Add(villaDTO);
            //return Ok(villaDTO);
            return CreatedAtRoute("GetVilla", new {id = villaDTO.id}, villaDTO);
        }




        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteVilla(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var villa = VillaStore.villaList.FirstOrDefault(u=>u.id == id);
            if(villa == null)
            {
                return NotFound();
            }
            VillaStore.villaList.Remove(villa);
            return new JsonResult(new  {Success= true, message= "Villa has been deleted!"});
        }


        [HttpPut("{id:int}", Name = "UpdateVilla")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        
        public IActionResult UpdateVilla(int id, [FromBody] VillaDTO villaDTO)
        {
            if(villaDTO == null || id!= villaDTO.id)
            {
                return BadRequest();
            }
            var villa = VillaStore.villaList.FirstOrDefault(i => i.id==id);
            villa.Name = villaDTO.Name;
            villa.Occupancy = villaDTO.Occupancy;
            villa.Sqft = villaDTO.Sqft;
            return new JsonResult(new {Success = true , message = "Villa details updated" });
        }



        [HttpPatch("{id:int}", Name = "UpdatePartialVilla")]
        public IActionResult UpdatePartialVilla(int id, JsonPatchDocument<VillaDTO> patchVilla)
        {
            if (patchVilla == null || id == 0)
            {
                return BadRequest();
            }
            var villa = VillaStore.villaList.FirstOrDefault(i => i.id == id);
            if(villa == null)
            {
                return BadRequest();
            }
            /*we are applying the changes we did in patchVilla object to villa
             and then if there is any error, we are storing it in the model state.*/
            patchVilla.ApplyTo(villa,ModelState);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return new JsonResult(new { Success = true, message = "Villa details updated" });
        }
    }
}
