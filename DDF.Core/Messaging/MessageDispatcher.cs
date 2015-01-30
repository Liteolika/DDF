using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DDF.Core.Messaging
{
    public class MessageDispatcher<T> : IMessageDispatcher<T>
        where T : IMessage
    {

        private Dictionary<Type, HandleObject> _commandHandlers =
            new Dictionary<Type, HandleObject>();

        private readonly bool _allowMultipleHandlers;

        /// <summary>
        /// Constructor for the MessageDispatcher
        /// </summary>
        /// <param name="allowMultipleHandlers">Allow the registerhandler to register several instances of IMessage in the same instance</param>
        public MessageDispatcher(bool allowMultipleHandlers)
        {
            this._allowMultipleHandlers = allowMultipleHandlers;
        }


        /// <summary>
        /// Register an instance of an object to handle events for the IMessage
        /// </summary>
        /// <param name="handlerObject">Instance of a class</param>
        public void RegisterHandler(object handlerObject)
        {
            var handlerType = handlerObject.GetType();

            //var handlerMethods = handlerType.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance);
            var handlerMethods = handlerType.GetMethods();

            foreach (var method in handlerMethods)
            {
                var methodParameter = method.GetParameters().SingleOrDefault();

                if (methodParameter != null)
                {
                    Type parameterType = methodParameter.ParameterType;

                    var parameterInterfaces = methodParameter.ParameterType.GetInterfaces();

                    foreach (var parameterInterface in parameterInterfaces)
                    {
                        if (parameterInterface == typeof(T))
                        {

                            if (!_allowMultipleHandlers)
                                if (_commandHandlers.ContainsKey(parameterType))
                                    throw new InvalidOperationException("Handler is already registred for " + parameterType.FullName + ".");

                            HandleObject ch = new HandleObject
                            {
                                handler = handlerObject,
                                method = method
                            };
                            _commandHandlers.Add(parameterType, ch);

                        }
                    }

                }

            }

        }

        /// <summary>
        /// Dispatches an IMessage to the registred handlers.
        /// </summary>
        /// <param name="message">A class derived from IMessage</param>
        public void Dispatch(T message)
        {
            var commandType = message.GetType();

            HandleObject handle = null;
            _commandHandlers.TryGetValue(commandType, out handle);
            if (handle != null)
            {
                handle.method.Invoke(handle.handler, new[] { message as object });
            }


        }

        public void Dispatch(IEnumerable<T> messages)
        {
            foreach (var message in messages)
            {
                Dispatch(message);
            }
        }

        private class HandleObject
        {
            public object handler { get; set; }
            public MethodInfo method { get; set; }
        }


        
    }
}
