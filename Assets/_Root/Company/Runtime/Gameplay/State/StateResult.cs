using Company.Runtime.Utilities;

namespace Company.Runtime.Gameplay {
  public class StateResult : StateMachine<GameplayManager>.BaseState {
    public override void OnStart() {
      Owner.BoardManager.OnClear();
      Owner.PlayerManager.DestroyPlayer();
      StateMachine.ChangeState((int) GameplayState.Play);
    }
  }
}