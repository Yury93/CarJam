using _Project.Scripts.StaticData;

namespace _Project.Scripts.Infrastructure.Services
{
    public interface IStaticData : IService
    {
        public void LoadData(); 
        public LevelStaticData GetLevelData(int id);
        public SceneInfo GetScene(int idScene);
    }
}