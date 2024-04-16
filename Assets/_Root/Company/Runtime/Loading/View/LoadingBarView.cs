using System;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Company.Runtime.Loading {
  public class LoadingBarView : MonoBehaviour {
    [SerializeField, Range(0f, 3f)] private float duration;
    [SerializeField] private Slider loadingBar;
    [SerializeField] private TextMeshProUGUI localeTxtPercent;

    private float _currentTimeLoading;

    public void OnStart() {
      loadingBar.value = 0;
      _currentTimeLoading = 0;
    }

    public void OnFill() {
      if (loadingBar.value < 0.4f) {
        loadingBar.value += 1 / duration / 3 * Time.deltaTime;
        _currentTimeLoading += Time.deltaTime / 3f;
      } else {
        loadingBar.value += 1 / duration * Time.deltaTime;
        _currentTimeLoading += Time.deltaTime;
      }
      float value = (float) System.Math.Round((loadingBar.value * 100));
      localeTxtPercent.text = $"Loading {value.ToString(CultureInfo.InvariantCulture)}%";
    }



    public bool IsFilled() {
      if (_currentTimeLoading >= duration) {
        return true;
      }
      return false;
    }
  }
}
