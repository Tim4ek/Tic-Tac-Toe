
using Company.Runtime.Gameplay;
using Company.Runtime.Persistent;
using Company.Runtime.Utilities;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;
using VContainer;

namespace Company.Runtime.Meta {
  public class MetaManager {
    private readonly SceneLoader _sceneLoader;
    private readonly IServicesManager _servicesManager;
    private readonly StateMachine<MetaManager> _stateMachine;
    private readonly GameplaySettings _gameplaySettings;

    private readonly ReactiveProperty<MetaTabType> _tab = new(MetaTabType.Main);
    public ReadOnlyReactiveProperty<MetaTabType> Tab => _tab;

    [Inject]
    public MetaManager(SceneLoader sceneLoader, IServicesManager servicesManager, GameplaySettings gameplaySettings) {
      _sceneLoader = sceneLoader;
      _servicesManager = servicesManager;
      _gameplaySettings = gameplaySettings;
    }

    public void OnStart() {
      Debug.Log($"MetaManager | OnStart");
      _tab.Value = MetaTabType.Main;
    }

    public void OnUpdate() {

    }

    public void StartToPlay(int filedSize, PlayerType botType) {
      _gameplaySettings.CellSideCount = filedSize;
      _gameplaySettings.SelectPlayer2 = botType;
      _sceneLoader.LoadSceneAsync(RuntimeConstants.GAMEPLAY_SCENE).Forget();
    }
  }
}