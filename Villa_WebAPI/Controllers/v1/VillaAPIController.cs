using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using Villa_WebAPI.Data;
using Villa_WebAPI.Logging;
using Villa_WebAPI.Models;
using Villa_WebAPI.Models.DTO;
using Villa_WebAPI.Repository;

namespace Villa_WebAPI.Controllers
{
    [Route("api/v{version:apiVersion}/VillaAPI")]
    [ApiVersion("1.0")]
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
        protected APIResponse _apiResponse;
        public VillaAPIController( /*ApplicationDbContext db*/ IVillaRepository dbVilla, IMapper mapper)
        {
            //_db = db;
            _dbVilla = dbVilla;
            _mapper = mapper;
            _apiResponse = new();
        }

        //Why use ActionResult?
        //ActionResult is defined from the interface IActionResult
        //and helps us to return any type of datatype and also the status code
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(VillaDTO))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]       
        //Cache the response for 30 seconds. This way if we have 50 requests in 1 minutes, it will
        //fetch only 2 requests from backend, rest it will fetch from the cache area
        //[ResponseCache(Duration = 30)]
        
        //Add the cache profiler which we added in program.cs
        [ResponseCache(CacheProfileName ="Default30")]
        //incase we just want to fetch the occupancy of the villas, then we use filter and parameter in this
        public async Task<ActionResult<APIResponse>> GetVillas([FromQuery(Name = "filterOccupancy")]int? occupancy,
            [FromQuery] string? search) //changing the return type here from IEnumerable to 

        //public async Task<ActionResult<IEnumerable<VillaDTO>>> GetVillas()
        {
            //default logger
            //_logger.LogInformation("Getting all Villas");

            //serilog
            //_logger.Log("Getting all Villas", "");
            try
            {
                IEnumerable<Villa> allVillas;
                if (occupancy > 0)
                {
                    allVillas = await _dbVilla.GetAllAsync(u=>u.Occupancy == occupancy);
                }
                else
                {
                    allVillas = await _dbVilla.GetAllAsync();
                }

                if (!string.IsNullOrEmpty(search))
                {
                    allVillas = allVillas.Where(u => /*u.Amenity.ToLower().Contains(search) ||*/ u.Name.ToLower().Contains(search));
                }
                
                _apiResponse.Response = _mapper.Map<List<VillaDTO>>(allVillas);
                _apiResponse.StatusCode = HttpStatusCode.OK;
                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.ErrorMessages = new List<string> { ex.Message };
            }
            return _apiResponse;
        }


        [HttpGet("{id:int}",Name ="GetVilla")]
        //[ProducesResponseType(200, Type=typeof(VillaDTO))]
        //[ProducesResponseType(400)]
        //[ProducesResponseType(404)]
        //more understandable way:
        /*Also, we are sending typeOf here as we have only defined
         ActionResult in the method return type instead of 
        ActionResult<T>*/
        
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(VillaDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //to specify that request cannot be cached. no store and no cache both means it will fetch data
        //from daa
        //[ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<ActionResult<APIResponse>> GetVilla(int id)
        //public async Task<ActionResult<VillaDTO>> GetVilla(int id)
        {
            try
            {
                if (id == 0)
                {
                    //default logger
                    //_logger.LogError("Get Villa Error with id" + id);

                    //serilog
                    //_logger.Log("Get Villa Error with id " + id, "error");
                    _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                    _apiResponse.IsSuccess = false;
                    return BadRequest(_apiResponse);
                }
                var villa = await _dbVilla.GetAsync(i => i.id == id);
                if (villa == null)
                {
                    _apiResponse.StatusCode = HttpStatusCode.NotFound;
                    _apiResponse.IsSuccess = false;
                    return NotFound(_apiResponse);
                }

                _apiResponse.Response = _mapper.Map<VillaDTO>(villa);
                _apiResponse.StatusCode = HttpStatusCode.OK;
                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.ErrorMessages = new List<string> { ex.Message };
            }
            return _apiResponse;

        }



        [HttpPost]
        [Authorize(Roles = "developer")]
        [ProducesResponseType(StatusCodes.Status201Created/*, Type=typeof(VillaCreateDTO)*/)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CreateVilla([FromBody]VillaCreateDTO createVillaDTO)
        {
            try
            {
                //if (!ModelState.IsValid)
                //{
                //    return BadRequest(ModelState);
                //}

                if (await _dbVilla.GetAsync(u => u.Name.ToLower() == createVillaDTO.Name.ToLower()) != null)
                {
                    //ModelState.AddModelError("ErrorMessages", "Villa already Exists!");
                    //return BadRequest(ModelState);
                    _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_apiResponse);
                }

                //sending null object not allowed
                if (createVillaDTO == null)
                {
                    _apiResponse.StatusCode = HttpStatusCode.BadRequest;                    
                    return BadRequest(_apiResponse);
                }
                //if > 0 that means it is not a create request
                //commenting below id after changing villaDTO obj to villaCreateDTO as now, providing id is not needed.
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

                Villa villa = _mapper.Map<Villa>(createVillaDTO);
                await _dbVilla.CreateAsync(villa);

                _apiResponse.Response = _mapper.Map<VillaDTO>(villa);
                _apiResponse.StatusCode = HttpStatusCode.Created;



                //return Ok(villaDTO);
                return CreatedAtRoute("GetVilla", new { id = villa.id }, _apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.ErrorMessages = new List<string> { ex.Message };
            }
            return _apiResponse;
        }




        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Authorize(Roles = "developer")]
        public async Task<ActionResult<APIResponse>> DeleteVilla(int id)
        {
            try
            {
                if (id == 0)
                {
                    _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_apiResponse);
                }
                var villa = await _dbVilla.GetAsync(u => u.id == id);
                if (villa == null)
                {
                    _apiResponse.StatusCode = HttpStatusCode.NotFound;                    
                    return NotFound(_apiResponse);
                }

                await _dbVilla.RemoveAsync(villa);


                _apiResponse.StatusCode = HttpStatusCode.NoContent;
                _apiResponse.IsSuccess = true;
                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.ErrorMessages = new List<string> { ex.Message };
            }
            return _apiResponse;
        }


        [HttpPut("{id:int}", Name = "UpdateVilla")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Roles = "developer")]
        public async Task<ActionResult<APIResponse>> UpdateVilla(int id, [FromBody] VillaUpdateDTO villaUpdateDTO)
        {
            try
            {
                if (villaUpdateDTO == null || id != villaUpdateDTO.id)
                {
                    _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_apiResponse);
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


                Villa villa = _mapper.Map<Villa>(villaUpdateDTO);

                await _dbVilla.UpdateAsync(villa);

                _apiResponse.StatusCode = HttpStatusCode.NoContent;
                _apiResponse.Response = _mapper.Map<VillaDTO>(villa);
                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.ErrorMessages = new List<string> { ex.Message };
            }
            return _apiResponse;


            //return new JsonResult(new {Success = true , message = "Villa details updated" });
        }



        [HttpPatch("{id:int}", Name = "UpdatePartialVilla")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> UpdatePartialVilla(int id, JsonPatchDocument<VillaUpdateDTO> patchDTO)
        {
            try
            {
                if (patchDTO == null || id == 0)
                {
                    _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_apiResponse);
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
                    _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_apiResponse);
                }
                /*we are applying the changes we did in patchVilla object to villa
                 and then if there is any error, we are storing it in the model state.*/
                patchDTO.ApplyTo(villaDTO, ModelState);

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

                _apiResponse.StatusCode = HttpStatusCode.OK;
                _apiResponse.Response = _mapper.Map<VillaDTO>(model);

                return Ok(_apiResponse);
            }

            catch (Exception ex)
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.ErrorMessages = new List<string> { ex.Message };
            }
            return _apiResponse;

            //return new JsonResult(new { Success = true, message = "Villa details updated" });
        }
    }
}
