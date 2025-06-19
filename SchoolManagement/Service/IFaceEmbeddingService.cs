using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

public interface IFaceEmbeddingService : IDisposable
{
    // Keep all existing methods exactly the same
    float[] GenerateEmbedding(Image<Rgb24> image);
    float[] GenerateEmbedding(byte[] imageBytes);
    (string Label, float Score) FindMostSimilar(float[] embedding);
    void AddEmbedding(float[] embedding, string label);
    void Initialize();

    // Add this single new method
    Task StoreInChromaAsync(string personId, float[] embedding, string name);
}