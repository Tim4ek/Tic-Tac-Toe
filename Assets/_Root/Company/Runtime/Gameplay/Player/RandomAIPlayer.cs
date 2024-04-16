using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Company.Runtime.Gameplay {
  public class RandomAIPlayer : Player {
    public RandomAIPlayer(BoardManager boardManager, ItemState selfItemState, Action<ItemState, Vector2Int> putItemAction) : base(boardManager, selfItemState, putItemAction) { }

    protected override void StartThink() {
      StartThinkAsync(CancellationTokenSource.Token);
    }

    private async void StartThinkAsync(CancellationToken token) {
      int delayRandom = UnityEngine.Random.Range(200, 600);
      await WaitSelectTime(delayRandom, token);

      await UniTask.DelayFrame(1, cancellationToken: token);

      List<Vector2Int> canPutItems = _boardManager.GetAllCanPutItemsIndex();
      int randomIndex = UnityEngine.Random.Range(0, canPutItems.Count);
      ItemIndex = new ItemIndex(canPutItems[randomIndex]);
    }
  }
}