using System.Threading.Tasks;

namespace Abg.StateMachines
{
    public abstract class CompositeState<TStartState> : CompositeState, IStateEnterAsync 
        where TStartState : class
    {
        public virtual ValueTask OnEnterAsync()
        {
            return Transit<TStartState>();
        }
    }
    
    public abstract class CompositeState : StateMachine, IStateExitAsync
    {
        public virtual ValueTask OnExitAsync()
        {
            return Stop();
        }
    }
}