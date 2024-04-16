using System;
using UnityEngine;
using VContainer;
using R3;

namespace Company.Runtime.Gameplay {
  public class GameplayPresenter : MonoBehaviour, IDisposable {
    [SerializeField] private GameTopPanelView gameTopPanelView;

    private BoardManager _boardManager;
    private PlayerManager _playerManager;
    private GameplayManager _gameplayManager;

    private IDisposable _disposable;

    [Inject]
    public void Construct(BoardManager boardManager, PlayerManager playerManager, GameplayManager gameplayManager) {
      _boardManager = boardManager;
      _playerManager = playerManager;
      _gameplayManager = gameplayManager;
    }

    public void OnStart() {
      OnInitializeAllView();
    }

    private void OnInitializeAllView() {
      gameTopPanelView.SetActive(true);

      OnSetAllButtonListener();

      OnSetStateSubscribe();
    }

    private void OnSetAllButtonListener() {
      gameTopPanelView.SetListenerBackButton(() => {
        _gameplayManager.OnBackScene();
      });
      gameTopPanelView.SetListenerReplayButton(() => {
        _gameplayManager.OnReplay();
      });
    }

    private void OnSetStateSubscribe() {
      IDisposable itemStateDisposable = _playerManager
        .ActivePlayer
        .Subscribe(activePlayer => {
          var activeItemState = activePlayer?.SelfItemState ?? ItemState.Empty;
          gameTopPanelView.ChangeActiveColor(activeItemState);
        });

      IDisposable gameplayStateDisposable = _gameplayManager
        .State
        .Subscribe(x => {
          switch (x) {

            case GameplayState.Play:
              OnShowStart();
              break;

            case GameplayState.Result:
              OnShowResult();
              break;
          }
        });
      _disposable = Disposable.Combine(itemStateDisposable, gameplayStateDisposable);
    }

    private void OnShowStart() {
      Debug.Log("Start of Game");
      //gameInfoView.ShowMessage("Start of game", () =>
      //{
      //  gameInfoView.SetActiveMessageArea(false);
      //}, 0.5f);
    }

    private void OnShowResult() {
      Debug.Log("Show result");
      //gameInfoView.ShowMessage("Start of game", () =>
      //{
      //  gameInfoView.SetActiveMessageArea(false);
      //}, 0.5f);
    }

    public void Dispose() {
      try {
        Debug.Log("Dispose Gameplay Presenter");
        _disposable.Dispose();
      } catch (Exception e) {
        Debug.Log($"Dispose error:{e.Message}");
      }
    }
  }
}

