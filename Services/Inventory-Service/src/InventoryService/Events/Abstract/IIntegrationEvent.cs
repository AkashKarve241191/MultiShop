namespace InventoryService.Events.Abstract
{
    using System;

    /// <summary>
    ///  Contract for Integration Events
    /// </summary>
    public interface IIntegrationEvent
    {
         Guid Id { get; set; }
         DateTime CreationDate {get; set;}
    }
}