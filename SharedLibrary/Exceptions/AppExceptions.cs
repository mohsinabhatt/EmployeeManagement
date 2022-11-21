
using System;
using System.Collections.Generic;

namespace SharedLibrary
{

    public class AppException : Exception
    {
        public AppException(string message = null) : base(message) { }
    }


    public sealed class AppValidationException : AppException
    {
        /// <summary>
        /// Use placeholder as {0} if you want to mention field parameter in message string
        /// </summary>
        public AppValidationException(string message = "Validation error, please check the data you have sent.", string field = null)
        {
            Errors = new List<AppError> { new AppError(string.Format(message, field), field) };
        }


        public AppValidationException(List<AppError> errors)
        {
            Errors = errors;
        }


        public List<AppError> Errors { get; private set; }
    }


    public sealed class ForbiddenException : AppException
    {
        public ForbiddenException(string message = null)
            : base(message ?? "You do not have permission.")
        {
        }
    }


    public sealed class AppInfoException : AppException
    {
        public AppInfoException(string message) : base(message)
        {

        }
    }


    public sealed class AppNotFoundException : AppException
    {
        public AppNotFoundException(string message) : base(message)
        {

        }
    }
}


