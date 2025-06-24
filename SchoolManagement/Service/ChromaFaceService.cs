using ChromaDB.Client;
using Microsoft.CodeAnalysis;
using System.Net.Http;
using System.Numerics;

namespace SchoolManagementApp.Services
{
    public class ChromaFaceService : IFaceEmbeddingStore, IDisposable
    {
        private readonly HttpClient _httpClient = new();
        private readonly ChromaClient _client;
        private const string CollectionName = "FaceEmbedding2406";

        public ChromaFaceService()
        {
            var config = new ChromaConfigurationOptions("http://localhost:8000/api/v1/");
            _client = new ChromaClient(config, _httpClient);
        }

        public async Task StoreEmbeddingAsync(int personId, int photoId, float[] embedding)
        {
            var collection = await _client.GetOrCreateCollection(CollectionName);
            var collectionClient = new ChromaCollectionClient(collection,
                new ChromaConfigurationOptions("http://localhost:8000/api/v1/"),
                _httpClient);
            //await collectionClient.Add([review.Ids], [vector], [metadata]);

            await collectionClient.Add(
                ids: new List<string> { photoId.ToString() },
                embeddings: new List<ReadOnlyMemory<float>> { embedding.AsMemory() },
                metadatas: new List<Dictionary<string, object>>
                {
                    new Dictionary<string, object>
                    {
                        { "person_id", personId },
                        { "photo_id", photoId },
                        { "timestamp", DateTime.UtcNow.ToString("o") }
                    }
                });
        }
        public async Task<int> GetEmbeddingsCountAsync()
        {
            var collection = await _client.GetOrCreateCollection(CollectionName);
            var collectionClient = new ChromaCollectionClient(collection,
                new ChromaConfigurationOptions("http://localhost:8000/api/v1/"),
                _httpClient);
            var count = await collectionClient.Count();
            return count;
        }
        //public async Task<List<(int PhotoId, float Score)>> FindSimilarFacesAsync(float[] embedding, int limit = 5)
        //{
        //    var collection = await _client.GetOrCreateCollection(CollectionName);
        //    var collectionClient = new ChromaCollectionClient(collection,
        //        new ChromaConfigurationOptions("http://localhost:8000/api/v1/"),
        //        _httpClient);

        //    var results = await collectionClient.Query(
        //        queryEmbeddings: new List<ReadOnlyMemory<float>> { embedding.AsMemory() },
        //        nResults: limit,
        //        include: ChromaQueryInclude.Metadatas | ChromaQueryInclude.Distances);

        //    return results.Items.Select(item => (
        //        PhotoId: int.Parse(item.Metadata["photo_id"].ToString()),
        //        Score: 1 - (float)item.Distance
        //    )).ToList();
        //}

        public void Dispose() => _httpClient.Dispose();
    }
}