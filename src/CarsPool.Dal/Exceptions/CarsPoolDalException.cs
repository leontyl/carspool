using System;

namespace CarsPool.Dal.Exceptions
{
    public class CarsPoolDalException : Exception
    {
        public DalErrorCodes ErrorCode { get; set; }
    }
}
