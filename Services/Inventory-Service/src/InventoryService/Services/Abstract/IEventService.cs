namespace InventoryService.Services.Abstract {
    using System.Threading.Tasks;
    using System.Threading;

    public interface IEventService {
        Task Subscribe ();
        Task UnSubscribe (CancellationToken token);
    }
}