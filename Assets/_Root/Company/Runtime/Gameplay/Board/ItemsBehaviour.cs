
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Company.Runtime.Gameplay {
  public class ItemsBehaviour {
    private const string _itemViewPrefabPath = "ItemView";

    private GameObject _itemsBase;
    private ItemViewBehaviour _itemViewBehaviourPrefab;

    private ItemViewBehaviour[,] _itemsViewBehaviours;

    public async UniTask Prepare(Transform parent, int cellSideCount) {
      _itemViewBehaviourPrefab = await LoadItemViewPrefab();
      if (_itemViewBehaviourPrefab == null) {
        return;
      }

      _itemsBase = new GameObject("Items");
      _itemsBase.transform.SetParent(parent);
      _itemsBase.transform.position = Vector3.zero;
      _itemsBase.transform.localScale = Vector3.one;

      _itemsViewBehaviours = new ItemViewBehaviour[cellSideCount, cellSideCount];
    }

    private AsyncOperationHandle<GameObject> _handle;

    private async UniTask<ItemViewBehaviour> LoadItemViewPrefab() {
      _handle = Addressables.LoadAssetAsync<GameObject>(_itemViewPrefabPath);
      //UniTask<GameObject> asyncOperationHandle = Addressables.LoadAssetAsync<GameObject>(_itemViewPrefabPath).ToUniTask();
      GameObject itemGO = await _handle;
      if (itemGO.TryGetComponent(out ItemViewBehaviour itemViewBehaviour) == false) {
        Debug.LogError("Object ItemViewBehaviour is null");
      }
      return itemViewBehaviour;
    }

    public void PutItem(ItemState itemState, Vector2Int position, Vector3 worldPosition, Sprite sprite) {
      ItemViewBehaviour item = UnityEngine.Object.Instantiate(_itemViewBehaviourPrefab, _itemsBase.transform, true);
      item.transform.localPosition = worldPosition;
      item.transform.localScale = Vector3.one;
      item.SetSprite(sprite);
      _itemsViewBehaviours[position.x, position.y] = item;
    }

    public void OnClear() {
      if (_itemsViewBehaviours != null) {
        int xLength = _itemsViewBehaviours.GetLength(0);
        int yLength = _itemsViewBehaviours.GetLength(1);
        for (int x = 0; x < xLength; x++) {
          for (int y = 0; y < yLength; y++) {
            if (_itemsViewBehaviours[x, y] != null) {
              _itemsViewBehaviours[x, y].DestroyView();
            }
          }
        }
        _itemsViewBehaviours = null;
        UnityEngine.Object.Destroy(_itemsBase);
      }
      if (_handle.IsValid()) {
        Addressables.Release(_handle);
      }
    }
  }
}