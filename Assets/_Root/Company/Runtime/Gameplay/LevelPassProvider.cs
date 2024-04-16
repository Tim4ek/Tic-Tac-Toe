using System;
using System.Collections.Generic;
using UnityEngine;

namespace Company.Runtime.Gameplay {
  public class LevelPassProvider {

    public static bool CheckWinner(ItemState[,] gridsState, ItemState itemState, out List<Vector3> matchLine) {
      matchLine = null;
      bool horizontalCheck = HasMatchByHorizontal(gridsState, itemState, out matchLine);
      if (horizontalCheck) {
        return true;
      }
      bool verticalCheck = HasMatchByVertical(gridsState, itemState, out matchLine);
      if (verticalCheck) {
        return true;
      }
      bool diagonalCheck = HasMatchByDiagonal(gridsState, itemState, out matchLine);
      if (diagonalCheck) {
        return true;
      }
      return false;
    }

    private static bool HasMatchByHorizontal(ItemState[,] gridsState, ItemState itemState, out List<Vector3> matchLine) {
      matchLine = new List<Vector3>();
      int cellSideCount = gridsState.GetLength(0);
      for (int i = 0; i < cellSideCount; i++) {
        bool isMatch = false;
        for (int j = 0; j < cellSideCount; j++) {
          if (gridsState[j, i] == itemState) {
            matchLine.Add(new Vector3(j, i + 0.5f));
            isMatch = true;
          } else {
            matchLine.Clear();
            isMatch = false;
            break;
          }
        }
        if (isMatch) {
          Vector3 last = matchLine[matchLine.Count - 1];
          matchLine.Add(new Vector3(last.x + 1, last.y));
          return true;
        } else {
          continue;
        }
      }
      matchLine.Clear();
      return false;
    }

    private static bool HasMatchByVertical(ItemState[,] gridsState, ItemState itemState, out List<Vector3> matchLine) {
      matchLine = new List<Vector3>();
      int cellSideCount = gridsState.GetLength(0);
      for (int i = 0; i < cellSideCount; i++) {
        bool isMatch = false;
        for (int j = 0; j < cellSideCount; j++) {
          if (gridsState[i, j] == itemState) {
            matchLine.Add(new Vector3(i + 0.5f, j));
            isMatch = true;
          } else {
            matchLine.Clear();
            isMatch = false;
            break;
          }
        }
        if (isMatch) {
          Vector3 last = matchLine[matchLine.Count - 1];
          matchLine.Add(new Vector3(last.x, last.y + 1));
          return true;
        } else {
          continue;
        }
      }
      matchLine.Clear();
      return false;
    }

    private static bool HasMatchByDiagonal(ItemState[,] gridsState, ItemState itemState, out List<Vector3> matchLine) {
      matchLine = new List<Vector3>();
      int cellSideCount = gridsState.GetLength(0);
      bool isMatch = false;
      for (int i = 0; i < cellSideCount; i++) {
        if (gridsState[i, i] == itemState) {
          matchLine.Add(new Vector3(i, i));
          isMatch = true;
        } else {
          matchLine.Clear();
          isMatch = false;
          break;
        }
      }
      if (isMatch) {
        Vector3 last = matchLine[matchLine.Count - 1];
        matchLine.Add(new Vector3(last.x + 1, last.y + 1));
        return true;
      }
      int rowIndex = 0;
      for (int i = cellSideCount - 1; i >= 0; i--) {
        if (gridsState[i, rowIndex] == itemState) {
          matchLine.Add(new Vector3(i+1, rowIndex));
          isMatch = true;
          rowIndex++;
        } else {
          matchLine.Clear();
          isMatch = false;
          break;
        }
      }
      if (isMatch) {
        Vector3 last = matchLine[matchLine.Count - 1];
        matchLine.Add(new Vector3(last.x-1, last.y + 1));
      }
      return isMatch;
    }

    public static bool IsFull(ItemState[,] gridsState) {
      int cellSideCount = gridsState.GetLength(0);
      for (int i = 0; i < cellSideCount; i++) {
        for (int j = 0; j < cellSideCount; j++) {
          if (gridsState[i, j] == ItemState.Empty) {
            return false;
          }
        }
      }
      return true;
    }
  }
}