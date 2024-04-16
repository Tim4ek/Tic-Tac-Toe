using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using VContainer;
using VContainer.Unity;

namespace Company.Runtime.Bootstrap {
  public class BootstrapFlow : IStartable {
    private readonly AssetReference _loadingScene;

    [Inject]
    public BootstrapFlow(AssetReference loadingScene) {
      _loadingScene = loadingScene;
    }

    public async void Start() {
      await Addressables.LoadSceneAsync(_loadingScene);
    }
  }
}