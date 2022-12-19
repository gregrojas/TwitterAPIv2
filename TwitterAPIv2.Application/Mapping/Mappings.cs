using System;
using System.Linq;
using AutoMapper;
using TwitterAPIv2.Application.Dtos;

namespace TwitterAPIv2.Application.Mapping
{
    public class Mappings : Profile
    {
        public Mappings()
        {
            TweetMappings();
        }

        private void TweetMappings()
        {
            //CreateMap<TweetDto, PaymentInvoiceRelationshipDto>()
            //    .ForMember(dest => dest.id, opt => opt.MapFrom(src => src.id))
            //    .ForMember(dest => dest.text, opt => opt.MapFrom(src => src.text))
            //    .ForMember(dest => dest.StatusHistory, opt => opt.MapFrom(src => InvoiceStatusMapping.TransformToCfmStatus(src.StatusHistory)));

            CreateMap<TweetDto, Entities>()
                .ForMember(dest => dest.annotations, opt => opt.MapFrom(src => src.public_metrics));

            CreateMap<TweetDto, Entities>()
                .ForMember(dest => dest.annotations, opt => opt.MapFrom(src => src.public_metrics));

        }   
    }
}