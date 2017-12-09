using AutoMapper;
using Lab.WebApplication.Mappings;

namespace Lab.WebApplication
{
    public class AutoMapperConfig
    {
        public static void Configure()
        {
            Mapper.Initialize
            (
                cfg =>
                {
                    cfg.AddProfile<MappingProfile>();
                }
            );
        }
    }
}