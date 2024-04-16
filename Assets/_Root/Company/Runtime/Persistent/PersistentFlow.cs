using Company.Runtime.Popups;
using Company.Runtime.Utilities;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Company.Runtime.Persistent {
  public class PersistentFlow : IStartable, ITickable {
    private readonly SceneLoader _sceneLoader;
    private readonly IServicesManager _servicesManager;
    private readonly IPopupsManager _popupsManager;

    [Inject]
    public PersistentFlow(SceneLoader sceneLoader, IServicesManager servicesManager, IPopupsManager popupsManager) {
      _sceneLoader = sceneLoader;
      _servicesManager = servicesManager;
      _popupsManager = popupsManager;
    }

    public void Start() {
      Debug.Log("PersistentFlow | Start | _servicesManager on start");
      _servicesManager.OnStart();
      Debug.Log("PersistentFlow | Start | _popupsManager on start");
      _popupsManager.OnStart();
      Debug.Log("PersistentFlow | Start | start load Meta_Scene");
      _sceneLoader.LoadSceneAsync(RuntimeConstants.META_SCENE).Forget();
    }

    public void Tick() {
      _servicesManager.OnUpdate();
    }
  }
}