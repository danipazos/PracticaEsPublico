namespace OrderImporter.Domain.Models
{
    internal sealed class Result<T>
    {
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public T Value { get; }
        public List<string> Errors { get; }

        protected Result(bool isSuccess, T value, List<string> errors)
        {
            IsSuccess = isSuccess;
            Value = value;
            Errors = errors;
        }

        public static Result<T> Success(T value) => new(true, value, new List<string>());
        public static Result<T> Failure(T value, List<string> errors) => new(false, value, errors);
    }
}