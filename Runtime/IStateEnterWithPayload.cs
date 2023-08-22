using System.Threading.Tasks;

namespace Abg.StateMachines
{
    public interface IStateEnterWithPayload<TPayload>
    {
        ValueTask OnEnterWithPayload(TPayload payload);
    }
}