using System;
using UnityEngine;

namespace Company.Runtime.Gameplay {
  public class PlayerFactory {
    private readonly IInputEventProvider _inputEventProvider;
    private readonly BoardManager _boardManager;

    public PlayerFactory(IInputEventProvider inputEventProvider, BoardManager boardManager) {
      _inputEventProvider = inputEventProvider;
      _boardManager = boardManager;
    }

    public IPlayer CreatePlayer(PlayerType playerType, ItemState itemState, Action<ItemState, Vector2Int> putItemAction) {
      IPlayer player = null;
      switch (playerType) {
        case PlayerType.InputPlayer:
          player = new InputPlayer(_boardManager, itemState, putItemAction, _inputEventProvider);
          break;
        case PlayerType.RandomAIPlayer:
          player = new RandomAIPlayer(_boardManager, itemState, putItemAction);
          break;
        case PlayerType.MiniMaxAIPlayer:
          player = new MiniMaxAIPlayer(_boardManager, itemState, putItemAction);
          break;
      }
      return player;
    }
  }
}