using System.ComponentModel.DataAnnotations;

namespace Villa_WebAPI.Models.DTO
{
    public class VillaNumberDTO
    {


        [Required]
        public int VillaNo { get; set; }

        [Required]
        public int VillaID { get; set; }

        //this property is created later on in project as a navigation property so that it can be
        //populated as well along with VillaNumber to be fetched in UI in MVC.
        //We also have a navigation property in VillaNumber, we need to explicitly tell EF core
        //that when you populate VillaNumber, you also populate Villa property in that class based
        //on the forign key relation
        public VillaDTO Villa { get; set; }
        public string SpecialDetails { get; set; }
    }
}

