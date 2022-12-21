using System.Threading.Tasks;

namespace Abg.StateMachines
{
    public interface IStateEnterWithPayload<TPayload>
    {
        Task OnEnterWithPayload(TPayload payload);
    }
}