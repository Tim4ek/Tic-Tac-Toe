using Company.Runtime.Extensions;
using Cysharp.Threading.Tasks;
using UnityEngine;
namespace Company.Runtime.Popups {
  public abstract class PopupView : MonoBehaviour {
    protected CanvasGroup _canvasGroup;
    public async UniTask InitializeAsync() {
      _canvasGroup = gameObject.GetOrAddComponent<CanvasGroup>();
      SetCanvasGroup(false);
      gameObject.SetActive(true);
      await Initialize();
    }

    protected abstract UniTask Initialize();
    public virtual void DestroyView() {
      Destroy(this.gameObject);
    }

    public virtual void ForceShow() {
      SetCanvasGroup(true);
    }

    public virtual void ForceHide() {
      SetCanvasGroup(false);
    }

    public void SetCanvasGroup(bool isVisible) {
      _canvasGroup.alpha = isVisible ? 1.0f : 0.0f;
    }
  }
}
