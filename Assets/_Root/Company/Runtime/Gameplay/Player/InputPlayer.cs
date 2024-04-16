using System;
using UnityEngine;

namespace Company.Runtime.Gameplay {
  public class InputPlayer : Player {
    private readonly IInputEventProvider _inputEventProvider;

    public InputPlayer(BoardManager boardManager, ItemState selfItemState, Action<ItemState, Vector2Int> putItemAction, IInputEventProvider inputEventProvider) : base(boardManager, selfItemState, putItemAction) {
      _inputEventProvider = inputEventProvider;      
    }

    protected override void StartThink() {
      _inputEventProvider.OnClick -= OnClick;
      _inputEventProvider.OnClick += OnClick;
    }

    private void OnClick(Vector3 lastDownPos, bool isDoubleClick, bool isLongTap) {
      ItemState itemState = _boardManager.GetItemStateByWorldPosition(lastDownPos, out Vector2Int gridPosition);
      if (itemState == ItemState.Empty) {
        _inputEventProvider.OnClick -= OnClick;
        ItemIndex = new ItemIndex(gridPosition);
      }
    }

    public override bool IsInputPlayer() {
      return true;
    }

    public override void Dispose() {
      Debug.Log("Player remove listeners");
      _inputEventProvider.OnClick -= OnClick;
      base.Dispose();
    }
  }
}