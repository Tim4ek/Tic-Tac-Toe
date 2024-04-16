using Company.Runtime.Persistent;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using VContainer;
using VContainer.Unity;

namespace Company.Runtime.Utilities {
  public class SceneLoader {
    private readonly PersistentScope _persistentScope;
    private Dictionary<string, SceneInstance> _sceneHolder = new Dictionary<string, SceneInstance>();

    [Inject]
    public SceneLoader(PersistentScope persistentScope) {
      _persistentScope = persistentScope;
    }

    public async UniTask LoadSceneAsync(string sceneName) {
      UnloadScene();
      using (LifetimeScope.EnqueueParent(_persistentScope)) {
        SceneInstance scene = await Addressables.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        string sceneName2 = scene.Scene.name;
        _sceneHolder.Add(sceneName2, scene);
        Debug.Log($"SceneLoader | SetActiveScene | name {sceneName}");
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
      }
    }

    private void UnloadScene() {
      foreach (var scene in GetAllLoadedScene()) {
        if (!scene.name.Equals(RuntimeConstants.PERSISTENT_SCENE)) {
          Addressables.UnloadSceneAsync(_sceneHolder[scene.name]);
          _sceneHolder.Remove(scene.name);
        }
      }
    }

    private Scene[] GetAllLoadedScene() {
      int countLoaded = SceneManager.sceneCount;
      var loadedScenes = new Scene[countLoaded];

      for (var i = 0; i < countLoaded; i++) {
        loadedScenes[i] = SceneManager.GetSceneAt(i);
      }

      return loadedScenes;
    }
  }
}