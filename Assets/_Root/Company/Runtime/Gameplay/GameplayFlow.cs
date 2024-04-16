using Company.Runtime.Popups;
using VContainer;
using VContainer.Unity;

namespace Company.Runtime.Gameplay {
  public class GameplayFlow : IStartable, ITickable {

    private readonly GameplayManager _gameplayManager;
    private readonly GameplayPresenter _gameplayPresenter;
    private readonly IPopupsManager _popupsManager;

    [Inject]
    public GameplayFlow(GameplayManager gameplayManager, GameplayPresenter gameplayPresenter, IPopupsManager popupsManager) {
      _gameplayManager = gameplayManager;
      _gameplayPresenter = gameplayPresenter;
      _popupsManager = popupsManager;
    }

    public void Start() {
      _popupsManager.OnStart();
      _gameplayManager.OnStart();
      _gameplayPresenter.OnStart();     
    }

    public void Tick() {
      _gameplayManager.OnUpdate();
    }
  }
}