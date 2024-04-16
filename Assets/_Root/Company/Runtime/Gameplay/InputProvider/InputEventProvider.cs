using Company.Runtime.Utilities.MobileInput;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Company.Runtime.Gameplay {
  public class InputEventProvider : IInputEventProvider {
    private const float DRAG_DURATION_THRESHOLD = 0.01f;
    private const int MOMENTUM_SAMPLES_COUNT = 5;

    public event Action<Vector3, bool> OnStartDrag;
    public event Action<Vector3, Vector3, Vector3, Vector3> OnUpdateDrag;
    public event Action<Vector3, Vector3> OnStopDrag;
    public event Action<Vector3> OnFingerDown;
    public event Action OnFingerUp;
    public event Action<Vector3, bool, bool> OnClick;
    public event Action<float> OnLongTapUpdate;
    public event Action<Vector3, float> OnStartPinch;
    public event Action<Vector3, float, float> OnUpdatePinch;
    public event Action<PinchData> OnUpdateExtendPinch;
    public event Action OnStopPinch;

    private float clickDurationThreshold = 0.7f;

    private float doubleClickThreshold = 0.5f;

    private float tiltVerticalThreshold = 0.7f;

    private float tiltHorizontalThreshold = 0.5f;

    private float dragThreshold = 0.05f;

    private bool longTapStartsDrag;

    private float _realTimeOfLastFinderDown;
    private float _realTimeOfLastClick;
    private bool _wasFingerDownLastFrame;
    private Vector3 _lastFinger0DownPosition;
    private bool _isDragging;
    private Vector3 _dragStartPosition;
    private Vector3 _dragPreviousFramePosition;
    private Vector3 _dragStartOffset;
    private List<Vector3> _dragFinalMomentumVector;
    private float _pinchStartDistance;
    private List<Vector3> _pinchStartPositions;
    private List<Vector3> _touchPositionsLastFrame;
    private Vector3 _pinchVectorStart = Vector3.zero;
    private Vector3 _pinchVectorLastFrame = Vector3.zero;
    private float _totalFingerMovement;
    private bool _wasDraggingLastFrame;
    private bool _wasPinchingLastFrame;
    private bool _isPinching;

    private float _timeSinceDragStart;
    private bool _isClickPrevented;
    private bool _isFingerDown;


    public InputEventProvider() {
      _realTimeOfLastFinderDown = 0;
      _realTimeOfLastClick = 0;
      _lastFinger0DownPosition = Vector3.zero;
      _dragStartPosition = Vector3.zero;
      _dragPreviousFramePosition = Vector3.zero;
      _isDragging = false;
      _wasFingerDownLastFrame = false;
      _dragFinalMomentumVector = new List<Vector3>();
      _pinchStartPositions = new List<Vector3> { Vector3.zero, Vector3.zero };
      _touchPositionsLastFrame = new List<Vector3> { Vector3.zero, Vector3.zero };
      _pinchStartDistance = 1;
      _isPinching = false;
      _isClickPrevented = false;
    }

    public void OnUpdate() {
      bool pinchToDragCurrentFrame = false;

      if (true) {
        #region pinch

        if (_isPinching == false) {
          if (TouchWrapper.TouchCount == 2) {
            StartPinch();
            _isPinching = true;
          }
        } else {
          switch (TouchWrapper.TouchCount) {
            case < 2:
              StopPinch();
              _isPinching = false;
              break;
            case 2:
              UpdatePinch();
              break;
          }
        }

        #endregion

        #region drag

        if (_isPinching == false) {
          if (_wasPinchingLastFrame == false) {
            if (_wasFingerDownLastFrame && TouchWrapper.IsFingerDown) {
              if (_isDragging == false) {
                float dragDistance = GetRelativeDragDistance(TouchWrapper.Touch0.Position, _dragStartPosition);
                float dragTime = Time.realtimeSinceStartup - _realTimeOfLastFinderDown;

                bool isLongTap = dragTime > clickDurationThreshold;
                if (OnLongTapUpdate != null) {
                  float longTapProgress = 0;
                  if (Mathf.Approximately(clickDurationThreshold, 0) == false) longTapProgress = Mathf.Clamp01(dragTime / clickDurationThreshold);
                  OnLongTapUpdate.Invoke(longTapProgress);
                }

                if ((dragDistance >= dragThreshold && dragTime >= DRAG_DURATION_THRESHOLD) || (longTapStartsDrag && isLongTap)) {
                  _isDragging = true;
                  _dragStartOffset = _lastFinger0DownPosition - _dragStartPosition;
                  _dragStartPosition = _lastFinger0DownPosition;
                  _dragPreviousFramePosition = _lastFinger0DownPosition;
                  DragStart(_dragStartPosition, isLongTap);
                }
              }
            }
          } else {
            if (TouchWrapper.IsFingerDown) {
              _isDragging = true;
              _dragStartPosition = TouchWrapper.Touch0.Position;
              _dragPreviousFramePosition = TouchWrapper.Touch0.Position;
              DragStart(_dragStartPosition, false);
              pinchToDragCurrentFrame = true;
            }
          }

          if (_isDragging && TouchWrapper.IsFingerDown) {
            DragUpdate(TouchWrapper.Touch0.Position, TouchWrapper.Touch0.Position - _dragPreviousFramePosition);
            _dragPreviousFramePosition = TouchWrapper.Touch0.Position;
          }

          if (_isDragging && TouchWrapper.IsFingerDown == false) {
            _isDragging = false;
            DragStop(_lastFinger0DownPosition);
          }
        }

        #endregion

        #region click

        if (_isPinching == false && _isDragging == false && _wasPinchingLastFrame == false && _wasDraggingLastFrame == false && _isClickPrevented == false) {
          if (_wasFingerDownLastFrame == false && TouchWrapper.IsFingerDown) {
            _realTimeOfLastFinderDown = Time.realtimeSinceStartup;
            _dragStartPosition = TouchWrapper.Touch0.Position;
            _dragPreviousFramePosition = TouchWrapper.Touch0.Position;
            FingerDown(TouchWrapper.AverageTouchPos);
          }

          if (_wasFingerDownLastFrame && TouchWrapper.IsFingerDown == false) {
            float fingerDownUpDuration = Time.realtimeSinceStartup - _realTimeOfLastFinderDown;

            if (_wasDraggingLastFrame == false && _wasPinchingLastFrame == false) {
              float clickDuration = Time.realtimeSinceStartup - _realTimeOfLastClick;
              bool isDoubleClick = clickDuration < doubleClickThreshold;
              bool isLongTap = fingerDownUpDuration > clickDurationThreshold;
              if (OnClick != null) OnClick.Invoke(_lastFinger0DownPosition, isDoubleClick, isLongTap);
              _realTimeOfLastClick = Time.realtimeSinceStartup;
            }
          }
        }

        #endregion
      }

      if (_isDragging && TouchWrapper.IsFingerDown && pinchToDragCurrentFrame == false) {
        _dragFinalMomentumVector.Add(TouchWrapper.Touch0.Position - _lastFinger0DownPosition);
        if (_dragFinalMomentumVector.Count > MOMENTUM_SAMPLES_COUNT) _dragFinalMomentumVector.RemoveAt(0);
      }

      if (true) _wasFingerDownLastFrame = TouchWrapper.IsFingerDown;

      if (_wasFingerDownLastFrame) _lastFinger0DownPosition = TouchWrapper.Touch0.Position;

      _wasDraggingLastFrame = _isDragging;
      _wasPinchingLastFrame = _isPinching;

      if (TouchWrapper.TouchCount == 0) {
        _isClickPrevented = false;
        if (_isFingerDown) FingerUp();
      }
    }

    private void StartPinch() {
      _pinchStartPositions[0] = _touchPositionsLastFrame[0] = TouchWrapper.Touches[0].Position;
      _pinchStartPositions[1] = _touchPositionsLastFrame[1] = TouchWrapper.Touches[1].Position;

      _pinchStartDistance = GetPinchDistance(_pinchStartPositions[0], _pinchStartPositions[1]);
      if (OnStartPinch != null) OnStartPinch.Invoke((_pinchStartPositions[0] + _pinchStartPositions[1]) * 0.5f, _pinchStartDistance);

      _isClickPrevented = true;
      _pinchVectorStart = TouchWrapper.Touches[1].Position - TouchWrapper.Touches[0].Position;
      _pinchVectorLastFrame = _pinchVectorStart;
      _totalFingerMovement = 0;
    }

    private void UpdatePinch() {
      float pinchDistance = GetPinchDistance(TouchWrapper.Touches[0].Position, TouchWrapper.Touches[1].Position);
      var pinchVector = TouchWrapper.Touches[1].Position - TouchWrapper.Touches[0].Position;
      float pinchAngleSign = Vector3.Cross(_pinchVectorLastFrame, pinchVector).z < 0 ? -1 : 1;
      float pinchAngleDelta = 0;
      if (Mathf.Approximately(Vector3.Distance(_pinchVectorLastFrame, pinchVector), 0) == false) {
        pinchAngleDelta = Vector3.Angle(_pinchVectorLastFrame, pinchVector) * pinchAngleSign;
      }

      float pinchVectorDeltaMag = Mathf.Abs(_pinchVectorLastFrame.magnitude - pinchVector.magnitude);
      float pinchAngleDeltaNormalized = 0;
      if (Mathf.Approximately(pinchVectorDeltaMag, 0) == false) pinchAngleDeltaNormalized = pinchAngleDelta / pinchVectorDeltaMag;

      var pinchCenter = (TouchWrapper.Touches[0].Position + TouchWrapper.Touches[1].Position) * 0.5f;

      #region tilting gesture

      float pinchTiltDelta = 0;
      var touch0DeltaRelative = GetTouchPositionRelative(TouchWrapper.Touches[0].Position - _touchPositionsLastFrame[0]);
      var touch1DeltaRelative = GetTouchPositionRelative(TouchWrapper.Touches[1].Position - _touchPositionsLastFrame[1]);
      float touch0DotUp = Vector2.Dot(touch0DeltaRelative.normalized, Vector2.up);
      float touch1DotUp = Vector2.Dot(touch1DeltaRelative.normalized, Vector2.up);
      float pinchVectorDotHorizontal = Vector3.Dot(pinchVector.normalized, Vector3.right);

      float epsilon = UnityEngineInternal.MathfInternal.IsFlushToZeroEnabled
            ? UnityEngineInternal.MathfInternal.FloatMinNormal
            : UnityEngineInternal.MathfInternal.FloatMinDenormal;

      float a = Mathf.Sign(touch0DotUp);
      float b = Mathf.Sign(touch1DotUp);
      bool approximately = Math.Abs(b - a) < Math.Max(0.000001f * Math.Max(Math.Abs(a), Math.Abs(b)), epsilon * 8);

      if (approximately) {
        if (Mathf.Abs(touch0DotUp) > tiltVerticalThreshold && Mathf.Abs(touch1DotUp) > tiltVerticalThreshold) {
          if (Mathf.Abs(pinchVectorDotHorizontal) >= tiltHorizontalThreshold) pinchTiltDelta = 0.5f * (touch0DeltaRelative.y + touch1DeltaRelative.y);
        }
      }

      _totalFingerMovement += touch0DeltaRelative.magnitude + touch1DeltaRelative.magnitude;

      #endregion

      if (OnUpdatePinch != null) OnUpdatePinch.Invoke(pinchCenter, pinchDistance, _pinchStartDistance);

      if (OnUpdateExtendPinch != null) {
        OnUpdateExtendPinch.Invoke(new PinchData {
          pinchCenter = pinchCenter,
          pinchDistance = pinchDistance,
          pinchStartDistance = _pinchStartDistance,
          pinchAngleDelta = pinchAngleDelta,
          pinchAngleDeltaNormalized = pinchAngleDeltaNormalized,
          pinchTiltDelta = pinchTiltDelta,
          pinchTotalFingerMovement = _totalFingerMovement
        });
      }

      _pinchVectorLastFrame = pinchVector;
      _touchPositionsLastFrame[0] = TouchWrapper.Touches[0].Position;
      _touchPositionsLastFrame[1] = TouchWrapper.Touches[1].Position;
    }

    private float GetPinchDistance(Vector3 pos0, Vector3 pos1) {
      float distanceX = Mathf.Abs(pos0.x - pos1.x) / Screen.width;
      float distanceY = Mathf.Abs(pos0.y - pos1.y) / Screen.height;
      return Mathf.Sqrt(distanceX * distanceX + distanceY * distanceY);
    }

    private void StopPinch() {
      _dragStartOffset = Vector3.zero;
      if (OnStopPinch != null) OnStopPinch.Invoke();
    }

    private void DragStart(Vector3 position, bool isLongTap) {
      if (OnStartDrag != null) OnStartDrag.Invoke(position, isLongTap);

      _isClickPrevented = true;
      _timeSinceDragStart = 0;
      _dragFinalMomentumVector.Clear();
    }

    private void DragUpdate(Vector3 currentPosition, Vector3 delta) {
      if (OnUpdateDrag != null) {
        _timeSinceDragStart += Time.unscaledDeltaTime;
        var offset = Vector3.Lerp(Vector3.zero, _dragStartOffset, Mathf.Clamp01(_timeSinceDragStart * 10.0f));
        OnUpdateDrag.Invoke(_dragStartPosition, currentPosition, offset, delta);
      }
    }

    private void DragStop(Vector3 stopPosition) {
      if (OnStopDrag != null) {
        var momentum = Vector3.zero;
        if (_dragFinalMomentumVector.Count > 0) {
          for (var i = 0; i < _dragFinalMomentumVector.Count; ++i) momentum += _dragFinalMomentumVector[i];

          momentum /= _dragFinalMomentumVector.Count;
        }

        OnStopDrag.Invoke(stopPosition, momentum);
      }

      _dragFinalMomentumVector.Clear();
    }

    private void FingerDown(Vector3 position) {
      _isFingerDown = true;
      if (OnFingerDown != null) OnFingerDown.Invoke(position);
    }

    private void FingerUp() {
      _isFingerDown = false;
      if (OnFingerUp != null) OnFingerUp.Invoke();
    }

    private Vector3 GetTouchPositionRelative(Vector3 touchPosScreen) {
      return new Vector3(touchPosScreen.x / Screen.width, touchPosScreen.y / Screen.height, touchPosScreen.z);
    }

    private float GetRelativeDragDistance(Vector3 pos0, Vector3 pos1) {
      Vector2 dragVector = pos0 - pos1;
      float dragDistance = new Vector2(dragVector.x / Screen.width, dragVector.y / Screen.height).magnitude;
      return dragDistance;
    }
  }
}