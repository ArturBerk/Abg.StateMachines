using System.Threading.Tasks;

namespace Abg.StateMachines
{
    public interface IStateEnter
    {
        Task OnEnter();
    }
}