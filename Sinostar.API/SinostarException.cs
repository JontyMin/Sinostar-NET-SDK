using System;

namespace Sinostar.API
{
    public class SinostarException:Exception
    {
        public SinostarException()
        {
        }

        public SinostarException(string msg, Exception ex = null)
            : base(msg, ex)
        {
        }

    }
}