using FaceONNX;
using SchoolManagementApp.Service;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SchoolManagementApp.Services
{
    public class FaceEmbeddingService : IFaceEmbeddingService
    {
        private readonly FaceDetector _faceDetector;
        private readonly Face68LandmarksExtractor _landmarkExtractor;
        private readonly FaceEmbedder _faceEmbedder;
        private readonly Embeddings _embeddings;
        private bool _disposed = false;

        public FaceEmbeddingService()
        {
            _faceDetector = new FaceDetector();
            _landmarkExtractor = new Face68LandmarksExtractor();
            _faceEmbedder = new FaceEmbedder();
            _embeddings = new Embeddings();
        }

        public float[] GenerateEmbedding(Image<Rgb24> image)
        {
            var array = GetImageFloatArray(image);
            var rectangles = _faceDetector.Forward(array);
            var rectangle = rectangles.FirstOrDefault().Box;

            if (!rectangle.IsEmpty)
            {
                var points = _landmarkExtractor.Forward(array, rectangle);
                var angle = points.RotationAngle;
                var aligned = FaceProcessingExtensions.Align(array, rectangle, angle);
                return _faceEmbedder.Forward(aligned);
            }

            return new float[512]; // Return empty embedding if no face found
        }

        public float[] GenerateEmbedding(byte[] imageBytes)
        {
            using var image = Image.Load<Rgb24>(imageBytes);
            return GenerateEmbedding(image);
        }

        public async Task StoreInChromaAsync(string personId, float[] embedding, string name)
        {
            try
            {

            }
            catch
            {
                // Silent failure as requested
            }
        }

        public (string Label, float Score) FindMostSimilar(float[] embedding)
        {
            return _embeddings.FromSimilarity(embedding);
        }

        public void AddEmbedding(float[] embedding, string label)
        {
            _embeddings.Add(embedding, label);
        }

        public void Initialize() { }

        private static float[][,] GetImageFloatArray(Image<Rgb24> image)
        {
            var array = new[]
            {
                new float[image.Height, image.Width],
                new float[image.Height, image.Width],
                new float[image.Height, image.Width]
            };

            image.ProcessPixelRows(pixelAccessor =>
            {
                for (var y = 0; y < pixelAccessor.Height; y++)
                {
                    var row = pixelAccessor.GetRowSpan(y);
                    for (var x = 0; x < pixelAccessor.Width; x++)
                    {
                        array[2][y, x] = row[x].R / 255.0f;
                        array[1][y, x] = row[x].G / 255.0f;
                        array[0][y, x] = row[x].B / 255.0f;
                    }
                }
            });

            return array;
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _faceDetector?.Dispose();
                _landmarkExtractor?.Dispose();
                _faceEmbedder?.Dispose();
                _disposed = true;
            }
        }
    }
}