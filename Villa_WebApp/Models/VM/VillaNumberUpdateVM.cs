using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using Villa_WebApp.Models.DTO;

namespace Villa_WebApp.Models.VM
{
    public class VillaNumberUpdateVM
    {
        //this is the model we are using for CreateVillaNumberView as we need for VillasNumber as well
        //as all Villas
        public VillaNumberUpdateDTO VillaNumber { get; set; }

        public VillaNumberUpdateVM()
        {
            VillaNumber = new VillaNumberUpdateDTO();
        }
        [ValidateNever]
        public IEnumerable<SelectListItem> VillaList { get; set; }
    }
}
