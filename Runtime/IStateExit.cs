using System.Threading.Tasks;

namespace Abg.StateMachines
{
    public interface IStateExit
    {
        void OnExit();
    }
    
    public interface IStateExitAsync
    {
        Task OnExitAsync();
    }
}