using System;
using System.Collections;
using UnityEngine;
namespace Company.Runtime.Gameplay {
  public class FinishLineAnimation : MonoBehaviour {
    [SerializeField] private LineRenderer _lr;
    private const string _lrName = "lr";

    public void Play(Vector3 startPos, Vector3 endPosition, Action onComplete) {
      ResetLine();
      _lr.positionCount = 2;
      StartCoroutine(LineDraw(startPos, endPosition, onComplete));
    }

    public IEnumerator LineDraw(Vector3 startPos, Vector3 endPosition, Action onComplete) {
      float t = 0;
      float time = 0.5f;
      Vector3 orig = _lr.GetPosition(0);
      Vector3 orig2 = _lr.GetPosition(1);
      _lr.SetPosition(0, startPos);
      _lr.SetPosition(1, startPos);
      Vector3 newpos;
      for (; t < time; t += Time.deltaTime) {
        newpos = Vector3.Lerp(startPos, endPosition, t / time);
        _lr.SetPosition(1, newpos);
        yield return null;
      }
      _lr.SetPosition(1, endPosition);
      onComplete?.Invoke();
      ResetLine();
    }

    private void ResetLine() {
      _lr.positionCount = 0;
    }
  }
}