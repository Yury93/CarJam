using UnityEngine;

namespace _Project.Scripts.GameLogic
{
    public interface IRaycastChecker
    {
        int CheckForward();
        int LaunchRaycast(Vector3 direction, float distance);
    }
}
