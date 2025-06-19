// Add this to your Models folder
namespace SchoolManagementApp.Models
{
    public class FaceEmbedding
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public int PersonPhotoId { get; set; }
        public byte[] EmbeddingData { get; set; } // Will store the 512 float array as bytes
        public Person Person { get; set; }
        public PersonPhoto PersonPhoto { get; set; }
    }
}