using AutoMapper;
using Ollert.DataAccess.Entitites;

namespace Ollert.Logic.DTOs.Mappings
{
    public class AutoMapperLogicProfile : Profile
    {
        public AutoMapperLogicProfile()
        {
            CreateMap<CategoryDTO, Category>()
                .ForMember(entity => entity.IsDeleted, cfg => cfg.MapFrom(dto => false))
                .ReverseMap();

        }
    }
}
