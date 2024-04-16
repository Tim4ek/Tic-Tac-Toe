using UnityEngine;
using UnityEngine.AddressableAssets;
using VContainer;
using VContainer.Unity;

namespace Company.Runtime.Bootstrap {
  public class BootstrapScope : LifetimeScope {

    [SerializeField] private AssetReference loading;

    protected override void Configure(IContainerBuilder builder) {

      builder.RegisterComponent(loading);

      builder.RegisterEntryPoint<BootstrapFlow>();
    }
  }
}