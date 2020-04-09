using System;
using System.Net;

namespace GithubStatistics.Common.Exceptions
{
    public class CustomException : Exception
    {
        public CustomException(HttpStatusCode status, string message)
            : base(message)
        {
            Status = status;
        }

        public CustomException(string message)
            : base(message)
        {
            Status = HttpStatusCode.InternalServerError;
        }

        public HttpStatusCode Status { get; }
    }
}
