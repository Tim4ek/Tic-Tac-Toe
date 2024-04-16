using UnityEngine;

namespace Company.Runtime {

  public static class ApplicationInitializer {

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize() {
      Application.targetFrameRate = 60;
    }
  }
}
