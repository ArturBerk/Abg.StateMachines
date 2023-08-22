using System.Threading.Tasks;

namespace Abg.StateMachines
{
    public interface IStateMachine
    {
        ValueTask Transit<TState, TPayload>(TPayload payload)
            where TState : class, IStateEnterWithPayload<TPayload>;
        ValueTask Transit<TState>() where TState : class;
        ValueTask Stop();
    }
}