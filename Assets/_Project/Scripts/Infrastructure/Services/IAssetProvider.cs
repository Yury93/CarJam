using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace _Project.Scripts.Infrastructure.Services
{
    public interface IAssetProvider : IService
    {
        Task<GameObject> instatiateAsync(string name);
        Task<GameObject> instatiateAsync(string name, Vector3 position);
        Task<GameObject> instatiateAsync(string name, Transform parent);

        Task<Material> instatiateMaterialAsync(string name);

        Task<T> LoadAsync<T>(string name) where T : class;
        void ReleaseAssets();
    }
}