using Company.Runtime.Persistent.Services;

namespace Company.Runtime.Persistent {
  public interface IServicesManager {
    void OnStart();
    void OnUpdate();

    IVibrationService VibrationService {
      get;
    }
  }
}