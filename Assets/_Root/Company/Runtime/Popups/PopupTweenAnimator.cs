using Company.Runtime.Extensions;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using UnityEngine;
namespace Company.Runtime.Popups {
  public enum AnimationMode { Default, FromToTop, FromToBottom, FromToLeft, FromToRight, Alpha }
  public class PopupTweenAnimator : MonoBehaviour {
    [SerializeField] private TweenSettings _settings;
    public async UniTask PlayShow() {
      TweenAnimation animation = TweenFabric.GetAnimation(_settings.showSettings.mode, _settings.container, _settings.showSettings.duration, true);
      await animation.PlayAsync();
    }

    public async UniTask PlayHide() {
      TweenAnimation animation = TweenFabric.GetAnimation(_settings.hideSettings.mode, _settings.container, _settings.hideSettings.duration, false);
      await animation.PlayAsync();
    }
  }

  [Serializable]
  public class TweenSettings {
    public Transform container;
    public ShowAnimationSettings showSettings;
    public HideAnimationSettings hideSettings;
  }

  [Serializable]
  public class ShowAnimationSettings {
    public AnimationMode mode;
    public float duration;
  }

  [Serializable]
  public class HideAnimationSettings {
    public AnimationMode mode;
    public float duration;
  }

  public class TweenFabric {
    public static TweenAnimation GetAnimation(AnimationMode mode, Transform container, float duration, bool isShow) {
      switch (mode) {
        case AnimationMode.FromToTop:
          return new TopTweenAnimation(container, duration, isShow);
        case AnimationMode.FromToBottom:
          return new BottomTweenAnimation(container, duration, isShow);
        case AnimationMode.FromToLeft:
          return new LeftTweenAnimation(container, duration, isShow);
        case AnimationMode.FromToRight:
          return new RightTweenAnimation(container, duration, isShow);
        case AnimationMode.Alpha:
          return new AlphaTweenAnimation(container, duration, isShow);
        default:
          return new TopTweenAnimation(container, duration, isShow);
      }
    }
  }

  public abstract class TweenAnimation {
    protected const string _tweenID = "PopupTween";
    protected Transform _container;
    protected float _duration;
    protected bool _isShow;

    public TweenAnimation(Transform container, float duration, bool isShow) {
      _container = container;
      _duration = duration;
      _isShow = isShow;
    }

    public abstract UniTask PlayAsync();
    public abstract void BeforePlay();
    public abstract void AfterPlay();
  }

  public class TopTweenAnimation : TweenAnimation {
    private Vector3 _startPosition = new Vector3(0f, 1375f, 0f);
    public TopTweenAnimation(Transform container, float duration, bool isShow) : base(container, duration, isShow) { }

    public override async UniTask PlayAsync() {
      BeforePlay();
      await Play();
      AfterPlay();
    }

    public override void BeforePlay() {
      if (_isShow) {
        _container.transform.localPosition = _startPosition;
      } else {
        _container.transform.localPosition = Vector3.zero;
      }
    }

    private async UniTask Play() {
      Vector3 destination;
      if (_isShow) {
        destination = Vector3.zero;
      } else {
        destination = _startPosition;
      }
      Sequence anim = DOTween.Sequence(_tweenID)
         .Append(_container.DOLocalMove(destination, _duration))
         .SetEase(Ease.Flash);
      await anim.Play().ToUniTask();
    }

    public override void AfterPlay() {
      if (_isShow) {
        _container.transform.localPosition = Vector3.zero;
      } else {
        _container.transform.localPosition = _startPosition;
      }

      DOTween.Kill(_tweenID);
    }
  }

  public class BottomTweenAnimation : TweenAnimation {
    private Vector3 _startPosition = new Vector3(0f, -1375f, 0f);
    public BottomTweenAnimation(Transform container, float duration, bool isShow) : base(container, duration, isShow) { }

    public override async UniTask PlayAsync() {
      BeforePlay();
      await Play();
      AfterPlay();
    }

    public override void BeforePlay() {
      if (_isShow) {
        _container.transform.localPosition = _startPosition;
      } else {
        _container.transform.localPosition = Vector3.zero;
      }
    }

    private async UniTask Play() {
      Vector3 destination;
      if (_isShow) {
        destination = Vector3.zero;
      } else {
        destination = _startPosition;
      }
      Sequence anim = DOTween.Sequence(_tweenID)
         .Append(_container.DOLocalMove(destination, _duration))
         .SetEase(Ease.Flash);
      await anim.Play().ToUniTask();
    }

    public override void AfterPlay() {
      if (_isShow) {
        _container.transform.localPosition = Vector3.zero;
      } else {
        _container.transform.localPosition = _startPosition;
      }

      DOTween.Kill(_tweenID);
    }
  }

  public class LeftTweenAnimation : TweenAnimation {
    private Vector3 _startPosition = new Vector3(-1400f, 0f, 0f);
    public LeftTweenAnimation(Transform container, float duration, bool isShow) : base(container, duration, isShow) { }

    public override async UniTask PlayAsync() {
      BeforePlay();
      await Play();
      AfterPlay();
    }

    public override void BeforePlay() {
      if (_isShow) {
        _container.transform.localPosition = _startPosition;
      } else {
        _container.transform.localPosition = Vector3.zero;
      }
    }

    private async UniTask Play() {
      Debug.Log($"isShow:{_isShow} duration:{_duration}");
      Vector3 destination;
      if (_isShow) {
        destination = Vector3.zero;
      } else {
        destination = _startPosition;
      }
      Sequence anim = DOTween.Sequence(_tweenID)
         .Append(_container.DOLocalMove(destination, _duration))
         .SetEase(Ease.Flash);
      await anim.Play().ToUniTask();
    }

    public override void AfterPlay() {
      if (_isShow) {
        _container.transform.localPosition = Vector3.zero;
      } else {
        _container.transform.localPosition = _startPosition;
      }

      DOTween.Kill(_tweenID);
    }
  }

  public class RightTweenAnimation : TweenAnimation {
    private Vector3 _startPosition = new Vector3(1400f, 0f, 0f);
    public RightTweenAnimation(Transform container, float duration, bool isShow) : base(container, duration, isShow) { }

    public override async UniTask PlayAsync() {
      BeforePlay();
      await Play();
      AfterPlay();
    }

    public override void BeforePlay() {
      if (_isShow) {
        _container.transform.localPosition = _startPosition;
      } else {
        _container.transform.localPosition = Vector3.zero;
      }
    }

    private async UniTask Play() {
      Debug.Log($"isShow:{_isShow} duration:{_duration}");
      Vector3 destination;
      if (_isShow) {
        destination = Vector3.zero;
      } else {
        destination = _startPosition;
      }
      Sequence anim = DOTween.Sequence(_tweenID)
         .Append(_container.DOLocalMove(destination, _duration))
         .SetEase(Ease.Flash);
      await anim.Play().ToUniTask();
    }

    public override void AfterPlay() {
      if (_isShow) {
        _container.transform.localPosition = Vector3.zero;
      } else {
        _container.transform.localPosition = _startPosition;
      }

      DOTween.Kill(_tweenID);
    }
  }

  public class AlphaTweenAnimation : TweenAnimation {
    private CanvasGroup _canvasGroup;
    public AlphaTweenAnimation(Transform container, float duration, bool isShow) : base(container, duration, isShow) {
      _canvasGroup = _container.GetOrAddComponent<CanvasGroup>();
    }

    public override async UniTask PlayAsync() {
      BeforePlay();
      await Play();
      AfterPlay();
    }

    public override void BeforePlay() {
      if (_isShow) {
        SetCanvasGroup(false);
      } else {
        SetCanvasGroup(true);
      }
    }

    private async UniTask Play() {
      Debug.Log($"isShow:{_isShow} duration:{_duration}");
      float value;
      if (_isShow) {
        value = 1.0f;
      } else {
        value = 0.0f;
      }
      Sequence anim = DOTween.Sequence(_tweenID)
         .Append(_canvasGroup.DOFade(value, _duration))
         .SetEase(Ease.Flash);
      await anim.Play().ToUniTask();
    }

    public override void AfterPlay() {
      if (_isShow) {
        SetCanvasGroup(true);
      } else {
        SetCanvasGroup(false);
      }

      DOTween.Kill(_tweenID);
    }

    public void SetCanvasGroup(bool isVisible) {
      _canvasGroup.alpha = isVisible ? 1.0f : 0.0f;
    }
  }
}