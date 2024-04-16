namespace Company.Runtime.Gameplay {
  using UnityEngine;
  using UnityEngine.EventSystems;
  public class PointerEventArgs : System.EventArgs {
    public PointerEventArgs(PointerEventData.InputButton button, Vector3 worldPosition) {
      Button = button;
      WorldPosition = worldPosition;
    }

    public Vector3 WorldPosition { get; }
    public PointerEventData.InputButton Button { get; }
  }
}