using System.ComponentModel.DataAnnotations;

namespace Villa_WebApp.Models.DTO
{
    public class VillaCreateDTO
    {
        

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        // ? because in .net6.0 model takes everything as required by default unless defined otherwise or we can disable nullable in project settings
        public string Details { get; set; }

        [Required]
        public double Rate { get; set; }
        public int Occupancy { get; set; }
        public int Sqft { get; set; }
        public string ImageUrl { get; set; }
        public string Amenity { get; set; }
    }
}
