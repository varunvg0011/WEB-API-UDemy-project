using System.ComponentModel.DataAnnotations;

namespace Villa_WebApp.Models.DTO
{
    public class VillaNumberDTO
    {


        [Required]
        public int VillaNo { get; set; }

        [Required]
        public int VillaID { get; set; }

        //this is the nvigation property to get Villa Name from Villa.
        //This will be populated when we return Villa Number
        public VillaDTO Villa { get; set; }
        public string SpecialDetails { get; set; }
    }
}
