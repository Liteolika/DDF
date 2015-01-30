using DDF.Core.Messaging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DDF.Core.Tests.Messaging
{
    [TestFixture]
    public class MessageDispatcherTests
    {

        [Test]
        public void register_handler_can_be_executed()
        {
            MessageDispatcher<ICommand> disp = new MessageDispatcher<ICommand>(false);

            SomeHandler handler = new SomeHandler();
            SomeCommand cmd = new SomeCommand();
            SomeOtherCommand cmd2 = new SomeOtherCommand();

            disp.RegisterHandler(handler);

            Thread.Sleep(20);

            disp.Dispatch(cmd);
            disp.Dispatch(cmd2);

            Assert.AreEqual(cmd, handler.command);
            Assert.AreEqual(cmd2, handler.command2);

        }

        [Test]
        public void register_handler_can_be_executed_with_notifications()
        {
            MessageDispatcher<INotification> disp = new MessageDispatcher<INotification>(true);

            SomeHandler handler = new SomeHandler();
            SomeCommand cmd = new SomeCommand();
            SomeOtherCommand cmd2 = new SomeOtherCommand();

            SomeNotification n1 = new SomeNotification();
            SomeNotification n2 = new SomeNotification();

            disp.RegisterHandler(handler);

            Thread.Sleep(20);

            disp.Dispatch(n1);
            Assert.AreEqual(n1, handler.notif);

            disp.Dispatch(n2);
            Assert.AreEqual(n2, handler.notif);

        }

    }

    

    public class SomeCommand : ICommand
    {

    }

    public class SomeOtherCommand : ICommand
    {

    }

    public class SomeNotification : INotification
    {

    }

    public class SomeHandler // : IHandleCommand<SomeCommand>
    {
        public ICommand command = null;
        public ICommand command2 = null;
        public INotification notif = null;

        private void HandleNofitification(SomeNotification notification)
        {
            this.notif = notification;
        }

        private void HandleCommand(SomeCommand command)
        {
            this.command = command;
        }

        private void HandleCommand(SomeOtherCommand command)
        {
            command2 = command;
        }

        private void HandleCommand(object someobject)
        {
            
        }

        public void HandleCommand(string somesthring)
        {

        }
    }

}
