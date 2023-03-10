using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Abg.StateMachines
{
    public class StateMachine : IStateMachine
    {
        private readonly Dictionary<(Type, Type), object> transitions = new();
        private object activeState;
        private Type activeStateType;
        private bool isStateChanging;

        private StateChanging BeginStateChanging()
        {
            return new StateChanging(this);
        }
        
        public void AddTransitionFromAny<TState>(Func<TState> stateFactory) where TState : class
        {
            transitions[(null, typeof(TState))] = stateFactory;
        }

        public void AddTransition<TFrom, TTo>(Func<TTo> stateFactory) where TTo : class
        {
            transitions[(typeof(TFrom), typeof(TTo))] = stateFactory;
        }

        public Task Transit<TState>() where TState : class
        {
            using var stateChanging = BeginStateChanging();
            if (!TryGetState<TState>(activeStateType, out var newState))
            {
                if (activeState is IStateMachine subStateMachine)
                    return subStateMachine.Transit<TState>();
                else throw new TransitNotFoundException(typeof(TState));
            }

            return SwitchState(newState);
        }

        public Task Transit<TState, TPayload>(TPayload payload)
            where TState : class, IStateEnterWithPayload<TPayload>
        {
            using var stateChanging = BeginStateChanging();
            if (!TryGetState<TState>(activeState?.GetType(), out var newState))
            {
                if (activeState is IStateMachine subStateMachine)
                    return subStateMachine.Transit<TState, TPayload>(payload);
                else throw new TransitNotFoundException(typeof(TState));
            }

            return SwitchState(newState, payload);
        }

        public Task Stop()
        {
            using var stateChanging = BeginStateChanging();
            return ExitState();
        }

        private bool TryGetState<TState>(Type activeStateType, out TState state) where TState : class
        {
            state = null;
            if (activeStateType != null &&
                transitions.TryGetValue((activeStateType, typeof(TState)), out var stateFactory))
                state = ((Func<TState>)stateFactory)();
            if (transitions.TryGetValue((null, typeof(TState)), out stateFactory))
                state = ((Func<TState>)stateFactory)();

            return state != null;
        }

        private async Task SwitchState<TState>(TState newState)
        {
            await ExitState();
            activeStateType = typeof(TState);
            activeState = newState;
            
            if (activeState is IStateEnter stateEnter)
                await stateEnter.OnEnter();
        }
        
        private async Task SwitchState<TState, TPayload>(TState newState, TPayload payload)
        {
            await ExitState();
            activeStateType = typeof(TState);
            activeState = newState;
            
            if (activeState is IStateEnterWithPayload<TPayload> stateEnter)
                await stateEnter.OnEnterWithPayload(payload);
        }

        private async Task ExitState()
        {
            if (activeState is IStateExit stateExit)
                await stateExit.OnExit();
            activeState = null;
            activeStateType = null;
        }

        private readonly struct StateChanging : IDisposable
        {
            private readonly StateMachine stateMachine;

            public StateChanging(StateMachine stateMachine)
            {
                if (stateMachine.isStateChanging)
                    throw new Exception("State machine in states changing state, can't transit to new state");
                this.stateMachine = stateMachine;
            }

            public void Dispose()
            {
                stateMachine.isStateChanging = false;
            }
        }
    }
}