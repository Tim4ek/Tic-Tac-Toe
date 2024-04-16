using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Company.Runtime.Gameplay {
  public static class AIAlgorithm {
    public static async UniTask<ItemIndex> FindBestMove(ItemState[,] gridsState, int depth, CancellationToken token) {
      await UniTask.SwitchToThreadPool();
      int bestScore = int.MaxValue;
      ItemIndex bestMove = null;

      List<Vector2Int> availableMoves = BoardManager.GetAvailableMoveByState(gridsState);
      foreach (Vector2Int move in availableMoves) {
        gridsState[move.x, move.y] = ItemState.O;
        int score = NegaAlpha(gridsState, depth - 1, int.MinValue, int.MaxValue, true);
        gridsState[move.x, move.y] = ItemState.Empty;
        if (score <= bestScore) {
          bestScore = score;
          bestMove = new ItemIndex(move);
        }
      }
      await UniTask.SwitchToMainThread();
      return bestMove;
    }

    private static int NegaAlpha(ItemState[,] gridsState, int depth, int alpha, int beta, bool maximizingPlayer) {
      if (depth == 0) {
        return Evaluate(gridsState);
      }
      List<Vector3> matchLine = null;
      if (LevelPassProvider.CheckWinner(gridsState, ItemState.X, out matchLine)) {
        return 1;
      }
      if (LevelPassProvider.CheckWinner(gridsState, ItemState.O, out matchLine)) {
        return -1;
      }
      if (LevelPassProvider.IsFull(gridsState)) {
        return 0;
      }
      List<Vector2Int> availableMoves = BoardManager.GetAvailableMoveByState(gridsState);
      if (maximizingPlayer) {
        int maxEval = int.MinValue;
        foreach (Vector2Int move in availableMoves) {
          gridsState[move.x, move.y] = ItemState.X;
          int eval = NegaAlpha(gridsState, depth - 1, alpha, beta, false);
          gridsState[move.x, move.y] = ItemState.Empty;
          maxEval = Math.Max(maxEval, eval);
          alpha = Math.Max(alpha, eval);
          if (beta <= alpha) {
            break;
          }
        }
        return maxEval;
      } else {
        int minEval = int.MaxValue;
        foreach (Vector2Int move in availableMoves) {
          gridsState[move.x, move.y] = ItemState.O;
          int eval = NegaAlpha(gridsState, depth - 1, alpha, beta, true);
          gridsState[move.x, move.y] = ItemState.Empty;
          minEval = Math.Min(minEval, eval);
          beta = Math.Min(beta, eval);
          if (beta <= alpha) {
            break;
          }
        }
        return minEval;
      }
    }

    private static int Evaluate(ItemState[,] gridsState) {
      List<Vector3> matchLine = null;
      if (LevelPassProvider.CheckWinner(gridsState, ItemState.X, out matchLine)) {
        return 1;
      } else if (LevelPassProvider.CheckWinner(gridsState, ItemState.O, out matchLine)) {
        return -1;
      } else if (LevelPassProvider.IsFull(gridsState)) {
        return 0;
      }
      return 0;
    }
  }
}
