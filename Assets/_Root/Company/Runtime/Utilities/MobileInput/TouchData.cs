using UnityEngine;

namespace Company.Runtime.Utilities.MobileInput {
  public class TouchData {
    public Vector3 Position { get; set; }
    public int FingerId { get; set; } = -1;

    public static TouchData From(Touch touch) => new TouchData { Position = touch.position, FingerId = touch.fingerId };
  }
}