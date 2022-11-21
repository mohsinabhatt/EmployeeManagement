namespace SharedLibrary
{
    public sealed class AppError
    {
        public AppError()
        {

        }


        public AppError(string message)
        {
            Message = message;
        }


        public AppError(string message, string field)
            :this(message)
        {
            Field = field;
        }


        public string Field { get; set; }


        public string Message { get; set; }
    }
}
