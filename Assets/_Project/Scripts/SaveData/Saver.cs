using UnityEngine;

namespace _Project.Scripts.Saver
{
    public class Saver
    {
        public const string LEVEL_ID = "LevelId";
        public const string SCENE_ID = "SceneId";
        public static void SaveLevel(int levelId)
        {
            PlayerPrefs.SetInt(LEVEL_ID, levelId);
        }
        public static int GetLastLevel()
        {
            return PlayerPrefs.GetInt(LEVEL_ID);
        }
        public static void SaveScene(int sceneId)
        {
            PlayerPrefs.SetInt(SCENE_ID, sceneId);
        }
        public static int GetCurrentScene()
        {
            return PlayerPrefs.GetInt(SCENE_ID);
        }
    }
}