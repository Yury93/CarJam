using _Project.Scripts.Infrastructure.States;
using _Project.Scripts.Saver;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.GameLogic
{

    public class MiniUIInfo : MonoBehaviour
    {
        public TextMeshProUGUI queuePersonText;
        public Button closeGameButton;
        private IStateMachine _stateMachine;
        [SerializeField]private PersonSpawner personSpawner;
        public static MiniUIInfo instance;
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
            int result = queue + (personSpawner.PersonOnLevel.Count);
            queuePersonText.text = (result)+"";
            if(result == 0)
            {
                closeGameButton.onClick.RemoveAllListeners();

                if (Saver.Saver.GetLastLevel() == 3)
                {
                    Saver.Saver.SaveLevel(0);
                }
                else
                    Saver.Saver.SaveLevel(Saver.Saver.GetLastLevel() + 1);


                StartCoroutine(CorDelay());
                IEnumerator CorDelay()
                {
                    yield return new WaitForSeconds(1);
                    _stateMachine.Enter<LoadGameState>();
                }
            }
        }
        private void OnDestroy()
        {
            closeGameButton.onClick.RemoveAllListeners();
        }
    }
}