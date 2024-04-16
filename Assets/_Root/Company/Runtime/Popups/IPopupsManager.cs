using System;

namespace Company.Runtime.Popups {
  public interface IPopupsManager : IDisposable {
    void OnStart();
  }
}