using System;

namespace Abg.StateMachines
{
    public class TransitNotFoundException : Exception
    {
        public TransitNotFoundException(Type transitType) : base($"Transit to state \"{transitType}\" not found")
        {
        }
    }
}