using System;
using System.Collections.Generic;

namespace Utils.State
{
    //base class for FSM and HFSM
    public abstract class AbstractStateMachine<TEnum> : IStateMachine<TEnum> where TEnum : Enum
    {
        protected IState _CurrentState;

        /// <summary> if the machine is inactive, current state reflects the last state before exit </summary>
        public TEnum CurrentState;

        //happens whenever the state changes
        public Action OnStateChanged;
        
        protected readonly Dictionary<TEnum, IState> StateMap = new();

        public void AddState(TEnum state_id, IState state)
        {
            if (state is null)
                throw new Exception($"Trying to add a null state with id: {state_id}. Are you stupid?");
            StateMap.Add(state_id, state);
        }

        public void RemoveState(TEnum state_id)
            => StateMap.Remove(state_id);

        //for clarity
        public void AddSubStateMachine<TSubEnum>(TEnum state_id, IStateMachine<TSubEnum> state_machine)
            where TSubEnum : Enum
            => AddState(state_id, state_machine);

        //for clarity
        public void RemoveSubStateMachine(TEnum state_id)
            => RemoveState(state_id);

        public void TransitionTo(TEnum new_state_id)
        {
            IState newState = StateMap[new_state_id];
            
            if (newState == null)
                throw new Exception($"state in {this} with id {new_state_id} not found!");
            
            if (_CurrentState == newState)
            {
                return;
            }
            
            _CurrentState?.Exit();

            _CurrentState = newState;
            CurrentState = new_state_id;
            
            newState.Enter();
            
            OnStateChanged?.Invoke();
        }

        public virtual void Update() => _CurrentState?.Update();

        //for clarity
        public virtual void Init(TEnum initial_state_id) => TransitionTo(initial_state_id);

        public virtual void Enter()
        {
        }

        public virtual void Exit()
        {
            if (_CurrentState is null)
                throw new Exception(
                    $"CurrentState for {this} is null. Did you call Init() in the machine Enter() override? Are you calling Exit() twice?");

            _CurrentState.Exit();
            _CurrentState = null;
        }

        public bool IsStateMachine() => true;

        public string GetName() => GetCurrentStateNameRecursive();

        /// <summary>Returns a string concatenating the current state's name and all substates' names.</summary>
        public string GetCurrentStateNameRecursive()
        {
            string result = CurrentState.ToString();
            return result + (_CurrentState.IsStateMachine()
                ? ", " + _CurrentState.GetName()
                : "");
        }
    }
}