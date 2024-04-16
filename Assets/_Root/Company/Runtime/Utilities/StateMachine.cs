using System;
using System.Collections.Generic;
using UnityEngine;

namespace Company.Runtime.Utilities {
  public class StateMachine<T> {
    public abstract class BaseState {
      public StateMachine<T> StateMachine;
      protected T Owner => StateMachine.Owner;
      public int StateId;

      public virtual void OnStart() { }
      public virtual void OnUpdate() { }
      public virtual void OnEnd() { }
    }

    private T Owner { get; }

    private BaseState _currentState;
    private BaseState _prevState;
    private BaseState CurrentState {
      get => _currentState;
      set {
        _currentState = value;
        OnChangedState?.Invoke(value.StateId);
      }
    }

    private readonly Dictionary<int, BaseState> _states = new Dictionary<int, BaseState>();

    private event Action<int> OnChangedState;

    public StateMachine(T owner) {
      Owner = owner;
    }

    public void SetChangeStateEvent(Action<int> action) {
      OnChangedState = action;
    }

    public void Add<U>(int stateId) where U : BaseState, new() {
      if (_states.ContainsKey(stateId)) {
        Debug.LogError($"Already add stateId! : {stateId}");
        return;
      }

      var newState = new U {
        StateMachine = this,
        StateId = stateId,
      };
      _states.Add(stateId, newState);
    }

    public void OnStart(int stateId) {
      if (!_states.TryGetValue(stateId, out var nextState)) {
        Debug.LogError($"Not such stateId! : {stateId}");
        return;
      }

      CurrentState = nextState;
      CurrentState.OnStart();
    }

    public void OnUpdate() {
      CurrentState?.OnUpdate();
    }

    public void ChangeState(int stateId) {
      if (!_states.TryGetValue(stateId, out var nextState)) {
        Debug.LogError($"Not such stateId! : {stateId}");
        return;
      }

      _prevState = CurrentState;

      CurrentState.OnEnd();
      CurrentState = nextState;
      CurrentState.OnStart();
    }

    public void ChangePrevState() {
      if (_prevState == null) {
        Debug.LogError("PrevState is null");
        return;
      }

      (_prevState, CurrentState) = (CurrentState, _prevState);
    }

    public bool IsCurrentState(int stateId) {
      if (!_states.TryGetValue(stateId, out var state)) {
        Debug.LogError($"Not such stateId! : {stateId}");
        return false;
      }
      return CurrentState == state;
    }
  }
}
