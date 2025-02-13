using UnityEngine;

namespace _Project.Scripts.Infrastructure.Services
{
    public class AssetProvider : IAssetProvider
    { 
        public GameObject instatiate(string path)
        {
            var go = Resources.Load<GameObject>(path);
            return GameObject.Instantiate(go);
        } 
        public GameObject instatiate(string path, Vector3 position)
        {
            var go = Resources.Load<GameObject>(path);
            return GameObject.Instantiate(go, position, Quaternion.identity);
        }
        public GameObject instatiate(string path, Transform parent)
        {
            var go = Resources.Load<GameObject>(path);
            return GameObject.Instantiate(go, parent);
        }
    }
}