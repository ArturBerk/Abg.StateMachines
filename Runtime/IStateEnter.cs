using System.Threading.Tasks;

namespace Abg.StateMachines
{
    public interface IStateEnter
    {
        void OnEnter();
    }

    public interface IStateEnterAsync
    {
        ValueTask OnEnterAsync();
    }
}