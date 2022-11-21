using System;
using System.ComponentModel.DataAnnotations;

namespace SharedLibrary
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public sealed class GuidRequired : RequiredAttribute
    {
        public override bool IsValid(object value)
        {
            return value switch
            {
                Guid v => v.IsNotEmpty(),
                _ => base.IsValid(value),
            };
        }
    }
}
