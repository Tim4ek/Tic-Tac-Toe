using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using VContainer;

namespace Company.Runtime.Gameplay {
  public class BoardManager : IDisposable {
    private const string _gridPrefabPath = "GridSlot";

    private readonly Grid _grid;

    private readonly CameraScaler _cameraHelper;
    private readonly Transform _boardParent;

    private readonly GameplaySettings _gameplaySettings;

    private readonly ItemsBehaviour _itemsBahaviour;


    private GridBehaviour _gridBehaviourPrefab;
    private GameObject _boardGridsBase;

    private int _cellSideCount = 3;

    private GridBehaviour[,] _viewGridBehaviours;

    private ItemState[,] _gridStates;
    private Bounds _gridsBounds;

    public ItemState this[Vector2Int position] => _gridStates[position.x, position.y];
    public ItemState this[int columnIndex, int rowIndex] => _gridStates[columnIndex, rowIndex];
    public Transform Parent => _boardParent;
    public int CellSideCount => _cellSideCount;
    public Bounds GridsBounds => _gridsBounds;
    public ItemState[,] GridStates => _gridStates;

    public event Action _onBoardCreated;

    [Inject]
    public BoardManager(Grid grid, CameraScaler cameraHelper, Transform boardParent, GameplaySettings gameplaySettings) {
      _grid = grid;
      _cameraHelper = cameraHelper;
      _boardParent = boardParent;
      _gameplaySettings = gameplaySettings;
      _cellSideCount = _gameplaySettings.CellSideCount;
      _itemsBahaviour = new ItemsBehaviour();
    }

    public void InitializeBoard(Action onBoardCreated) {
      _onBoardCreated = onBoardCreated;
      CreateBoard();
    }

    private async void CreateBoard() {
      await _itemsBahaviour.Prepare(_boardParent, _cellSideCount);
      _gridBehaviourPrefab = await LoadGridPrefab();
      if (_gridBehaviourPrefab == null) {
        return;
      }

      _boardGridsBase = new GameObject("Grids");
      _boardGridsBase.transform.SetParent(_boardParent);
      _boardGridsBase.transform.position = Vector3.zero;
      _boardGridsBase.transform.localScale = Vector3.one;

      _viewGridBehaviours = new GridBehaviour[_cellSideCount, _cellSideCount];
      _gridStates = new ItemState[_cellSideCount, _cellSideCount];

      for (var x = 0; x < _viewGridBehaviours.GetLength(0); x++) {
        for (var y = 0; y < _viewGridBehaviours.GetLength(1); y++) {

          GridBehaviour grid = UnityEngine.Object.Instantiate(_gridBehaviourPrefab, _boardGridsBase.transform, true);
          grid.transform.localPosition = GetWorldPosition(x, y);
          grid.transform.localScale = Vector3.one;
          _viewGridBehaviours[x, y] = grid;
          _gridStates[x, y] = ItemState.Empty;
          _gridsBounds.Encapsulate(GetGridBound(x, y));
        }
      }
      _gridsBounds.center = new Vector3(_gridsBounds.center.x + 0.5f, _gridsBounds.center.y + 0.5f, 0f);
      _cameraHelper.UpdateSizeAndPosition(_gridsBounds);
      _onBoardCreated?.Invoke();
      _onBoardCreated = null;
    }

    private Bounds GetGridBound(int x, int y) {
      Vector3Int cell = new Vector3Int(x, y, 0);
      Bounds bounds = _grid.GetBoundsLocal(cell);
      return bounds;
    }

    private AsyncOperationHandle<GameObject> _handle;

    private async UniTask<GridBehaviour> LoadGridPrefab() {
      _handle = Addressables.LoadAssetAsync<GameObject>(_gridPrefabPath);
      //UniTask<GameObject> asyncOperationHandle = Addressables.LoadAssetAsync<GameObject>(_gridPrefabPath).ToUniTask();
      GameObject gridGO = await _handle;
      if (gridGO.TryGetComponent(out GridBehaviour gridBehaviour) == false) {
        Debug.LogError("Object GridBehaviour is null");
      }
      return gridBehaviour;
    }

    public Vector3 GetWorldPosition(int x, int y) {
      Vector3Int cell = new Vector3Int(x, y, 0);
      Vector3 cellWorld = _grid.CellToWorld(cell);
      return cellWorld;
    }

    public ItemState GetItemStateByWorldPosition(Vector3 position, out Vector2Int gridPosition) {
      gridPosition = default;
      Vector3 point = _cameraHelper.ScreenToWorldPoint(position);
      Vector3Int cell = WorldToCell(point);
      if (IsPositionOnBoard(new Vector2Int(cell.x, cell.y), out ItemState itemState)) {
        gridPosition = new Vector2Int(cell.x, cell.y);
        return itemState;
      }
      return ItemState.None;
    }

    public void PutItem(ItemState itemState, Vector2Int position) {
      _gridStates[position.x, position.y] = itemState;
      _itemsBahaviour.PutItem(itemState, position, GetWorldPosition(position.x, position.y), _gameplaySettings.GetSpriteByItemState(itemState));
    }

    public List<Vector2Int> GetAllCanPutItemsIndex() {
      List<Vector2Int> canPutIndexes = new List<Vector2Int>();
      for (var x = 0; x < _gridStates.GetLength(0); x++) {
        for (var y = 0; y < _gridStates.GetLength(1); y++) {
          if (_gridStates[x, y] == ItemState.Empty) {
            canPutIndexes.Add(new Vector2Int(x, y));
          }
        }
      }
      return canPutIndexes;
    }

    public Vector3Int WorldToCell(Vector3 worldPosition) {
      return _grid.WorldToCell(worldPosition);
    }

    public bool IsPositionOnBoard(Vector2Int position, out ItemState itemState) {
      itemState = ItemState.None;
      if (IsPositionOnBoard(position)) {
        itemState = _gridStates[position.x, position.y];
        return true;
      } else {
        return false;
      }
    }

    public bool IsPositionOnBoard(Vector2Int gridPosition) {
      return IsPositionOnBoard(gridPosition, _cellSideCount, _cellSideCount);
    }

    private bool IsPositionOnBoard(Vector2Int gridPosition, int rowCount, int columnCount) {
      return gridPosition.x >= 0 &&
             gridPosition.x < columnCount &&
             gridPosition.y >= 0 &&
             gridPosition.y < rowCount;
    }

    public void OnClear() {
      _itemsBahaviour.OnClear();

      if (_viewGridBehaviours != null) {
        int xLength = _viewGridBehaviours.GetLength(0);
        int yLength = _viewGridBehaviours.GetLength(1);
        for (int x = 0; x < xLength; x++) {
          for (int y = 0; y < yLength; y++) {
            if (_viewGridBehaviours[x, y] != null) {
              _viewGridBehaviours[x, y].DestroyView();
            }
          }
        }
        UnityEngine.Object.Destroy(_boardGridsBase);
        _gridStates = null;
        _gridsBounds = new Bounds(Vector3.zero, Vector3.zero);
        _viewGridBehaviours = null;
      }
      if (_handle.IsValid()) {
        Addressables.Release(_handle);
      }
    }

    public static List<Vector2Int> GetAvailableMoveByState(ItemState[,] gridsState) {
      List<Vector2Int> availableMoves = new List<Vector2Int>();
      for (var x = 0; x < gridsState.GetLength(0); x++) {
        for (var y = 0; y < gridsState.GetLength(1); y++) {
          if (gridsState[x, y] == ItemState.Empty) {
            availableMoves.Add(new Vector2Int(x, y));
          }
        }
      }
      return availableMoves;
    }

    public void Dispose() {
      Debug.Log("BoardManager dispose");
      OnClear();
    }
  }
}
