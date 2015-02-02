﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDF.Infrastructure.Messaging.Events
{
    public interface IEventHandler<T>
    {
        void Handle(T @event);
    }
}
