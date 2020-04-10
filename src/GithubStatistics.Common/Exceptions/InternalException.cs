using System.Net;

namespace GithubStatistics.Common.Exceptions
{
    public class InternalException : CustomException
    {
        public InternalException(string message)
            : base(HttpStatusCode.InternalServerError, message)
        {
        }

        public InternalException()
            : base(nameof(HttpStatusCode.InternalServerError))
        {
        }
    }
}
