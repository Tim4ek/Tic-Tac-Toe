using UnityEngine;

namespace Company.Runtime.Gameplay {
  public class ItemIndex {
    private Vector2Int _position;

    public Vector2Int Position => _position;

    public ItemIndex(Vector2Int position) {
      _position = position;
    }
  }
}