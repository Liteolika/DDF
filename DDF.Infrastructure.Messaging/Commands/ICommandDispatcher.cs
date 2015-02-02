using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDF.Infrastructure.Messaging.Commands
{
    public interface ICommandDispatcher
    {

        void Dispatch<TCommand>(TCommand command);

    }
}
