using Company.Runtime.Popups;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Company.Runtime.Gameplay {
  public class GameplayScope : LifetimeScope {

    [SerializeField] private Camera mainCamera;
    [SerializeField] private Grid grid;
    [SerializeField] private FinishLineAnimation finishLineAnimation;
    [SerializeField] private GameplayPresenter gameplayPresenter;   
    [SerializeField] private Transform boardParent;
    [SerializeField] private Transform _mainPopupCanvasContainer;
    [SerializeField] private Transform _background;

    protected override void Configure(IContainerBuilder builder) {

      builder.Register<GameplayManager>(Lifetime.Scoped);
      builder.Register<BoardManager>(Lifetime.Scoped);
      builder.Register<PlayerManager>(Lifetime.Scoped);
      builder.Register<IInputEventProvider, InputEventProvider>(Lifetime.Scoped);
      builder.Register<PlayerFactory>(Lifetime.Scoped);
      builder.Register<CameraScaler>(Lifetime.Scoped).WithParameter(_background);
      builder.Register<IPopupsManager, PopupsManager>(Lifetime.Scoped).WithParameter(_mainPopupCanvasContainer);

      builder.RegisterComponent(mainCamera);
      builder.RegisterComponent(grid);
      builder.RegisterComponent(gameplayPresenter);
      builder.RegisterComponent(boardParent);
      builder.RegisterComponent(finishLineAnimation);

      builder.RegisterEntryPoint<GameplayFlow>();
    }
  }
}