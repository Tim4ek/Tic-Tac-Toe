
namespace Company.Runtime.Gameplay {
  public interface IPlayer {
    public ItemState SelfItemState { get; }

    public PlayerType SelfPlayerType { get; }

    public void OnInitialize(PlayerType playerType);
    public void OnStartTurn();
    public void OnUpdateTurn();
    public void OnEndGame();
    public bool IsInputPlayer();
    public void OnDestroy();
  }
}
