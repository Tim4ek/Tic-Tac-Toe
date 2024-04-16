namespace Company.Runtime.Persistent.Services {
  public interface IInitialize {
    void Init();
  }

  public interface IUpdateable {
    void OnUpdate();
  }

  public interface IInitAndUpdate : IInitialize, IUpdateable {
  }
}