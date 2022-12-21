using System.Threading.Tasks;

namespace Abg.StateMachines
{
    public interface IStateMachine
    {
        Task Transit<TState, TPayload>(TPayload payload)
            where TState : class, IStateEnterWithPayload<TPayload>;
        Task Transit<TState>() where TState : class;
        Task Stop();
    }
}