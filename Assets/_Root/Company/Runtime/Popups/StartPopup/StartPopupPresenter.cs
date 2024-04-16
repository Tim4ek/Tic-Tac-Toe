using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Company.Runtime.Popups {
  public class StartPopupPresenter : PopupPresenter<StartPopupView> {
    public override async UniTask LoadPrefabAsync() {
      _handle = Addressables.LoadAssetAsync<GameObject>(RuntimeConstants.START_POPUP);
      GameObject startPopupPrefab = await _handle;
      if (startPopupPrefab.TryGetComponent(out _viewPrefab) == false) {
        Debug.LogError("Popup StartPopupView is null");
      }
      return;
    }

    protected override async UniTask Initialize() {
      _popupView.SetText("You Turn");
      await _popupView.InitializeAsync();
    }

    public override void AfterOpen() {
      WaitAndClose(600);
    }

    private async void WaitAndClose(int delay) {
      await UniTask.Delay(delay);
      PopupsManager.Close(_popupView.transform);
    }

    public override void AfterClose() {
      _popupView.DestroyView();
      Addressables.Release(_handle);
    }
  }
}