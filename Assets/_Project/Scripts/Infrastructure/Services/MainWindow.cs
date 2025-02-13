using _Project.Scripts.Infrastructure.States;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.Infrastructure.Services
{
    public class MainWindow : Window, IMainWindow
    {
        [SerializeField] private Button _startGameButton;
        private IStateMachine _stateMachine;
         
        public void Construct(IStateMachine stateMachine)
        {
             this._stateMachine = stateMachine;
        } 
        public override void Init()
        {
            _startGameButton.onClick.AddListener(LoadGame);
        }
        public override void Open()
        {
            if (!gameObject.activeSelf) gameObject.SetActive(true);
        }
        public override void Close()
        {
            if (gameObject.activeSelf) gameObject.SetActive(false);
        }
        private void LoadGame()
        {
            _startGameButton.onClick.RemoveListener(LoadGame);
            _stateMachine.Enter<LoadGameState>(); 
        } 
    }
}