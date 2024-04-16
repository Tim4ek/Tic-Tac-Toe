using UnityEngine;
namespace Company.Runtime.Gameplay {
  public class GamePlayerInfo : MonoBehaviour {
    [SerializeField] private Transform selectTransform;
    public void SetActiveColor(bool isActive) {
      selectTransform.gameObject.SetActive(isActive);
    }
  }
}