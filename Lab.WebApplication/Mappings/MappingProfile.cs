using AutoMapper;
using Lab.Repository;
using Lab.WebApplication.Models;

namespace Lab.WebApplication.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<WifiSpotModel, WifiSpot>();
        }
    }
}