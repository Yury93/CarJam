using UnityEngine;

namespace _Project.Scripts.Infrastructure.Services
{
    public interface IAssetProvider : IService
    {
        GameObject instatiate(string path);
        GameObject instatiate(string path,Vector3 position);
        GameObject instatiate(string path, Transform parent);
    }
}