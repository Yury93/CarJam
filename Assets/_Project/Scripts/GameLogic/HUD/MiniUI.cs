using _Project.Scripts.Infrastructure.States;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.GameLogic
{

    public class MiniUI : MonoBehaviour
    {
        public TextMeshProUGUI queuePersonText;
        public Button closeGameButton;
        private IStateMachine _stateMachine;
        public static MiniUI instance;
        public void Construct(IStateMachine stateMachine)
        {
            this._stateMachine = stateMachine;
        }
        private void Awake()
        {
            instance = this;
            closeGameButton.onClick.AddListener(()=> _stateMachine.Enter<LoadMainState>());
        }
        public void ShowQueue(int queue)
        {
            queuePersonText.text = queue + "";
        }
        private void OnDestroy()
        {
            closeGameButton.onClick.RemoveAllListeners();
        }
    }
}