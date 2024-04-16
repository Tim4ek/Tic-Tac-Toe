using Company.Runtime.Extensions;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VContainer;

namespace Company.Runtime.Popups {
  public class PopupsManager : IPopupsManager {
    private readonly Transform _canvasContainer;
    private readonly string _displayName;
    private readonly Dictionary<string, PopupPresenter> _popups = new Dictionary<string, PopupPresenter>();
    private readonly List<string> _orderedPopupIds = new List<string>();

    public static List<PopupsManager> Instances { get; } = new List<PopupsManager>();
    private static readonly Dictionary<string, PopupsManager> InstanceCacheByName = new Dictionary<string, PopupsManager>();


    private CanvasGroup _canvasGroup;

    public bool Interactable { get => _canvasGroup.interactable; set => _canvasGroup.interactable = value; }

    [Inject]
    public PopupsManager(Transform canvasContainer) {
      _canvasContainer = canvasContainer;
      _displayName = _canvasContainer.name;
    }

    public void OnStart() {
      Instances.Add(this);

      if (_canvasContainer != null) {
        InstanceCacheByName.Add(_displayName, this);
      }
      _canvasGroup = _canvasContainer.gameObject.GetOrAddComponent<CanvasGroup>();
    }

    public static PopupsManager Find(string containerName) {
      if (InstanceCacheByName.TryGetValue(containerName, out var instance)) return instance;

      return null;
    }

    public static void Close(Transform popupTransform) {
      string parentName = popupTransform.parent.name;
      PopupsManager container = Find(parentName);
      container.CloseAsync();
    }

    private async UniTask CloseAsync() {
      if (_orderedPopupIds.Count <= 0) {
        Debug.LogWarning($"All popup closed");
        return;
      }
      SetInteractableState(false);
      string name = _orderedPopupIds.Last();
      PopupPresenter popupPresenter = _popups[name];
      await popupPresenter.BeforeClose();
      if (popupPresenter.IsPlayAnimation()) {
        await popupPresenter.PlayHideAnimationAsync();
      } else {
        popupPresenter.ForceHide();
      }
      popupPresenter.AfterClose();

      _orderedPopupIds.Remove(name);
      _popups.Remove(name);

      SetInteractableState(true);
    }

    public void Show<TPopup>(Action<ICallbackReceiver> onCreated = null) where TPopup : PopupPresenter {
      ShowAsync(typeof(TPopup), onCreated);
    }

    private async UniTask ShowAsync(Type popupType, Action<ICallbackReceiver> onCreated) {
      string popupName = popupType.Name;
      if (_orderedPopupIds.Contains(popupName)) {
        Debug.LogWarning($"Popup {popupName} already open");
        return;
      }
      SetInteractableState(false);
      PopupPresenter popupPresenter = CreatePresenter(popupType);
      onCreated?.Invoke(popupPresenter);
      await popupPresenter.LoadPrefabAsync();
      popupPresenter.Instantiate(_canvasContainer);
      await popupPresenter.InitializeAsync();
      if (popupPresenter.IsPlayAnimation()) {
        await popupPresenter.PlayShowAnimationAsync();
      } else {
        popupPresenter.ForceShow();
      }
      popupPresenter.AfterOpen();
      _popups.Add(popupName, popupPresenter);
      _orderedPopupIds.Add(popupName);
      SetInteractableState(true);
    }

    private void SetInteractableState(bool isActive) {
      foreach (var popupContainer in Instances) popupContainer.Interactable = false;
    }

    private PopupPresenter CreatePresenter(Type popupType) {
      PopupPresenter value = (PopupPresenter) Activator.CreateInstance(popupType);
      return value;
    }

    public void Dispose() {
      InstanceCacheByName.Remove(_displayName);
      Instances.Remove(this);
    }
  }
}
