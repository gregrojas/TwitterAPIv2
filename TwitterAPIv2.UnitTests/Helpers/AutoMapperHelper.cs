using AutoMapper;
using TwitterAPIv2.Application.Mapping;

namespace TwitterAPIv2.UnitTests.Helpers
{
    public class AutoMapperHelper
    {
        private static IMapper _mapper;

        public static IMapper Mapper
        {
            get
            {
                if (_mapper == null)
                {
                    var mappingConfig = new MapperConfiguration(mc =>
                    {
                        mc.AddProfile(new Mappings());
                    });
                    IMapper mapper = mappingConfig.CreateMapper();
                    _mapper = mapper;
                }

                return _mapper;
            }
        }
    }
}
