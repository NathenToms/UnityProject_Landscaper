using Landscaper.Save;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Landscaper.Chunks
{
    public class Chunk : MonoBehaviour
    {
        [Header("Component References")]
        [SerializeReference] private Transform _chunkTransform = null;
        [SerializeReference] private Transform _tilesTransform = null;

        [Header("Save Data")]
        public SaveData SaveData;

        [Header("Audio")]
        public AudioSource AudioSource;

        public AudioClip Rumbling;
        public AudioClip RumblingStop;

        [Header("Chunk Properties")]
        public float Speed = 1;
        public float Height = 1;
        public float StartHeight = 1;
        public float MaxHeight = 20;
        public float MinHeight = 1;

        [Header("Chunk Coordinates")]
        public Vector2Int ChunkID;

        // Start is called before the first frame update
        void Start()
        {
            if (Rumbling && SaveData) {
                AudioSource.volume = SaveData.EnvironmentalVolume;
                AudioSource.clip = Rumbling;
            }

            ChunkID = new Vector2Int((int)transform.position.x, (int)transform.position.z);

            SetHeight(StartHeight);
        }

        public void SetHeight(float height)
		{
            Height = height;

            StopAllCoroutines();
            StartCoroutine(CoUpdateHeight(height));
        }

        IEnumerator CoUpdateHeight(float height)
		{
            // Start playing the Rumbling sound for this chunk
            if (AudioSource) AudioSource.Play();

            // As long as we have not reached out target height
            while (_chunkTransform.localScale.y != height)
            {
				float newHeight = 0;

                // Calculate the chunks next position step
				if (height > _chunkTransform.localScale.y)
                    newHeight = Mathf.Clamp(_chunkTransform.localScale.y + (Speed * Time.deltaTime), MinHeight, height);
                else
                    newHeight = Mathf.Clamp(_chunkTransform.localScale.y - (Speed * Time.deltaTime), height, MaxHeight);

                // Set the chunks and its tile groups position
                _chunkTransform.localScale = new Vector3(1, newHeight, 1);
                _tilesTransform.localPosition = new Vector3(0, newHeight, 0);

                yield return null;
            }

            _chunkTransform.localScale = new Vector3(1, height, 1);

            // Now that we are done..
            // Stop the Rumbling sound
            if (AudioSource) AudioSource.Stop();
            if (AudioSource) AudioSource.PlayOneShot(RumblingStop);
        }
    }
}