using UnityEngine;

namespace _Project.Scripts.StaticData
{
    [CreateAssetMenu(fileName = "SceneInfo", menuName = "StaticData/SceneInfo")]
    public class SceneInfo : ScriptableObject
    {
        [field: SerializeField] public int Id { get; private set; } = 0;
        [field: SerializeField] public string SceneKey { get; private set; } = "Level";
    }
}