using Villa_WebAPI.Models.DTO;

namespace Villa_WebAPI.Data
{
    public static class VillaStore
    {
        public static List<VillaDTO> villaList = new List<VillaDTO>
        {
            new VillaDTO() { id = 1, Name ="Pool View", Occupancy=4, Sqft=100},
            new VillaDTO() { id = 2, Name ="Beach View", Occupancy=4, Sqft=100},
        };
    }
}
