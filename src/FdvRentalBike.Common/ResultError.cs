namespace FdvRentalBike.Common
{
    public class ResultError
    {
        public string ErrorCode { get; set; }

        public string ErrorMessage { get; set; }

        public ResultError(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        public ResultError(string errorCode, string errorMessage)
        {
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
        }
    }
}
