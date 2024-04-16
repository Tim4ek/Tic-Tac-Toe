using Company.Runtime.Gameplay;
using System;
using TMPro;
using UnityEngine;
using VContainer;

namespace Company.Runtime.Meta {
  public class MetaPresenter : MonoBehaviour, IDisposable {
    [SerializeField] private TapToPlayButton tapToPlayButton;
    [SerializeField] private TMP_Dropdown _fieldSize;
    [SerializeField] private TMP_Dropdown _botAI;

    private MetaManager _metaManager;
    private IDisposable _disposable;

    [Inject]
    public void Construct(MetaManager metaManager) {
      _metaManager = metaManager;
    }

    public void OnStart() {
      OnInitializeAllView(_metaManager.Tab.CurrentValue);
      //SetMetaStateListener();
    }

    //private void SetMetaStateListener() {
    //  IDisposable stateDisposable = _metaManager
    //           .Tab
    //           .Subscribe(state => {

    //           });
    //  _disposable = Disposable.Combine(stateDisposable);
    //}


    private void OnInitializeAllView(MetaTabType tab) {
      OnSetAllButtonListener();
    }

    private void OnSetAllButtonListener() {
      tapToPlayButton.SetListenerBackButton(() => {
        int fieldSize = GetFieldSize();
        PlayerType botAI = GetBotAI();
        _metaManager.StartToPlay(fieldSize, botAI);
      });
    }

    private PlayerType GetBotAI() {
      string value = _botAI.options[_botAI.value].text;
      if (value.Equals("MiniMax AI")) {
        return PlayerType.MiniMaxAIPlayer; ;
      } else if (value.Equals("Random AI")) {
        return PlayerType.RandomAIPlayer; ;
      } else {
        return PlayerType.MiniMaxAIPlayer;
      }
    }

    private int GetFieldSize() {
      string value = _fieldSize.options[_fieldSize.value].text;
      if (int.TryParse(value, out int result)) {
        return result;
      } else {
        return 3;
      }
    }

    public void Dispose() {
      //_disposable.Dispose();
      Debug.Log("Dispose Meta Presentation");
    }
  }
}