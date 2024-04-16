using Company.Runtime.Gameplay;
using Company.Runtime.Popups;
using Company.Runtime.Utilities;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Company.Runtime.Persistent {
  public class PersistentScope : LifetimeScope {

    [SerializeField] private Transform _masterCanvasContainer;
    [SerializeField] private GameplaySettings gameplaySettings;

    protected override void Configure(IContainerBuilder builder) {

      builder.Register<SceneLoader>(Lifetime.Singleton);
      builder.Register<IServicesManager, ServicesManager>(Lifetime.Singleton);
      builder.Register<IPopupsManager, PopupsManager>(Lifetime.Scoped).WithParameter(_masterCanvasContainer);

      builder.RegisterComponent(_masterCanvasContainer);
      builder.RegisterComponent(gameplaySettings);

      builder.RegisterEntryPoint<PersistentFlow>();
    }
  }
}