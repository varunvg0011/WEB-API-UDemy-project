using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Villa_WebAPI.Data;
using Villa_WebAPI.Models;
using Villa_WebAPI.Models.DTO;

namespace Villa_WebAPI.Controllers
{
    [Route("api/VillaAPI")]
    [ApiController]
    public class VillaAPIController : ControllerBase
    {
        //Why use ActionResult?
        //ActionResult is defined from the interface IActionResult
        //and helps us to return any type of datatype and also the status code
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(VillaDTO))]
        public ActionResult<IEnumerable<VillaDTO>> GetVillas()
        {
            return Ok(VillaStore.villaList);
        }


        [HttpGet("{id:int}")]
        //[ProducesResponseType(200, Type=typeof(VillaDTO))]
        //[ProducesResponseType(400)]
        //[ProducesResponseType(404)]
        //more understandable way:
        [ProducesResponseType(StatusCodes.Status200OK, Type=typeof(VillaDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult GetVilla(int id)
        {

            if (id == 0)
            {
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
        public ActionResult<VillaDTO> CreateVilla([FromBody]VillaDTO villaDTO)
        {
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
            
        }
    }
}
