
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Company.Runtime.Persistent.Services {
  public class VibrationService : IVibrationService {
    private bool _isVibration = false;
    private bool _isActive = true;
    public bool IsActive => _isActive;

    public void Init() {

    }

    public void OnUpdate() {
      if (_isVibration == true && _isActive) {
        Handheld.Vibrate();
      }
    }

    public void SetState(bool isActive) {
      _isActive = isActive;
    }

    public void PlayVibration(float time) {
      if (!_isActive) {
        return;
      }
      VibrationTime(time);
    }

    public void PlayVibration(bool isPressed) {
      if (!_isActive) {
        return;
      }
      _isVibration = isPressed;
    }

    public void PlayVibration() {
      UnityEngine.Debug.Log("PlayVibration");
      if (!_isActive) {
        return;
      }
      Handheld.Vibrate();
    }

    private async UniTask VibrationTime(float time) {
      _isVibration = true;
      await UniTask.WaitForSeconds(time);
      _isVibration = false;
    }
  }
}