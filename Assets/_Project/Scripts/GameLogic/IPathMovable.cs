using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.GameLogic
{
    public interface IPathMovable
    {
        void SetPath(List<Transform> path);
        public void MoveToPath();
    }
}
