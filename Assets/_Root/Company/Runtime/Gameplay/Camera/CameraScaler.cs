using System;
using UnityEngine;
using VContainer;

namespace Company.Runtime.Gameplay {

  public class CameraScaler {
    private Transform _background;
    private float _buffer = 1f;

    private Camera _camera;

    [Inject]
    public CameraScaler(Camera camera, Transform background) {
      _camera = camera;
      _background = background;
    }

    public void UpdateSizeAndPosition(Bounds bounds) {
      var (center, size) = CalculateOrthoSize(bounds);
      _camera.transform.position = center;
      _camera.orthographicSize = size;

      _background.position = new Vector3(center.x, center.y, 0f)
        ;
    }

    private (Vector3 center, float size) CalculateOrthoSize(Bounds bounds) {

      bounds.Expand(_buffer);

      float vertical = bounds.size.y;
      float horizontal = bounds.size.x * _camera.pixelHeight / _camera.pixelWidth;

      float size = Mathf.Max(horizontal, vertical) * 0.5f;
      Vector3 center = bounds.center + new Vector3(0, 0, -10);

      return (center, size);
    }

    public Vector3 ScreenToWorldPoint(Vector3 point) {
      return _camera.ScreenToWorldPoint(point);
    }
  }
}