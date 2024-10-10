namespace CustomEmailSender.Services
{
    public interface ICosmosService
    {
        Task AddItemAsync<T>(T item);
        Task<T> GetItemAsync<T>(string id, string email);
        Task<IEnumerable<T>> GetItemsAsync<T>(string queryString);
    }
}
