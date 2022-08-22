using AutoMapper;
using System;
using System.Text.RegularExpressions;

namespace BuyWithOffer
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // Offer service
            CreateMap<OfferDto, Offer>();
            CreateMap<Offer, OfferDto>();
            CreateMap<UpdateOfferDto, Offer>();
            CreateMap<CreateOfferDto, Offer>()
                .ForMember(x => x.ModifiedBy, opt => opt.Ignore())
                .ForMember(x => x.ModifiedById, opt => opt.Ignore())
                .ForMember(x => x.CreatedDate, opt => opt.MapFrom(s => DateTime.UtcNow))
                .ForMember(x => x.ModifiedDate, opt => opt.Ignore());

            // Diger Servislerde AutoMapper kullanilmamistir.
        }
    }
}
