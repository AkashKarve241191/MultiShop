namespace InventoryService {
    using AutoMapper;
    using InventoryService.Commands;
    using InventoryService.Domain;
    using InventoryService.Events;

    public class MappingProfile : Profile {
        public MappingProfile () {
            CreateMap<ProductAddedEvent, AddNewProductToInventoryCommand> ();
            CreateMap<AddNewProductToInventoryCommand, InventoryStore> ();
            CreateMap<ProductUpdatedEvent, UpdateProductToInventoryCommand> ();
            CreateMap<UpdateProductToInventoryCommand, InventoryStore> ();
            CreateMap<DeleteProductFromInventoryCommand, int> ();
            CreateMap<ProductDeletedEvent, DeleteProductFromInventoryCommand> ();
        }
    }
}