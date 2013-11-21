using System;
using UnityEngine;
using System.Collections.Generic;
using Object = UnityEngine.Object;

namespace InAudio
{

    public class AudioGOPool : MonoBehaviour
    {
        private List<RuntimePlayer> freeObjects = new List<RuntimePlayer>();

        private Vector3 offscreen = new Vector3(-10000, -10000, -10000);

        [Range(0, 128)]
        public int InitialSize = 10;
        
        public int ChunkSize = 20;

        public GameObject RuntimeAudioPrefab;
        

        void Awake()
        {
            ReserveExtra(InitialSize);
        }


        public void ReleaseObject(RuntimePlayer player)
        {
            player.transform.parent = transform;
            player.transform.position = offscreen;
            freeObjects.Add(player);
            player.gameObject.SetActive(false);
        }

        public void ReserveExtra(int extra)
        {
            for (int i = 0; i < extra; ++i)
            {
                var go = Object.Instantiate(RuntimeAudioPrefab, offscreen, Quaternion.identity) as GameObject;
                go.name = "Audio Object " + i;
                go.transform.parent = transform;
                freeObjects.Add(go.GetComponent<RuntimePlayer>());
                var runtimeAudio = freeObjects[freeObjects.Count - 1];
                go.SetActive(false);
                runtimeAudio.Initialize(this);
            }
        }

        public RuntimePlayer GetObject()
        {
            RuntimePlayer player;
            if (freeObjects.Count > 0)
            {
                player = freeObjects[freeObjects.Count - 1];
                freeObjects.RemoveAt(freeObjects.Count - 1);
            }
            else
            {
                ReserveExtra(ChunkSize);
                player = freeObjects[freeObjects.Count - 1];
                freeObjects.RemoveAt(freeObjects.Count - 1);
            }
            player.gameObject.SetActive(true);
            return player;
        }
    }
}
