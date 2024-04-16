
using Company.Runtime.Persistent.Services;
using System.Collections.Generic;

namespace Company.Runtime.Persistent {
  public class ServicesManager : IServicesManager {
    private List<IInitAndUpdate> _initAndUpdateServices = new List<IInitAndUpdate>();

    private IVibrationService _vibrationService;

    public IVibrationService VibrationService => _vibrationService;


    public ServicesManager() {
      _vibrationService = new VibrationService();
      _initAndUpdateServices.Add(_vibrationService);
    }


    public void OnStart() {
      for (int i = 0; i < _initAndUpdateServices.Count; i++) {
        _initAndUpdateServices[i].Init();
      }
    }

    public void OnUpdate() {
      for (int i = 0; i < _initAndUpdateServices.Count; i++) {
        _initAndUpdateServices[i].OnUpdate();
      }
    }
  }
}
