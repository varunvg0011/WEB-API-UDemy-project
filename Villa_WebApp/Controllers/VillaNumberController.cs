using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Villa_WebApp.Models;
using Villa_WebApp.Models.DTO;
using Villa_WebApp.Models.VM;
using Villa_WebApp.Services.IServices;

namespace Villa_WebApp.Controllers
{
    public class VillaNumberController : Controller
    {
        private readonly IVillaNumberServices _villaNumberService;
        private readonly IVillaServices _villaService;
        private readonly IMapper _mapper;
        public VillaNumberController(IVillaNumberServices villaNumberService, IMapper mapper, IVillaServices villaService)
        {
            _villaNumberService = villaNumberService;
            _mapper = mapper;
            _villaService = villaService;
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

        //For this we need createVillaNumber DTO as well as a dropdown.
        //we can use viewbag/viewdata to pass on those but we will create a separate view Model
        //for it as VillaNumberCreateVM
        public async Task<IActionResult> CreateVillaNumber()
        {
            //in this method, we are populating the villaList and sending it along with the VillaNumber object
            VillaNumberCreateVM villaNoVM = new();
            //Now when we create VillaNumber, we need to populate the list of
            //Villas in VillaNumberCreateVM:
            var response = await _villaService.GetAllAsync<APIResponse>();
            if (response != null && response.IsSuccess == true)
            {
                villaNoVM.VillaList = JsonConvert.DeserializeObject<List<VillaDTO>>
                    (Convert.ToString(response.Response)).Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.id.ToString()
                    }); ;
            }
            
            return View(villaNoVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateVillaNumber(VillaNumberCreateVM villaNoVM)
        {
            if (ModelState.IsValid)
            {
                var response = await _villaNumberService.CreateAsync<APIResponse>(villaNoVM.VillaNumber);
                if (response != null && response.IsSuccess == true)
                {
                    return RedirectToAction("IndexVillaNumber");
                }
                //incase we have some error, it should go to this and display some error.
                else
                {
                    if (response.ErrorMessages.Count > 0)
                    {
                        ModelState.AddModelError("ErrorMessages", response.ErrorMessages.FirstOrDefault());
                    }
                }
            }

           //if the model state is not true, we need to repopulate the dropdown again
            var resp = await _villaService.GetAllAsync<APIResponse>();
            if(resp != null && resp.IsSuccess)
            {
                villaNoVM.VillaList = JsonConvert.DeserializeObject<List<VillaDTO>>
                    (Convert.ToString(resp.Response)).Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.id.ToString()
                    }); ;

            }
            return View(villaNoVM);

        }



        public async Task<IActionResult> UpdateVillaNumber(int villaNo)
        {
            VillaNumberUpdateVM villaNoVM = new();
            var response = await _villaNumberService.GetAsync<APIResponse>(villaNo);
            if (response != null && response.IsSuccess == true)
            {
                VillaNumberDTO model = JsonConvert.DeserializeObject<VillaNumberDTO>(Convert.ToString(response.Response));
                return View(_mapper.Map<VillaNumberUpdateDTO>(model));
            }


            //for the dropdown and poulating villaList
            response = await _villaService.GetAllAsync<APIResponse>();
            if (response != null && response.IsSuccess == true)
            {
                villaNoVM.VillaList = JsonConvert.DeserializeObject<List<VillaDTO>>
                    (Convert.ToString(response.Response)).Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.id.ToString()
                    });
                return View(villaNoVM);
            }

            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateVillaNumber(VillaNumberUpdateVM model)
        {
            if (ModelState.IsValid)
            {
                var response = await _villaNumberService.UpdateAsync<APIResponse>(model.VillaNumber);
                if (response != null && response.IsSuccess == true)
                {
                    return RedirectToAction("IndexVillaNumber");
                }
            }

            //if the model state is not true, we need to repopulate the dropdown again
            var resp = await _villaService.GetAllAsync<APIResponse>();
            if (resp != null && resp.IsSuccess)
            {
                model.VillaList = JsonConvert.DeserializeObject<List<VillaDTO>>
                    (Convert.ToString(resp.Response)).Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.id.ToString()
                    }); ;

            }
            return View(model);

        }

        public async Task<IActionResult> DeleteVillaNumber(int villaNo)
        {
            VillaNumberDeleteVM villaNoVM = new();
            //getting the villa we want to delete
            var response = await _villaNumberService.GetAsync<APIResponse>(villaNo);
            if (response != null && response.IsSuccess == true)
            {
                VillaNumberDTO model = JsonConvert.DeserializeObject<VillaNumberDTO>(Convert.ToString(response.Response));
                return View(_mapper.Map<VillaNumberUpdateDTO>(model));
            }

            response = await _villaService.GetAllAsync<APIResponse>();
            if (response != null && response.IsSuccess == true)
            {
                villaNoVM.VillaList = JsonConvert.DeserializeObject<List<VillaDTO>>
                    (Convert.ToString(response.Response)).Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.id.ToString()
                    });
                return View(villaNoVM);
            }

            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteVillaNumber(VillaNumberDeleteVM villaNoVM)
        {
            {
                var response = await _villaNumberService.DeleteAsync<APIResponse>(villaNoVM.VillaNumber.VillaNo);
                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction("IndexNumberVilla");
                }

                return View(villaNoVM);
            }

        }
    }
}
