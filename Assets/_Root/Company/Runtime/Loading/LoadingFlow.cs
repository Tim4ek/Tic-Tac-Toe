using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using VContainer;
using VContainer.Unity;

namespace Company.Runtime.Loading {
  public class LoadingFlow : IStartable, ITickable {
    private readonly LoadingBarPresenter _loadingBarPresenter;

    [Inject]
    public LoadingFlow(LoadingBarPresenter loadingBarPresenter) {
      _loadingBarPresenter = loadingBarPresenter;
    }

    public void Start() {
      _loadingBarPresenter.OnStart();
      LoadScene();
    }

    public void Tick() {
      _loadingBarPresenter.OnUpdate();
    }

    private async void LoadScene() {
      Debug.Log("LoadingFlow | LoadScene");
      await UniTask.WaitUntil(() => _loadingBarPresenter.IsLoadingCompleted);
      Debug.Log("LoadingFlow | LoadScene | after loaded bar");
      await Addressables.LoadSceneAsync(RuntimeConstants.PERSISTENT_SCENE);
      Debug.Log("LoadingFlow | LoadScene | after loaded persistent scene");
    }
  }
}