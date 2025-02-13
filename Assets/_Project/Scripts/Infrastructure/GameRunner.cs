using UnityEngine;

namespace _Project.Scripts.Infrastructure
{
    public class GameRunner : MonoBehaviour
    {
        public Bootstrap BootstraperPrefab;
        private void Awake()
        {
            var bootstrapper = GameObject.FindAnyObjectByType<Bootstrap>();
            if (bootstrapper == null)
            {
                Instantiate(BootstraperPrefab);
            }

        }
    }
}