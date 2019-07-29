namespace ProductService.Events.Contracts
{
    using System;

    /// <summary>
    ///  Contract for Integration Events
    /// </summary>
    public interface IIntegrationEvent
    {
         Guid Id { get; }
         DateTime CreationDate {get;}
    }
}