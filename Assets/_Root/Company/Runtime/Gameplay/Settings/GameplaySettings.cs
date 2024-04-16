using System;
using System.Collections.Generic;
using UnityEngine;

namespace Company.Runtime.Gameplay {

  [CreateAssetMenu(fileName = "GameplaySettings", menuName = "Company/GameplaySettings")]
  public class GameplaySettings : ScriptableObject, ISerializationCallbackReceiver {

    [SerializeField] private int _cellSideCount;
    [NonSerialized] public int CellSideCount;
    [Space]
    [SerializeField] private PlayerType _selectPlayer1;
    [SerializeField] private PlayerType _selectPlayer2;
    [NonSerialized] public PlayerType SelectPlayer1;
    [NonSerialized] public PlayerType SelectPlayer2;
    [Space]
    [SerializeField] private List<ItemSettings> itemsSettings;


    public void OnBeforeSerialize() { }
    public void OnAfterDeserialize() {
      SelectPlayer1 = _selectPlayer1;
      SelectPlayer2 = _selectPlayer2;

      CellSideCount = _cellSideCount;
    }

    public Sprite GetSpriteByItemState(ItemState itemState) {
      if (itemsSettings == null || itemsSettings.Count == 0) {
        return null;
      }
      for (int i = 0; i < itemsSettings.Count; i++) {
        if (itemsSettings[i].itemState == itemState) {
          return itemsSettings[i].sprite;
        }
      }
      return null;
    }
  }

  [Serializable]
  public class ItemSettings {
    public ItemState itemState;
    public Sprite sprite;
  }
}