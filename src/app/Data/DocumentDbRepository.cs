using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;

using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;

namespace MusicBot.App.Data
{
    public class DocumentDbRepository<T> : IDocumentDbRepository<T> where T : Document
    {
        private readonly DocumentClient _client;
        private readonly string _collectionId;
        private readonly string _databaseId;
        private readonly Uri _uri;

        public DocumentDbRepository(DocumentClient client, string databaseId, string collectionId)
        {
            _client = client;
            _databaseId = databaseId;
            _collectionId = collectionId;
            _uri = UriFactory.CreateDocumentCollectionUri(_databaseId, _collectionId);
            CreateCollectionIfNotExistsAsync().Wait();
        }

        public async Task<IEnumerable<T>> GetItemsAsync(Expression<Func<T, bool>> predicate)
        {
            var query = _client.CreateDocumentQuery<T>(_uri)
                .Where(predicate)
                .AsDocumentQuery();

            var results = new List<T>();
            while (query.HasMoreResults)
                results.AddRange(await query.ExecuteNextAsync<T>());

            return results;
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
        {
            var query = await _client.CreateDocumentQuery<T>(_uri)
                .Where(predicate)
                .CountAsync();

            return query > 0;
        }

        public async Task<Document> CreateItemAsync(T item)
        {
            return await _client.CreateDocumentAsync(
                UriFactory.CreateDocumentCollectionUri(_databaseId, _collectionId), item);
        }

        public async Task<T> FirstOrDefault(Expression<Func<T, bool>> predicate)
        {
            return (await _client.CreateDocumentQuery<T>(_uri, new FeedOptions {MaxItemCount = 1})
                .Where(predicate)
                .AsDocumentQuery()
                .ExecuteNextAsync<T>()).FirstOrDefault();
        }

        public async Task ClearAllAsync()
        {
            await _client.DeleteDocumentCollectionAsync(_uri);
            await CreateCollectionIfNotExistsAsync();
        }

        private async Task CreateCollectionIfNotExistsAsync()
        {
            try
            {
                await _client.ReadDocumentCollectionAsync(_uri);
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == HttpStatusCode.NotFound)
                    await _client.CreateDocumentCollectionAsync(
                        UriFactory.CreateDatabaseUri(Config.Instance.DocumentDbDatabaseId),
                        new DocumentCollection {Id = _collectionId},
                        new RequestOptions {OfferThroughput = 1000});
                else
                    throw;
            }
        }
    }

    public interface IDocumentDbRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetItemsAsync(Expression<Func<T, bool>> predicate);
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
        Task<Document> CreateItemAsync(T item);
        Task<T> FirstOrDefault(Expression<Func<T, bool>> predicate);
        Task ClearAllAsync();
    }
}