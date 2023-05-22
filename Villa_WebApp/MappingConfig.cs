using Villa_WebApp.Models.DTO;
using AutoMapper;
using Villa_WebApp.Models.DTO;

namespace Villa_WebApp
{
    public class MappingConfig:Profile
    {
        public MappingConfig()
        {
            CreateMap<VillaDTO, VillaCreateDTO>().ReverseMap();
            CreateMap<VillaDTO, VillaUpdateDTO>().ReverseMap();
            CreateMap<VillaNumberDTO, VillaNumberCreateDTO>().ReverseMap();
            CreateMap<VillaNumberDTO, VillaNumberUpdateDTO>().ReverseMap();
        }
    }
}
