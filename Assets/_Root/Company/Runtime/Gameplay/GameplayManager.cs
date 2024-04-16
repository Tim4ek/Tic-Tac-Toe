using Company.Runtime.Popups;
using Company.Runtime.Utilities;
using Cysharp.Threading.Tasks;
using R3;
using System;
using System.Threading;
using VContainer;

namespace Company.Runtime.Gameplay {
  public class GameplayManager : IDisposable {

    private readonly SceneLoader _sceneLoader;
    private readonly IInputEventProvider _inputEventProvider;
    private readonly BoardManager _boardManager;
    private readonly PlayerManager _playerManager;
    private readonly FinishLineAnimation _finishLineAnimation;
    private readonly GameplaySettings _gameplaySettings;
    private readonly StateMachine<GameplayManager> _stateMachine;
    private readonly IPopupsManager _popupsManager;

    private readonly LevelPassProvider _levelPassProvider;

    private readonly ReactiveProperty<GameplayState> _state = new(GameplayState.Play);

    private readonly CancellationTokenSource _cancellationTokenSource;

    private PlayerType _selectPlayer1Type;
    private PlayerType _selectPlayer2Type;

    public ReadOnlyReactiveProperty<GameplayState> State => _state;

    public FinishLineAnimation FinishLineAnimation => _finishLineAnimation;
    public BoardManager BoardManager => _boardManager;
    public PlayerManager PlayerManager => _playerManager;
    public GameplaySettings GameplaySettings => _gameplaySettings;

    public PlayerType SelectPlayer1Type => _selectPlayer1Type;
    public PlayerType SelectPlayer2Type => _selectPlayer2Type;

    public CancellationTokenSource CancellationTokenSource => _cancellationTokenSource;

    private PopupsManager MainPopupContainer => PopupsManager.Find(RuntimeConstants.MAIN_POPUP_CONTAINER);

    [Inject]
    public GameplayManager(SceneLoader sceneLoader, IInputEventProvider inputEventProvider, BoardManager boardManager, PlayerManager playerManager, GameplaySettings gameplaySettings, FinishLineAnimation finishLineAnimation) {
      _sceneLoader = sceneLoader;
      _inputEventProvider = inputEventProvider;
      _boardManager = boardManager;
      _playerManager = playerManager;
      _gameplaySettings = gameplaySettings;

      _levelPassProvider = new LevelPassProvider();
      _finishLineAnimation = finishLineAnimation;

      _stateMachine = new StateMachine<GameplayManager>(this);
      _stateMachine.SetChangeStateEvent((stateId) => {
        _state.Value = (GameplayState) Enum.ToObject(typeof(GameplayState), stateId);
      });
      _stateMachine.Add<StatePlay>((int) GameplayState.Play);
      _stateMachine.Add<StateReplay>((int) GameplayState.Replay);
      _stateMachine.Add<StateResult>((int) GameplayState.Result);

      _cancellationTokenSource = new CancellationTokenSource();
    }

    public void OnStart() {
      MainPopupContainer.Show<StartPopupPresenter>((callbackReceiver) => {
        callbackReceiver.OnAfterClose += OnAfterClose;
      });
    }

    private void OnAfterClose(object sender, AfterCloseEventArgs args) {
      args.receiver.OnAfterClose -= OnAfterClose;

      _selectPlayer1Type = _gameplaySettings.SelectPlayer1;
      _selectPlayer2Type = _gameplaySettings.SelectPlayer2;

      _stateMachine.OnStart((int) GameplayState.Play);
    }

    public void OnUpdate() {
      _inputEventProvider.OnUpdate();
      _stateMachine.OnUpdate();
    }

    public void OnBackScene() {
      _sceneLoader.LoadSceneAsync(RuntimeConstants.META_SCENE).Forget();
    }

    public void OnReplay() {
      _stateMachine.ChangeState((int) GameplayState.Replay);
    }

    public void Dispose() {
      _state.Dispose();
      _cancellationTokenSource?.Cancel();
    }
  }
}