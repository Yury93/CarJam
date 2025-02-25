using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UIElements;

namespace _Project.Scripts.Infrastructure.Services
{
    public class AssetProvider : IAssetProvider
    {
        public Dictionary<string, AsyncOperationHandle> _handleResources = new Dictionary<string, AsyncOperationHandle>();
        public Dictionary<string, AsyncOperationHandle> _cachedResources = new Dictionary<string, AsyncOperationHandle>();
        public AssetProvider()
        {
            Addressables.InitializeAsync();
        }
 
        public async Task<GameObject> instatiateAsync(string path)
        {
            var task = LoadAsync<GameObject>(path);
            await task;
            var result = task.Result;
            var go = GameObject.Instantiate(result);
            return go;
        } 
        public async Task<GameObject> instatiateAsync(string path, Vector3 position)
        {
            var task = LoadAsync<GameObject>(path);
            await task;
            var result = task.Result;
            var go = GameObject.Instantiate(result);
            go.transform.position = position;
            return go;
        }
        public async Task<GameObject> instatiateAsync(string path, Transform parent)
        {
           var task = LoadAsync<GameObject>(path);
            await task;
            var result = task.Result;
            var go = GameObject.Instantiate(result);
            go.transform.SetParent(parent);
            return go;
        }


        public async Task<T> LoadAsync<T>(string name) where T : class
        {
            if (_cachedResources.ContainsKey(name))
            {
                return _cachedResources[name].Result as T;
            }
            if (_handleResources.ContainsKey(name))
            {
                Debug.Log($"{name} в процессе загрузки");
                return _handleResources[name].Result as T;
            }
            var locations = await Addressables.LoadResourceLocationsAsync(name).Task;
            if (locations == null || locations.Count == 0)
            {
                Debug.LogError($"{name} не найден в группах");
                return null;
            }

            Debug.Log($"Загружаю {name}");

            var handle = Addressables.LoadAssetAsync<T>(name);
            _handleResources[name] = handle;
            handle.Completed += (asset) =>
            {
                _cachedResources[name] = asset;
            };
            await handle.Task;
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                Debug.Log($"{name} загружен");
                return handle.Result;
            }
            else
            {
                Debug.LogError($"Ошибка загрузки {name}");
                return null;
            }
        }
    }
}