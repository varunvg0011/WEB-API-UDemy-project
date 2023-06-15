using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Villa_WebAPI.Data;
using Villa_WebAPI.Models;
using Villa_WebAPI.Models.DTO;
using Villa_WebAPI.Repository;
using System.Net;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Authorization;

namespace Villa_WebAPI.Controllers
{
    //specifying which version of API we want to invoke. we specify the API version in program.cs
    //to make swagger hit that API version

    [Route("api/v{version:apiVersion}/VillaNumberAPI")]
    [ApiController] 
    //Here we are specifying APi version 1.0 as we are using 1.0, if we specify other
    //API version it wont work
    
    //when we are using 2 versions of api in 1 project we have to specify which one of Get() methd 
    //is using which api version and then we use [MapToApiVersion] attribute to map tho
    //se APIs
    [ApiVersion("2.0")]
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

        
        

        
        [HttpGet("GetString")]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }


        

    }
}
