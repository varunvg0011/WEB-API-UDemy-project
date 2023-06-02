using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using Villa_WebApp.Models.DTO;

namespace Villa_WebApp.Models.VM
{
    public class VillaNumberDeleteVM
    {
        //this is the model we are using for CreateVillaNumberView as we need for VillasNumber as well
        //as all Villas
        public VillaNumberDTO VillaNumber { get; set; }

        public VillaNumberDeleteVM()
        {
            VillaNumber = new VillaNumberDTO();
        }
        [ValidateNever]
        public IEnumerable<SelectListItem> VillaList { get; set; }
    }
}
