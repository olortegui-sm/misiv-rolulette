using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RouletteMisiv.Infrastructure.Exceptions
{
    [Serializable]
    public class CustomException : Exception
    {
        public int HttpStatusCode { get; set; }
        public string[] Messages { get; set; }
        public ServiceResponse Result { get; set; }

        public CustomException(string[] messages) : base(String.Join('-', messages))
        {
            HttpStatusCode = 400;
            Messages = messages;
        }

        public CustomException(int statusCode, string[] messages) : base(String.Join('-', messages))

        {
            HttpStatusCode = statusCode;
            Messages = messages;
        }

        public CustomException(int statusCode, ServiceResponse result) : base(result.Message)
        {
            HttpStatusCode = statusCode;
            Result = result;
        }
        public class ServiceResponse
        {
            public int Code { get; set; }
            public string Message { get; set; }
        }
    }
}
