
using System;
using VContainer;

namespace Company.Runtime.Loading {
  public class LoadingBarPresenter {
    private readonly LoadingBarView _loadingBarView;
    private bool _isLoadingCompleted = false;

    public bool IsLoadingCompleted => _isLoadingCompleted;

    [Inject]
    public LoadingBarPresenter(LoadingBarView loadingBarView) {
      _loadingBarView = loadingBarView;
    }

    public void OnStart() {
      _loadingBarView.OnStart();
    }

    public void OnUpdate() {
      if (!_isLoadingCompleted) {
        _loadingBarView.OnFill();
        if (_loadingBarView.IsFilled()) {
          _isLoadingCompleted = true;
        }
      }
    }
  }
}
