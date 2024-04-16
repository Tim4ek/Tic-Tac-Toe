using R3;
using System;
using UnityEngine;
using VContainer;

namespace Company.Runtime.Gameplay {
  public class PlayerManager : IDisposable {
    private readonly BoardManager _boardManager;
    private readonly PlayerFactory _playerFactory;
    private readonly GameplaySettings _gameplaySettings;

    private IPlayer _player1;
    private IPlayer _player2;

    public ReadOnlyReactiveProperty<IPlayer> ActivePlayer => _activePlayer;
    private ReactiveProperty<IPlayer> _activePlayer = new(null);

    private Action _changeNextTurnAction;

    [Inject]
    public PlayerManager(BoardManager boardManager, PlayerFactory playerFactory, GameplaySettings gameplaySettings) {
      _boardManager = boardManager;
      _playerFactory = playerFactory;
      _gameplaySettings = gameplaySettings;
    }

    public void Dispose() {
      _activePlayer.Dispose();
      DestroyPlayer();
    }

    public void InitializeGame(PlayerType player1Type, PlayerType player2Type, Action changeNextTurnAction) {
      _changeNextTurnAction = changeNextTurnAction;

      if (_activePlayer.Value == null) {
        _player1 = _playerFactory.CreatePlayer(player1Type, ItemState.X, PutItem);
        _player2 = _playerFactory.CreatePlayer(player2Type, ItemState.O, PutItem);


        _player1.OnInitialize(player1Type);
        _player2.OnInitialize(player2Type);
      }
      _activePlayer.Value = null;
    }

    public void StartGame() {
      _activePlayer.Value = _player1;
    }

    public void EndGame() {
      _activePlayer.Value = null;
      _player1.OnEndGame();
      _player2.OnEndGame();
    }

    public void DestroyPlayer() {
      _player1?.OnDestroy();
      _player2?.OnDestroy();
    }

    public void StartTurn() {
      _activePlayer.Value.OnStartTurn();
    }

    public void UpdateTurn() {
      if (_activePlayer.Value == null) return;

      _activePlayer.Value.OnUpdateTurn();
    }

    public void EndTurn() {
      _activePlayer.Value = null;
    }

    public void ChangePlayer(int turnCount) {
      var isFirstTurn = turnCount % 2 == 0;
      _activePlayer.Value = isFirstTurn ? _player1 : _player2;
    }

    public PlayerResultState GetPlayer1ResultState() {
      return PlayerResultState.Win;
      //return _boardManager.GetPlayerResultState(_player1.SelfItemState);
    }
    public PlayerResultState GetPlayer2ResultState() {
      return PlayerResultState.Win;
      //return _boardManager.GetPlayerResultState(_player2.SelfItemState);
    }

    private void PutItem(ItemState itemState, Vector2Int position) {
      _boardManager.PutItem(itemState, position);
      _changeNextTurnAction?.Invoke();
    }
  }
}