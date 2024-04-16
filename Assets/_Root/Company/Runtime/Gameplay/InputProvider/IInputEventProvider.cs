using Company.Runtime.Utilities.MobileInput;
using System;
using UnityEngine;

namespace Company.Runtime.Gameplay {
  public interface IInputEventProvider {

    event Action<Vector3, bool> OnStartDrag;
    event Action<Vector3, Vector3, Vector3, Vector3> OnUpdateDrag;
    event Action<Vector3, Vector3> OnStopDrag;
    event Action<Vector3> OnFingerDown;
    event Action OnFingerUp;
    event Action<Vector3, bool, bool> OnClick;
    event Action<float> OnLongTapUpdate;
    event Action<Vector3, float> OnStartPinch;
    event Action<Vector3, float, float> OnUpdatePinch;
    event Action<PinchData> OnUpdateExtendPinch;
    event Action OnStopPinch;

    void OnUpdate();
  }
}
