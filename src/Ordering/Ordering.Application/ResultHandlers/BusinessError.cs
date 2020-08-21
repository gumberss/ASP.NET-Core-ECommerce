using System;

namespace Ordering.Application.ResultHandlers
{
    public class BusinessError
    {
        public String Message { get; private set; }

        public BusinessError(string message)
        {
            Message = message;
        }
    }
}
