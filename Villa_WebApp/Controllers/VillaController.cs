using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Villa_WebApp.Models;
using Villa_WebApp.Models.DTO;
using Villa_WebApp.Services;
using Villa_WebApp.Services.IServices;

namespace Villa_WebApp.Controllers
{
    public class VillaController : Controller
    {
        private readonly IVillaServices _villaService;
        private readonly IMapper _mapper;
        public VillaController(IVillaServices villaServices, IMapper mapper)
        {
            _villaService = villaServices;
            _mapper = mapper;
        }
        public async Task<IActionResult> IndexVilla()
        {
            List<VillaDTO> list = new();

            var response = await _villaService.GetAllAsync<APIResponse>();
            if (response != null && response.IsSuccess == true)
            {
                list = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(response.Response));
            }

            return View(list);
        }


        public async Task<IActionResult> CreateVilla()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateVilla(VillaCreateDTO villa)
        {
            if (ModelState.IsValid)
            {
                var response = await _villaService.CreateAsync<APIResponse>(villa);
                if (response != null && response.IsSuccess == true)
                {
                    TempData["success"] = "Villa created successfully";
                    return RedirectToAction("IndexVilla");
                }
            }
            TempData["error"] = "Error encountered";
            return View(villa);
            
        }
        
        public async Task<IActionResult> UpdateVilla(int villaId)
        {
            var response = await _villaService.GetAsync<APIResponse>(villaId);
            if (response != null && response.IsSuccess == true)
            {
                VillaDTO model = JsonConvert.DeserializeObject<VillaDTO>(Convert.ToString(response.Response));
                return View(_mapper.Map<VillaUpdateDTO>(model));
            }
            return NotFound();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateVilla(VillaUpdateDTO villa)
        {
            if (ModelState.IsValid)
            {
                var response = await _villaService.UpdateAsync<APIResponse>(villa);
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Villa updated succesfully";
                    return RedirectToAction("IndexVilla");
                }
            }
            TempData["error"] = "Error encountered";
            return View(villa);
            
        }

        public async Task<IActionResult> DeleteVilla(int villaId)
        {
            var response = await _villaService.GetAsync<APIResponse>(villaId);
            if (response != null && response.IsSuccess == true)
            {
                VillaDTO model = JsonConvert.DeserializeObject<VillaDTO>(Convert.ToString(response.Response));

                return View(model);
            }
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteVilla(VillaDTO villa)
        {

            {
                var response = await _villaService.DeleteAsync<APIResponse>(villa.id);
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Villa deleted succesfully";
                    return RedirectToAction("IndexVilla");
                }
                TempData["error"] = "Error encountered";
                return View(villa);
            }

        }
    }
}
