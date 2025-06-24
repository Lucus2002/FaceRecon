namespace SchoolManagementApp.Services
{
    public interface IFaceEmbeddingStore
    {
        Task StoreEmbeddingAsync(int personId, int photoId, float[] embedding);
        Task<int> GetEmbeddingsCountAsync();
        //Task<List<(int PhotoId, float Score)>> FindSimilarFacesAsync(float[] embedding, int limit = 5);
    }
}