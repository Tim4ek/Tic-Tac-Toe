using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

namespace Company.Runtime.Gameplay {
  public abstract class Player : IPlayer, IDisposable {
    protected readonly BoardManager _boardManager;
    protected readonly ItemState SelfItemState;
    ItemState IPlayer.SelfItemState => SelfItemState;

    private PlayerType _playerType;
    PlayerType IPlayer.SelfPlayerType => _playerType;

    protected ItemIndex ItemIndex;

    private readonly Action<ItemState, Vector2Int> _putItemAction;

    protected readonly CancellationTokenSource CancellationTokenSource;

    protected Player(BoardManager boardManager, ItemState selfItemState, Action<ItemState, Vector2Int> putItemAction) {
      _boardManager = boardManager;
      SelfItemState = selfItemState;
      _putItemAction = putItemAction;

      CancellationTokenSource = new CancellationTokenSource();
    }

    public virtual void Dispose() {
      CancellationTokenSource?.Cancel();
    }

    public void OnInitialize(PlayerType playerType) {
      _playerType = playerType;
    }

    public void OnStartTurn() {
      //StoneStates = stoneStates;
      //SelectStoneIndex = null;

      StartThink();
    }

    public void OnUpdateTurn() {
      UpdateThink();

      if (ItemIndex == null) {
        return;
      }
      _putItemAction(SelfItemState, ItemIndex.Position);
      ItemIndex = null;
    }

    protected virtual void StartThink() { }
    protected virtual void UpdateThink() { }

    public void OnEndGame() {
      EndGame();
    }
    protected virtual void EndGame() { }

    protected async UniTask WaitSelectTime(int waitMs, CancellationToken token) {
      await UniTask.Delay(waitMs, cancellationToken: token);
    }

    public virtual bool IsInputPlayer() {
      return false;
    }

    public void OnDestroy() {
      Dispose();
    }
  }
}
