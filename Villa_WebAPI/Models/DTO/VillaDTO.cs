using System.ComponentModel.DataAnnotations;

namespace Villa_WebAPI.Models.DTO
{
    public class VillaDTO
    {
        public int id { get; set; }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }
        public int Occupancy { get; set; }
        public int Sqft { get; set; }

    }
}
