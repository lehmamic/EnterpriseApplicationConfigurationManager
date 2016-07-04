using System;
using System.Collections.Generic;
using System.Linq;
using Zuehlke.Eacm.Web.Backend.Diagnostics;

namespace Zuehlke.Eacm.Web.Backend.CQRS
{
    public class MessageDispatcher
    {
        private readonly Dictionary<Type, Action<object>> commandHandlers = new Dictionary<Type, Action<object>>();      
        private readonly Dictionary<Type, List<Action<object>>> eventSubscribers = new Dictionary<Type, List<Action<object>>>();
        private readonly IEventStore eventStore;

        public MessageDispatcher(IEventStore eventStore)
        {
            this.eventStore = eventStore.ArgumentNotNull(nameof(eventStore));
        }

        public void SendCommand<TCommand>(TCommand command)
            where TCommand : ICommand
        {
            command.ArgumentNotNull(nameof(command));

            if (commandHandlers.ContainsKey(typeof(TCommand)))
            {
                commandHandlers[typeof(TCommand)](command);
            }
            else
            {
                throw new ArgumentException("No command handler registered for " + typeof(TCommand).Name);
            }
        }

        public void AddCommandHandler<TCommand, TAggregate>()
            where TAggregate : IAggregateRoot, new()
            where TCommand : ICommand
        {
            if (this.commandHandlers.ContainsKey(typeof(TCommand)))
            {
                throw new ArgumentException("Command handler already registered for " + typeof(TCommand).Name);
            }
            
            this.commandHandlers.Add(typeof(TCommand), command =>
                {
                    // Create an empty aggregate.
                    var aggregate = new TAggregate();

                    // Load the aggregate with events.
                    aggregate.Id = ((dynamic)command).Id;

                    IEnumerable<IEvent> history = this.eventStore.LoadEvents<TAggregate>(aggregate.Id);
                    aggregate.HandleEvents(history);
                    
                    // With everything set up, we invoke the command handler, collecting the
                    // events that it produces.
                    var commandHandler = (ICommandHandler<TCommand>)aggregate;
                    var resultEvents = commandHandler.Handle((TCommand)command);
                    
                    // Store the events in the event store.
                    if (resultEvents.Count() > 0)
                    {    
                        this.eventStore.SaveEvents<TAggregate>(aggregate.Id, resultEvents);
                    }

                    // Publish them to all subscribers.
                    foreach (var e in resultEvents)
                    {
                        this.PublishEvent(e);
                    }    
                });
        }

        public void AddSubscriber<TEvent>(IEventHandler<TEvent> eventHandler)
            where TEvent : IEvent
        {
            eventHandler.ArgumentNotNull(nameof(eventHandler));

            if (!this.eventSubscribers.ContainsKey(typeof(TEvent)))
            {
                this.eventSubscribers.Add(typeof(TEvent), new List<Action<object>>());
            }

            this.eventSubscribers[typeof(TEvent)].Add(e => eventHandler.Handle((TEvent)e));
        }

        private void PublishEvent(object e)
        {
            var eventType = e.GetType();
            if (this.eventSubscribers.ContainsKey(eventType))
            {
                foreach (var subscriber in this.eventSubscribers[eventType])
                {
                	subscriber(e);
                }
            }
        }
    }
}
