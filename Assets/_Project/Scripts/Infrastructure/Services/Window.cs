using UnityEngine;

namespace _Project.Scripts.Infrastructure.Services
{
    public abstract class Window : MonoBehaviour, IWindow
    {
        public enum WindowType
        {
            None,
            Main
        }
  
        public abstract void Init();
        public abstract void Open();
        public abstract void Close(); 
    }
}