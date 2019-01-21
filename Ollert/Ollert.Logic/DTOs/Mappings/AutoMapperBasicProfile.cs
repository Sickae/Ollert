using AutoMapper;

namespace Ollert.Logic.DTOs.Mappings
{
    public class AutoMapperBasicProfile : Profile
    {
        public AutoMapperBasicProfile()
        {
            AllowNullCollections = true;
        }
    }
}
