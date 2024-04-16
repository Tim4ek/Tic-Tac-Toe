using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Company.Runtime.Gameplay {
  public class GameTopPanelView : MonoBehaviour {
    [SerializeField] private GamePlayerInfo xPlayerInfo;
    [SerializeField] private GamePlayerInfo oPlayerInfo;

    [SerializeField] private Button backButton;
    [SerializeField] private Button replayButton;

    public void SetActive(bool isActive) {
      gameObject.SetActive(isActive);
    }

    public void ChangeActiveColor(ItemState activeItemState) {
      xPlayerInfo.SetActiveColor(false);
      oPlayerInfo.SetActiveColor(false);

      if (activeItemState == ItemState.X) {
        xPlayerInfo.SetActiveColor(true);
      } else if (activeItemState == ItemState.O) {
        oPlayerInfo.SetActiveColor(true);
      }
    }

    public void SetListenerBackButton(UnityAction action) {
      backButton.onClick.RemoveAllListeners();
      backButton.onClick.AddListener(action);
    }

    public void SetListenerReplayButton(UnityAction action) {
      replayButton.onClick.RemoveAllListeners();
      replayButton.onClick.AddListener(action);
    }
  }
}
