using UnityEngine;
namespace Company.Runtime.Extensions {
  public static class GameObjectExtensions {
    public static T GetOrAddComponent<T>(this GameObject source) where T : Component {
      if (!source.TryGetComponent<T>(out var component)) component = source.AddComponent<T>();
      return component;
    }

    public static T GetOrAddComponent<T>(this Transform source) where T : Component {
      if (!source.TryGetComponent<T>(out var component)) component = source.gameObject.AddComponent<T>();
      return component;
    }
  }
}
