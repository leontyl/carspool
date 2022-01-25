using System;

namespace CarsPool.Api.Exceptions
{
    public class CarsPoolDalException : Exception
    {
        public CarsPoolDalException(string message) : base(message)
        {
        }

        public DalErrorCodes ErrorCode { get; set; }
    }
}
