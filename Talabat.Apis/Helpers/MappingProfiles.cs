using AutoMapper;
using Talabat.Apis.Dtos;
using Talabat.APIs.Dtos;
using Talabat.Core.Models.Basket;
using Talabat.Core.Models.Identity;
using Talabat.Core.Models.Product;

namespace Talabat.Apis.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductToReturnDto>()
                .ForMember(d => d.Brand, O => O.MapFrom(s => s.Brand.Name))
                .ForMember(d => d.Category, O => O.MapFrom(s => s.Category.Name))
                .ForMember(d => d.PictureUrl, O => O.MapFrom<ProductPictureUrlResolver>());


            CreateMap<BasketItemDto, BasketItem>();
            IMappingExpression<CustomerBasketDto, CustomerBasket> mappingExpression = CreateMap<CustomerBasketDto, CustomerBasket>();

            CreateMap<Address, AddressDto>().ReverseMap();
        }
    }
}
