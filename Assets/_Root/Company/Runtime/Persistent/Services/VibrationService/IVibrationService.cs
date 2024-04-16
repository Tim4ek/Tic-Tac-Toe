namespace Company.Runtime.Persistent.Services {
  public interface IVibrationService : IInitAndUpdate {
    void SetState(bool isActive);
    void PlayVibration(float time);
    void PlayVibration(bool isPressed);
    void PlayVibration();
  }
}