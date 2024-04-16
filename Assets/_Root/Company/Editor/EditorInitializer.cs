#if UNITY_EDITOR
using Company.Runtime;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace Company.Editor {
  internal static class EditorInitializer {
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static async void Init() {
      string startScene = SceneManager.GetActiveScene().name;
      switch (startScene) {
        case RuntimeConstants.LOADING_SCENE: return;
        case RuntimeConstants.PERSISTENT_SCENE:
        case RuntimeConstants.META_SCENE:
        case RuntimeConstants.GAMEPLAY_SCENE:
          await Addressables.LoadSceneAsync(RuntimeConstants.LOADING_SCENE);
          break;
      }
    }
  }
}

#endif