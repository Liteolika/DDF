using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDF.Core.Exceptions
{
    public class ConcurrencyException : Exception
    {

        public ConcurrencyException()
        {

        }

        public ConcurrencyException(string message)
            : base(message)
        {

        }

    }
}
