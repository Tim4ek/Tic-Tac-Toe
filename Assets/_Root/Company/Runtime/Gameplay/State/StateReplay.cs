using Company.Runtime.Utilities;

namespace Company.Runtime.Gameplay {
  public class StateReplay : StateMachine<GameplayManager>.BaseState {
    public override void OnStart() {
      Owner.PlayerManager.EndTurn();
      Owner.BoardManager.OnClear();
      Owner.PlayerManager.DestroyPlayer();

      StateMachine.ChangeState((int) GameplayState.Play);
    }
  }
}