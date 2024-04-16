using Company.Runtime.Utilities;
using System.Collections.Generic;
using UnityEngine;

namespace Company.Runtime.Gameplay {
  public class StatePlay : StateMachine<GameplayManager>.BaseState {
    private int _turnCount = 0;
    private bool _isGameStart = false;
    public override void OnStart() {
      _turnCount = 0;
      _isGameStart = false;

      Owner.BoardManager.InitializeBoard(OnBoardCreated);
    }

    private void OnBoardCreated() {
      Owner.PlayerManager.InitializeGame(
          Owner.SelectPlayer1Type, Owner.SelectPlayer2Type,
          () => ChangeNextTurnAsync());
      StartGameAsync();
    }

    private void StartGameAsync() {
      Owner.PlayerManager.StartGame();
      Owner.PlayerManager.StartTurn();
      _isGameStart = true;
    }

    public override void OnUpdate() {
      if (!_isGameStart) return;

      Owner.PlayerManager.UpdateTurn();
    }

    private void ChangeNextTurnAsync() {
      _turnCount++;
      List<Vector3> matchLine = null;
      if (LevelPassProvider.CheckWinner(Owner.BoardManager.GridStates, Owner.PlayerManager.ActivePlayer.CurrentValue.SelfItemState, out matchLine)) {
        EndGame(matchLine);
        return;
      }

      if (LevelPassProvider.IsFull(Owner.BoardManager.GridStates)) {
        EndGame();
        return;
      }

      Owner.PlayerManager.EndTurn();

      Owner.PlayerManager.ChangePlayer(_turnCount);

      Owner.PlayerManager.StartTurn();
    }

    private void EndGame(List<Vector3> matchLine = null) {
      if (matchLine != null) {
        Owner.FinishLineAnimation.Play(matchLine[0], matchLine[matchLine.Count - 1], () => {
          Owner.PlayerManager.EndGame();
          StateMachine.ChangeState((int) GameplayState.Result);
        });
      } else {
        Owner.PlayerManager.EndGame();
        StateMachine.ChangeState((int) GameplayState.Result);
      }
    }
  }
}