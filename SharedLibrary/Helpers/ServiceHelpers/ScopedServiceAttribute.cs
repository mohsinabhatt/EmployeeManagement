using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLibrary
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false, Inherited = false)]
    public sealed class ScopedServiceAttribute : Attribute
    {
        public ScopedServiceAttribute()
        {
        }


        public ScopedServiceAttribute(Type implementationType)
        {
            ImplementationType = implementationType;
        }


        public Type ImplementationType { get; }
    }
}
