using _Project.Scripts.Helper;
using _Project.Scripts.StaticData;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Project.Scripts.Infrastructure.Services
{
    public class StaticData : IStaticData
    {
        public Dictionary<int, LevelStaticData> Levels  = new Dictionary<int, LevelStaticData>();
        public Dictionary<int, SceneInfo> Scenes = new Dictionary<int, SceneInfo>();
        public const string LEVELS = "StaticData/Levels";
        public const string SCENES = "StaticData/Scenes";
        public void LoadData()
        {
            Scenes = Resources.LoadAll<SceneInfo>(SCENES).ToDictionary(scene => scene.Id);
            Levels = Resources.LoadAll<LevelStaticData>(LEVELS).ToDictionary(level => level.Id);
            Debug.Log("Конфигурации загружены");
        }
        public LevelStaticData GetLevelData(int id)
        {
            if (!Levels.ContainsKey(id))
            {
                Debug.LogError($"{id} не найдено в словаре Levels");
                return Levels.Last().Value;
            }
            return Levels[id];
        }
        public SceneInfo GetScene(int id)
        {
            if (!Scenes.ContainsKey(id))
            {
                Debug.LogError($"{id} не найдено в словаре Scenes");
                return Scenes.Last().Value;
            }
            return Scenes[id];
        }
    }
}