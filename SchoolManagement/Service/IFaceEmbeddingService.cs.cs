using ChromaDB.Client;

namespace SchoolManagementApp.Services
{
    public class ChromaHelper
    {
        private readonly ChromaCollectionClient _client;

        public ChromaHelper(ChromaCollectionClient client)
        {
            _client = client;
        }

        public async Task SaveEmbedding(string personPhotoId, float[] embedding)
        {
            await _client.Add(
                ids: new List<string> { personPhotoId },
                embeddings: new List<ReadOnlyMemory<float>> { new ReadOnlyMemory<float>(embedding) },
                metadatas: new List<Dictionary<string, object>>
                {
                    new Dictionary<string, object> { {"personPhotoId", personPhotoId } }
                });
        }
    }
}