using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Company.Runtime.Popups {
  public abstract class PopupPresenter<TView> : PopupPresenter where TView : PopupView {
    protected TView _viewPrefab;
    protected TView _popupView;
    protected PopupTweenAnimator _animator;

    public override void Instantiate(Transform parentContainer) {
      _popupView = Object.Instantiate(_viewPrefab, parentContainer, false);
      _animator = _popupView.GetComponent<PopupTweenAnimator>();
      InvokeOnBeforeOpen();
    }

    public override async UniTask PlayShowAnimationAsync() {
      _popupView.SetCanvasGroup(true);
      await _animator.PlayShow();
      InvokeOnAfterOpen();
    }

    public override async UniTask PlayHideAnimationAsync() {
      await _animator.PlayHide();
      InvokeOnAfterClose();
    }

    public override void ForceShow() {
      _popupView.ForceShow();
      InvokeOnAfterOpen();
    }

    public override void ForceHide() {
      _popupView.ForceHide();
      InvokeOnAfterClose();
    }

    public override bool IsPlayAnimation() {
      return _animator != null;
    }
  }
}