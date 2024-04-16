using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Company.Runtime.Meta {
  public class TapToPlayButton : MonoBehaviour {
    [SerializeField] private Button tapToPlayButton;

    public void SetActive(bool isActive) {
      gameObject.SetActive(isActive);
    }

    public void SetActiveTapToPlayButton(bool isActive) {
      tapToPlayButton.gameObject.SetActive(isActive);
    }

    public void SetListenerBackButton(UnityAction action) {
      tapToPlayButton.onClick.RemoveAllListeners();
      tapToPlayButton.onClick.AddListener(action);
    }
  }
}