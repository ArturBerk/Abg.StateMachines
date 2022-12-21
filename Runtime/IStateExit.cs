using System.Threading.Tasks;

namespace Abg.StateMachines
{
    public interface IStateExit
    {
        Task OnExit();
    }
}