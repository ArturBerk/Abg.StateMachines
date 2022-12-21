using System;

namespace Abg.StateMachines
{
    public class TransitAlreadyExistsException : Exception
    {
        public TransitAlreadyExistsException(Type transitType) : base($"Transit to state \"{transitType}\" already exists")
        {
        }
    }
}