using System;

namespace Company.Runtime.Popups {
  public interface ICallbackReceiver {
    event EventHandler<BeforeOpenEventArgs> OnBeforeOpen;
    event EventHandler<AfterOpenEventArgs> OnAfterOpen;
    event EventHandler<BeforeCloseEventArgs> OnBeforeClose;
    event EventHandler<AfterCloseEventArgs> OnAfterClose;
  }

  public class BasePopupEventArgs : EventArgs {
    public ICallbackReceiver receiver;
  }

  public class BeforeOpenEventArgs : BasePopupEventArgs {

  }

  public class AfterOpenEventArgs : BasePopupEventArgs {

  }

  public class BeforeCloseEventArgs : BasePopupEventArgs {

  }

  public class AfterCloseEventArgs : BasePopupEventArgs {

  }
}