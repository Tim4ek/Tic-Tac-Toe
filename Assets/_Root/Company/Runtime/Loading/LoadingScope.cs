using UnityEngine;
using UnityEngine.AddressableAssets;
using VContainer;
using VContainer.Unity;

namespace Company.Runtime.Loading {
  public class LoadingScope : LifetimeScope {
    [SerializeField] private LoadingBarView loadingBarView;
    protected override void Configure(IContainerBuilder builder) {

      builder.RegisterComponent(loadingBarView);
      builder.Register<LoadingBarPresenter>(Lifetime.Singleton);

      builder.RegisterEntryPoint<LoadingFlow>();
    }
  }
}