using System.Threading.Tasks;

namespace Abg.StateMachines
{
    public abstract class CompositeState : IStateMachine, IStateEnter, IStateExit
    {
        private StateMachine stateMachine;

        protected abstract Task InitializeStates(StateMachine stateMachine);
        
        public virtual Task OnEnter()
        {
            stateMachine = new StateMachine();
            return InitializeStates(stateMachine);
        }

        public virtual Task OnExit()
        {
            var t = stateMachine.Stop();
            stateMachine = null;
            return t;
        }

        public Task Transit<TState, TPayload>(TPayload payload) where TState : class, IStateEnterWithPayload<TPayload>
        {
            return stateMachine.Transit<TState, TPayload>(payload);
        }

        public Task Transit<TState>() where TState : class
        {
            return stateMachine.Transit<TState>();
        }

        public Task Stop()
        {
            return stateMachine.Stop();
        }
    }
}