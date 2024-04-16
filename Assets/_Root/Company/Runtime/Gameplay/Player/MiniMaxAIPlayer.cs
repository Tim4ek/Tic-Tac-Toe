using System;
using System.Threading;
using UnityEngine;

namespace Company.Runtime.Gameplay {
  public class MiniMaxAIPlayer : Player {
    public MiniMaxAIPlayer(BoardManager boardManager, ItemState selfItemState, Action<ItemState, Vector2Int> putItemAction) : base(boardManager, selfItemState, putItemAction) { }

    protected override void StartThink() {
      StartThinkAsync(CancellationTokenSource.Token);
    }

    private async void StartThinkAsync(CancellationToken token) {
      int delayRandom = UnityEngine.Random.Range(200, 600);
      await WaitSelectTime(delayRandom, token);

      ItemState[,] gridsState = _boardManager.GridStates;
      //int depth = UnityEngine.Random.Range(3, 5);
      ItemIndex = await AIAlgorithm.FindBestMove(gridsState, 4, token);
    }
  }
}