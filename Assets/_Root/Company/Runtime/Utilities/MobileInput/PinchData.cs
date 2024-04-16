using UnityEngine;
namespace Company.Runtime.Utilities.MobileInput {
  public class PinchData {
    public Vector3 pinchCenter;
    public float pinchDistance;
    public float pinchStartDistance;
    public float pinchAngleDelta;
    public float pinchAngleDeltaNormalized;
    public float pinchTiltDelta;
    public float pinchTotalFingerMovement;
  }
}