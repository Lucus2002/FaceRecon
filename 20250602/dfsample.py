import os
import cv2
import numpy as np
from deepface import DeepFace
from scipy.spatial.distance import cosine
import uuid
from datetime import datetime
from typing import List
import logging

# Configure logging
logging.basicConfig(level=logging.DEBUG)
logger = logging.getLogger(__name__)


if not os.path.exists("faces"):
    os.makedirs("faces")
if not os.path.exists("embeddings"):
    os.makedirs("embeddings")

def detect_and_save_face(image_path, output_dir="faces"):
    """Detect face in image, save cropped face, and return face info"""
    try:
        # Read image
        img = cv2.imread(image_path)
        if img is None:
            raise ValueError(f"Could not read image: {image_path}")

        # Detect face using DeepFace
        faces = DeepFace.extract_faces(img_path=image_path, detector_backend='opencv')
        
        if not faces:
            raise ValueError("No faces detected in the image")

        # Get first detected face
        face = faces[0]
        facial_area = face['facial_area']
        
        # Crop face
        x, y, w, h = facial_area['x'], facial_area['y'], facial_area['w'], facial_area['h']
        cropped_face = img[y:y+h, x:x+w]
        
        # Generate unique filename
        timestamp = datetime.now().strftime("%Y%m%d_%H%M%S")
        unique_id = str(uuid.uuid4())[:8]
        output_filename = f"face_{timestamp}_{unique_id}.jpg"
        output_path = os.path.join(output_dir, output_filename)
        
        # Save cropped face
        cv2.imwrite(output_path, cropped_face)
        
        return {
            'filename': output_filename,
            'path': output_path,
            'facial_area': facial_area
        }
    except Exception as e:
        print(f"Error processing {image_path}: {str(e)}")
        return None

def generate_embedding(image_path, model_name='Facenet'):
    """Generate vector embedding for a face image"""
    try:
        # Generate embedding using DeepFace
        embedding = DeepFace.represent(img_path=image_path, model_name=model_name,enforce_detection=True,detector_backend="opencv")
        #return np.array(embedding[0]['embedding'])
        print("Hello!")
        #embedding_np = np.array(embedding, dtype=np.float32)
        print(embedding[0]['embedding'])
        result = embedding[0] if isinstance(embedding, list) else embedding
        embedding = np.array(result["embedding"], dtype=np.float32)
        facial_area = result["facial_area"]  # Dictionary with x, y, w, h
        print(f"Embedding shape: {embedding.shape}, Facial area: {facial_area}")
        return embedding.tolist()
    except Exception as e:
        print(f"Error generating embedding for {image_path}: {str(e)}")
        return None

def save_embedding(embedding, filename, output_dir="embeddings"):
    """Save embedding to file"""
    try:
        embedding_filename = os.path.splitext(filename)[0] + ".npy"
        output_path = os.path.join(output_dir, embedding_filename)
        np.save(output_path, embedding)
        return output_path
    except Exception as e:
        print(f"Error saving embedding for {filename}: {str(e)}")
        return None

def load_embeddings(embeddings_dir="embeddings"):
    """Load all embeddings from directory"""
    embeddings = []
    for file in os.listdir(embeddings_dir):
        if file.endswith('.npy'):
            embedding = np.load(os.path.join(embeddings_dir, file))
            embeddings.append((file, embedding))
    return embeddings

def find_similar_faces(query_embedding, embeddings, threshold=0.4):
    """Find similar faces based on cosine similarity"""
    similar_faces = []
    
    for filename, embedding in embeddings:
        similarity = 1 - cosine(query_embedding, embedding)
        if similarity > threshold:
            similar_faces.append((filename, similarity))
    
    # Sort by similarity in descending order
    similar_faces.sort(key=lambda x: x[1], reverse=True)
    return similar_faces

def main():
    # Example usage
    image_path = "test.jpg"  # Replace with your image path
    
    # Detect and save face
    face_info = detect_and_save_face(image_path)
    if face_info:
        print(f"Face saved: {face_info['path']}")
        
        # Generate embedding
        embedding = generate_embedding(face_info['path'])
        if embedding is not None:
            print(embedding)
        else:
            print("Failed to generate embedding")
    else:
        print("Failed to detect face")

if __name__ == "__main__":
    main()