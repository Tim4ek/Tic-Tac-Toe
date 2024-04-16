using Company.Runtime.Popups;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Company.Runtime.Meta {
  public class MetaScope : LifetimeScope {
    [SerializeField] private MetaPresenter metaPresenter;

    [SerializeField] private Transform _mainPopupCanvasContainer;    

    protected override void Configure(IContainerBuilder builder) {

      builder.Register<MetaManager>(Lifetime.Scoped);
      builder.Register<IPopupsManager, PopupsManager>(Lifetime.Scoped).WithParameter(_mainPopupCanvasContainer);

      builder.RegisterComponent(metaPresenter);

      builder.RegisterEntryPoint<MetaFlow>();
    }
  }
}