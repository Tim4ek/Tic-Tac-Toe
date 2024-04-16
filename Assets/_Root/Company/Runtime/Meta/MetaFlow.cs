using Company.Runtime.Popups;
using VContainer;
using VContainer.Unity;

namespace Company.Runtime.Meta {
  public class MetaFlow : IStartable, ITickable {
    private readonly MetaManager _metaManager;
    private readonly MetaPresenter _metaPresenter;
    private readonly IPopupsManager _popupsManager;

    [Inject]
    public MetaFlow(MetaManager metaManager, MetaPresenter metaPresenter, IPopupsManager popupsManager) {
      _metaManager = metaManager;
      _metaPresenter = metaPresenter;
      _popupsManager = popupsManager;
    }

    public void Start() {
      _metaManager.OnStart();
      _metaPresenter.OnStart();
      _popupsManager.OnStart();
    }

    public void Tick() {
      _metaManager.OnUpdate();
    }
  }
}