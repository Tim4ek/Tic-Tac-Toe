using Cysharp.Threading.Tasks;
using System;
using TMPro;
using UnityEngine;

namespace Company.Runtime.Popups {
  public class StartPopupView : PopupView {
    [SerializeField] private TextMeshProUGUI txt;

    protected override UniTask Initialize() {
      return UniTask.CompletedTask;
    }

    public void SetText(string text) {
      txt.text = text;
    }
  }
}
