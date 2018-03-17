using AutoMapper;

namespace Blog.Service.Mapping
{
    public static class AutoMapperConfig
    {
        public static MapperConfiguration RegisterMappings()
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new DomainMappingToDtoProfile());
            });
        }
    }
}