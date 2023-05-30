using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Villa_WebApp.Models;
using Villa_WebApp.Models.DTO;
using Villa_WebApp.Services.IServices;

namespace Villa_WebApp.Controllers
{
    public class VillaNumberController : Controller
    {
        private readonly IVillaNumberServices _villaNumberService;
        private readonly IMapper _mapper;
        public VillaNumberController(IVillaNumberServices villaNumberService, IMapper mapper)
        {
            _villaNumberService = villaNumberService;
            _mapper = mapper;
        }
        public async Task<IActionResult> IndexVillaNumber()
        {
            List<VillaNumberDTO> list = new();

            var response = await _villaNumberService.GetAllAsync<APIResponse>();
            if (response != null && response.IsSuccess == true)
            {
                list = JsonConvert.DeserializeObject<List<VillaNumberDTO>>(Convert.ToString(response.Response));
            }

            return View(list);
        }
    }
}
