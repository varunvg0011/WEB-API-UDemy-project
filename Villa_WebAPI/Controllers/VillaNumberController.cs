using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Villa_WebAPI.Data;
using Villa_WebAPI.Models;
using Villa_WebAPI.Models.DTO;
using Villa_WebAPI.Repository;
using System.Net;
using Microsoft.AspNetCore.JsonPatch;

namespace Villa_WebAPI.Controllers
{
    [Route("api/VillaNumberAPI")]
    [ApiController]   
    public class VillaNumberController : Controller
    {

        private readonly IMapper _mapper;
        private readonly IVillaNumberRepository _dbVillaNumbers;
        private readonly IVillaRepository _dbVilla ;
        protected APIResponse _apiResponse;
        public VillaNumberController(IMapper mapper, IVillaNumberRepository dbVillaNumbers, IVillaRepository dbVilla)
        {
            _mapper = mapper;
            _dbVillaNumbers = dbVillaNumbers;
            _apiResponse = new();
            _dbVilla = dbVilla;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(APIResponse))]
        public async Task<ActionResult<APIResponse>> GetVillaNumbers()
        {
            try
            {
                IEnumerable<VillaNumber> allVillaNumbersList = await _dbVillaNumbers.GetAllAsync(includeProperties: "Villa");
                _apiResponse.Response = _mapper.Map<List<VillaNumberDTO>>(allVillaNumbersList);
                _apiResponse.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _apiResponse;
        }


        [HttpGet("{villaNo:int}", Name = "GetVillaNumber")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(APIResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> GetVillaNumber(int villaNo)
        {
            try
            {
                if (villaNo == 0)
                {
                    _apiResponse.StatusCode=HttpStatusCode.BadRequest;
                    _apiResponse.IsSuccess=false;
                    return BadRequest(_apiResponse);
                }

                VillaNumber villaNumber = await _dbVillaNumbers.GetAsync(i => i.VillaNo == villaNo);
                if(villaNumber == null)
                {
                    _apiResponse.StatusCode = HttpStatusCode.NotFound;
                    _apiResponse.IsSuccess = false;
                    return NotFound(_apiResponse);
                }
                _apiResponse.Response = _mapper.Map<VillaNumberDTO>(villaNumber);
                _apiResponse.IsSuccess = true;
                _apiResponse.StatusCode = HttpStatusCode.OK;
                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _apiResponse;
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> CreateVillaNumber([FromBody]VillaNumberCreateDTO villaNoCreate)
        {
            try
            {

                if(await _dbVilla.GetAsync(u=>u.id == villaNoCreate.VillaID) == null)
                {
                    ModelState.AddModelError("ErrorMessages", "Villa Id is invalid");
                    return BadRequest(ModelState);
                }

                if(await _dbVillaNumbers.GetAsync(u=> u.VillaNo == villaNoCreate.VillaNo ) != null)
                {
                    ModelState.AddModelError("ErrorMessages", "Villa Number already exists");
                    return BadRequest(ModelState);
                }

                if(villaNoCreate == null)
                {
                    _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_apiResponse);
                }

                VillaNumber villaNumber = _mapper.Map<VillaNumber>(villaNoCreate);
                await _dbVillaNumbers.CreateAsync(villaNumber);

                _apiResponse.Response = _mapper.Map<VillaNumberDTO>(villaNumber);
                _apiResponse.StatusCode = HttpStatusCode.Created;

                return CreatedAtRoute("GetVillaNumber", new { villaNo = villaNumber.VillaNo }, _apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.IsSuccess=false;
                _apiResponse.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _apiResponse;
        }



        [HttpDelete("{villaNumber:int}", Name = "DeleteVillaNumber")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> DeleteVillaNumber(int villaNumber)
        {
            try
            {
                if(villaNumber == 0)
                {
                    _apiResponse.Response = HttpStatusCode.BadRequest;
                    return BadRequest(_apiResponse);
                }

                VillaNumber villaNo = await _dbVillaNumbers.GetAsync(g => g.VillaNo == villaNumber);
                if (villaNo == null)
                {
                    _apiResponse.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_apiResponse);
                }
                await _dbVillaNumbers.RemoveAsync(villaNo);
                _apiResponse.StatusCode = HttpStatusCode.NoContent;
                _apiResponse.IsSuccess = true;
                return Ok(_apiResponse);

            }
            catch (Exception ex)
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _apiResponse;
        }



        [HttpPut("{villaNo:int}", Name = "UpdateVillaNumber")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> UpdateVillaNumber(int villaNo, [FromBody] VillaNumberUpdateDTO villaNoUpdateDTO)
        {
            
            try
            {

                if (await _dbVilla.GetAsync(u => u.id == villaNoUpdateDTO.VillaID) == null)
                {
                    ModelState.AddModelError("ErrorMessages", "Villa Id is invalid");
                    return BadRequest(ModelState);
                }

                if (villaNo!=villaNoUpdateDTO.VillaNo || villaNoUpdateDTO == null)
                {
                    _apiResponse.StatusCode=HttpStatusCode.BadRequest;
                    _apiResponse.IsSuccess = false;
                    return BadRequest(_apiResponse);
                }


                VillaNumber villaNumber = _mapper.Map<VillaNumber>(villaNoUpdateDTO);
                await _dbVillaNumbers.UpdateAsync(villaNumber);
                _apiResponse.StatusCode = HttpStatusCode.OK;
                _apiResponse.IsSuccess=true;
                _apiResponse.Response = _mapper.Map<VillaNumberDTO>(villaNumber);
            }
            catch (Exception ex)
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _apiResponse;
        }


        [HttpPatch("{id:int}", Name = "UpdatePartialVillaNumber")]
        public async Task<ActionResult<APIResponse>> UpdatePartialVillaNumber(int villaNo, JsonPatchDocument<VillaNumberUpdateDTO> patchDTO)
        {
            try
            {
                if (patchDTO == null || villaNo == 0)
                {
                    _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_apiResponse);
                }

                //we are adding the AsNoTracking here we dont want the EF to track the
                //villa with mentioned ID in this statement as we are updating it using
                //update method below and that one also has the villa model with id 6.
                //EF doesn't allow 1 model being tracked twice.
                var villa = await _dbVillaNumbers.GetAsync(i => i.VillaNo == villaNo, tracked: false);


                VillaNumberUpdateDTO villaNoDTO = _mapper.Map<VillaNumberUpdateDTO>(villa);



                if (villa == null)
                {
                    _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_apiResponse);
                }
                patchDTO.ApplyTo(villaNoDTO, ModelState);

                


                VillaNumber model = _mapper.Map<VillaNumber>(villaNoDTO);


                await _dbVillaNumbers.UpdateAsync(model);
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                _apiResponse.StatusCode = HttpStatusCode.OK;
                _apiResponse.Response = _mapper.Map<VillaNumberDTO>(model);

                return Ok(_apiResponse);
            }

            catch (Exception ex)
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.ErrorMessages = new List<string> { ex.Message };
            }
            return _apiResponse;

        }

    }
}
