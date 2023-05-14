using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Villa_WebAPI.Data;
using Villa_WebAPI.Logging;
using Villa_WebAPI.Models;
using Villa_WebAPI.Models.DTO;
using Villa_WebAPI.Repository;

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


        //rather than making the direct connection with application DB context by using below,
        //we are going to have repository pattern for it.
        //private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        private readonly IVillaRepository _dbVilla;
        public VillaAPIController( /*ApplicationDbContext db*/ IVillaRepository dbVilla, IMapper mapper)
        {
            //_db = db;
            _dbVilla = dbVilla;
            _mapper = mapper;
        }

        //Why use ActionResult?
        //ActionResult is defined from the interface IActionResult
        //and helps us to return any type of datatype and also the status code
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(VillaDTO))]
        public async Task<ActionResult<IEnumerable<VillaDTO>>> GetVillas()
        {
            //default logger
            //_logger.LogInformation("Getting all Villas");

            //serilog
            //_logger.Log("Getting all Villas", "");
            IEnumerable<Villa> allVillas = await _dbVilla.GetAllAsync();
            return Ok(_mapper.Map<List<VillaDTO>>(allVillas));
        }


        [HttpGet("{id:int}",Name ="GetVilla")]
        //[ProducesResponseType(200, Type=typeof(VillaDTO))]
        //[ProducesResponseType(400)]
        //[ProducesResponseType(404)]
        //more understandable way:
        /*Also, we are sending typeOf here as we have only defined
         ActionResult in the method return type instead of 
        ActionResult<T>*/
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(VillaDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<VillaDTO>> GetVilla(int id)
        {
            if (id == 0)
            {
                //default logger
                //_logger.LogError("Get Villa Error with id" + id);

                //serilog
                //_logger.Log("Get Villa Error with id " + id, "error");
                return BadRequest();
            }
            var villa = await _dbVilla.GetAsync(i => i.id == id);
            if(villa == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<VillaDTO>(villa));
        }



        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created/*, Type=typeof(VillaCreateDTO)*/)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<VillaDTO>> CreateVilla([FromBody]VillaCreateDTO createVillaDTO)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            if(await _dbVilla.GetAsync(u=>u.Name.ToLower() == createVillaDTO.Name.ToLower()) != null)
            {
                ModelState.AddModelError("CustomError", "Villa already Exists!");
                return BadRequest(ModelState);
            }

            //sending null object not allowed
            if(createVillaDTO == null)
            {
                return BadRequest(createVillaDTO);
            }
            //if > 0 that means it is not a create request
            //commenting below id after changing villaDTO obj to villaCreateDTO as it is not needed in this case
            //if (villaDTO.id > 0)
            //{
            //    //we are returning a custom status code that is not
            //    //one of our pre-defined 

            //    return StatusCode(StatusCodes.Status500InternalServerError);
            //}

            //converting villaDTO to villa type as we are passing villa type 
            //object to the DB side
            //not needed anymore in case of automapper
            //Villa model = new()
            //{
            //    Amenity = createVillaDTO.Amenity,
            //    Details = createVillaDTO.Details,                
            //    ImageUrl = createVillaDTO.ImageUrl,
            //    Name = createVillaDTO.Name,
            //    Occupancy = createVillaDTO.Occupancy,
            //    Rate = createVillaDTO.Rate,
            //    Sqft = createVillaDTO.Sqft,
            //};

            Villa model = _mapper.Map<Villa>(createVillaDTO);


            await _dbVilla.CreateAsync(model);
            

            
            //return Ok(villaDTO);
            return CreatedAtRoute("GetVilla", new {id = model.id}, model);
        }




        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteVilla(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var villa = await _dbVilla.GetAsync(u=>u.id == id);
            if(villa == null)
            {
                return NotFound();
            }
            
            await _dbVilla.RemoveAsync(villa);
            
            return new JsonResult(new  {Success= true, message= "Villa has been deleted!"});
        }


        [HttpPut("{id:int}", Name = "UpdateVilla")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        
        public async Task<IActionResult> UpdateVilla(int id, [FromBody] VillaUpdateDTO villaUpdateDTO)
        {
            if(villaUpdateDTO == null || id!= villaUpdateDTO.id)
            {
                return BadRequest();
            }
            //var villa = VillaStore.villaList.FirstOrDefault(i => i.id==id);
            //villa.Name = villaDTO.Name;
            //villa.Occupancy = villaDTO.Occupancy;
            //villa.Sqft = villaDTO.Sqft;




            //adding below and commenting above for EF core
            //not needed anymore in case of automapper
            //Villa model = new()
            //{
            //    Amenity = villaUpdateDTO.Amenity,
            //    Details = villaUpdateDTO.Details,
            //    id = villaUpdateDTO.id,
            //    ImageUrl = villaUpdateDTO.ImageUrl,
            //    Name = villaUpdateDTO.Name,
            //    Occupancy = villaUpdateDTO.Occupancy,
            //    Rate = villaUpdateDTO.Rate,

            //    Sqft = villaUpdateDTO.Sqft,
            //};


            Villa model = _mapper.Map<Villa>(villaUpdateDTO);

            await _dbVilla.UpdateAsync(model);

            return new JsonResult(new {Success = true , message = "Villa details updated" });
        }



        [HttpPatch("{id:int}", Name = "UpdatePartialVilla")]
        public async Task<IActionResult> UpdatePartialVilla(int id, JsonPatchDocument<VillaUpdateDTO> patchDTO)
        {
            if (patchDTO == null || id == 0)
            {
                return BadRequest();
            }

            //we are adding the AsNoTracking here we dont want the EF to track the
            //villa with mentioned ID in this statement as we are updating it using
            //update method below and that one also has the villa model with id 6.
            //EF doesn't allow 1 model being tracked twice.
            var villa = await _dbVilla.GetAsync(i => i.id == id, tracked: false);

            //Firsst we are changing villa to VillaUpdateDTO so that we can pass
            //to applyTo function below in the form of VillaUpdateDTO
            //VillaUpdateDTO villaDTO = new()
            //{
            //    Amenity = villa.Amenity,
            //    Details = villa.Details,
            //    id = villa.id,
            //    ImageUrl = villa.ImageUrl,
            //    Name = villa.Name,
            //    Occupancy = villa.Occupancy,
            //    Rate = villa.Rate,
            //    Sqft = villa.Sqft,
            //};


            VillaUpdateDTO villaDTO = _mapper.Map<VillaUpdateDTO>(villa);



            if (villa == null)
            {
                return BadRequest();
            }
            /*we are applying the changes we did in patchVilla object to villa
             and then if there is any error, we are storing it in the model state.*/
            patchDTO.ApplyTo(villaDTO,ModelState);

            //Villa model = new()
            //{
            //    Amenity = villaDTO.Amenity,
            //    Details = villaDTO.Details,
            //    id = villaDTO.id,
            //    ImageUrl = villaDTO.ImageUrl,
            //    Name = villaDTO.Name,
            //    Occupancy = villaDTO.Occupancy,
            //    Rate = villaDTO.Rate,
            //    Sqft = villaDTO.Sqft,
            //};


            Villa model = _mapper.Map<Villa>(villaDTO);

            
            await _dbVilla.UpdateAsync(model);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return new JsonResult(new { Success = true, message = "Villa details updated" });
        }
    }
}
