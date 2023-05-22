using System.ComponentModel.DataAnnotations;

namespace Villa_WebAPI.Models.DTO
{
    public class VillaNumberDTO
    {


        [Required]
        public int VillaNo { get; set; }

        [Required]
        public int VillaID { get; set; }
        public string SpecialDetails { get; set; }
    }
}
