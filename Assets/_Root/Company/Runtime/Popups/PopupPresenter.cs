using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Company.Runtime.Popups {
  public abstract class PopupPresenter : ICallbackReceiver {

    protected AsyncOperationHandle<GameObject> _handle;

    public event EventHandler<BeforeOpenEventArgs> OnBeforeOpen;
    public event EventHandler<AfterOpenEventArgs> OnAfterOpen;
    public event EventHandler<BeforeCloseEventArgs> OnBeforeClose;
    public event EventHandler<AfterCloseEventArgs> OnAfterClose;

    public abstract UniTask LoadPrefabAsync();
    public abstract void Instantiate(Transform parentContainer);
    public virtual async UniTask InitializeAsync() {
      await Initialize();
    }
    protected abstract UniTask Initialize();
    public abstract bool IsPlayAnimation();
    public abstract UniTask PlayShowAnimationAsync();
    public abstract UniTask PlayHideAnimationAsync();
    public abstract void ForceShow();
    public abstract void ForceHide();
    public abstract void AfterOpen();
    public virtual UniTask BeforeClose() {
      InvokeOnBeforeClose();
      return UniTask.CompletedTask;
    }
    public abstract void AfterClose();

    protected void InvokeOnBeforeOpen() {
      BeforeOpenEventArgs args = new BeforeOpenEventArgs();
      args.receiver = this;
      OnBeforeOpen?.Invoke(this, args);
    }

    protected void InvokeOnAfterOpen() {
      AfterOpenEventArgs args = new AfterOpenEventArgs();
      args.receiver = this;
      OnAfterOpen?.Invoke(this, args);
    }

    protected void InvokeOnBeforeClose() {
      BeforeCloseEventArgs args = new BeforeCloseEventArgs();
      args.receiver = this;
      OnBeforeClose?.Invoke(this, args);
    }

    protected void InvokeOnAfterClose() {
      AfterCloseEventArgs args = new AfterCloseEventArgs();
      args.receiver = this;
      OnAfterClose?.Invoke(this, args);
    }
  }
}
