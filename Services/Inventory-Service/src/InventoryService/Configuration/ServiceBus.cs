namespace InventoryService.Configuration {
    
    /// <summary>
    /// Configuration settings for Service Bus
    /// </summary>
    public class ServiceBus {
         public string Namespace { get; set; }
        public string AccessKeyName { get; set; }
        public string AccessKeyValue { get; set; }
        public string ConnectionString { get; set; }
        public string ProductAddedTopic { get; set; }
        public string ProductDeletedTopic { get; set; }
        public string ProductUpdatedTopic { get; set; }
        public string ProductAddedSubscription { get; set; }
        public string ProductUpdatedSubscription { get; set; }
        public string ProductDeletedSubscription { get; set; }
    }
}