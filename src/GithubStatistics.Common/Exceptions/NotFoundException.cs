using System.Net;

namespace GithubStatistics.Common.Exceptions
{
    public class NotFoundException : CustomException
    {
        public NotFoundException(string message)
            : base(HttpStatusCode.NotFound, message)
        {
        }

        public NotFoundException()
            : base(nameof(HttpStatusCode.NotFound))
        {
        }
    }
}
