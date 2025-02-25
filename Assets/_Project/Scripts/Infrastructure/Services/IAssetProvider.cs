using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace _Project.Scripts.Infrastructure.Services
{
    public interface IAssetProvider : IService
    {
        Task<GameObject> instatiateAsync(string path);
        Task<GameObject> instatiateAsync(string path, Vector3 position);
        Task<GameObject> instatiateAsync(string path, Transform parent);
        Task<T> LoadAsync<T>(string name) where T : class; 
    }
}