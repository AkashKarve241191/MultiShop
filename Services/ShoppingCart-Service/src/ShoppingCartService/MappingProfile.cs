namespace ShoppingCartService {
    using AutoMapper;
    using ShoppingCartService.Commands;
    using ShoppingCartService.Domain;

    public class MappingProfile : Profile {
        public MappingProfile () {
            CreateMap<ShoppingCart, AddNewShoppingCartCommand> ()
                .ForMember (dest => dest.ShoppingCartId, opt => opt.MapFrom (src => src.ShoppingCartId))
                .ForMember (dest => dest.UserId, opt => opt.MapFrom (src => src.UserId))
                .ForMember (dest => dest.ShoppingCartItems, opt => opt.MapFrom (src => src.ShoppingCartItems));

            CreateMap<AddNewShoppingCartCommand, ShoppingCart> ()
                .ForMember (dest => dest.ShoppingCartId, opt => opt.MapFrom (src => src.ShoppingCartId))
                .ForMember (dest => dest.UserId, opt => opt.MapFrom (src => src.UserId))
                .ForMember (dest => dest.ShoppingCartItems, opt => opt.MapFrom (src => src.ShoppingCartItems));

            CreateMap<ShoppingCartItem, ShoppingCartItemCommand> ()
                .ForMember (dest => dest.ProductId, opt => opt.MapFrom (src => src.ProductId))
                .ForMember (dest => dest.ProductDescription, opt => opt.MapFrom (src => src.ProductDescription))
                .ForMember (dest => dest.ProductName, opt => opt.MapFrom (src => src.ProductName))
                .ForMember (dest => dest.Quantity, opt => opt.MapFrom (src => src.Quantity))
                .ForMember (dest => dest.UnitPrice, opt => opt.MapFrom (src => src.UnitPrice))
                .ForMember (dest => dest.ShoppingCartItemId, opt => opt.MapFrom (src => src.ShoppingCartItemId))
                .ForMember (dest => dest.ShoppingCartId, opt => opt.MapFrom (src => src.ShoppingCartId));

            CreateMap<ShoppingCartItemCommand, ShoppingCartItem> ()
                .ForMember (dest => dest.ProductId, opt => opt.MapFrom (src => src.ProductId))
                .ForMember (dest => dest.ProductDescription, opt => opt.MapFrom (src => src.ProductDescription))
                .ForMember (dest => dest.ProductName, opt => opt.MapFrom (src => src.ProductName))
                .ForMember (dest => dest.Quantity, opt => opt.MapFrom (src => src.Quantity))
                .ForMember (dest => dest.UnitPrice, opt => opt.MapFrom (src => src.UnitPrice))
                .ForMember (dest => dest.ShoppingCartItemId, opt => opt.MapFrom (src => src.ShoppingCartItemId))
                .ForMember (dest => dest.ShoppingCartId, opt => opt.MapFrom (src => src.ShoppingCartId));

            CreateMap<ShoppingCart, UpdateShoppingCartCommand> ()
                .ForMember (dest => dest.ShoppingCartId, opt => opt.MapFrom (src => src.ShoppingCartId))
                .ForMember (dest => dest.UserId, opt => opt.MapFrom (src => src.UserId))
                .ForMember (dest => dest.ShoppingCartItems, opt => opt.MapFrom (src => src.ShoppingCartItems));

            CreateMap<UpdateShoppingCartCommand, ShoppingCart> ()
                .ForMember (dest => dest.ShoppingCartId, opt => opt.MapFrom (src => src.ShoppingCartId))
                .ForMember (dest => dest.UserId, opt => opt.MapFrom (src => src.UserId))
                .ForMember (dest => dest.ShoppingCartItems, opt => opt.MapFrom (src => src.ShoppingCartItems));

            CreateMap<int, DeleteShoppingCartCommand>();

        }
    }

}