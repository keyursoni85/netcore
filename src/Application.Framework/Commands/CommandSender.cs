/*
Application - The Multi server multi operating system control panel © copyright 2018.

Application is free software; you can redistribute it and/or modify it under the terms of the GNU Lesser General Public License 3.0 as published by the Free Software Foundation.

This file is part of the Application project and is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the GNU Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public License along with this file; if not, please contact Application at https://www.Application.com/contact

https://www.gnu.org/licenses/lgpl-3.0.txt
*/

using System;
using System.Threading.Tasks;
using Application.Framework.DependencyResolver;
using Application.Framework.Domain;
using Application.Framework.Events;

namespace Application.Framework.Commands
{
    public class CommandSender : ICommandSender
    {
        private readonly IResolver _resolver;
        private readonly IEventPublisher _eventPublisher;
        private readonly IEventStore _eventStore;

        public CommandSender(IResolver resolver,
            IEventPublisher eventPublisher,
            IEventStore eventStore)
        {
            _resolver = resolver;
            _eventPublisher = eventPublisher;
            _eventStore = eventStore;
        }

        public void Send<TCommand>(TCommand command, bool publishEvents = true) where TCommand : ICommand
        {
            var commandHandler = GetHandler<ICommandHandler<TCommand>, TCommand>(command);

            var events = commandHandler.Handle(command);

            if (!publishEvents)
                return;

            foreach (var @event in events)
            {
                var concreteEvent = EventFactory.CreateConcreteEvent(@event);
                _eventPublisher.Publish(concreteEvent);
            }
        }

        public void Send<TCommand, TAggregate>(TCommand command, bool publishEvents = true)
            where TCommand : ICommand
            where TAggregate : IAggregateRoot
        {
            var commandHandler = GetHandler<ICommandHandler<TCommand>, TCommand>(command);

            var events = commandHandler.Handle(command);

            foreach (var @event in events)
            {
                var concreteEvent = EventFactory.CreateConcreteEvent(@event);

                _eventStore.SaveEvent<TAggregate>((IDomainEvent)concreteEvent);
                
                if (!publishEvents)
                    continue;

                _eventPublisher.Publish(concreteEvent);
            }
        }

        public async Task SendAsync<TCommand>(TCommand command, bool publishEvents = true) where TCommand : ICommand
        {
            var commandHandler = GetHandler<ICommandHandlerAsync<TCommand>, TCommand>(command);

            var events = await commandHandler.HandleAsync(command);

            if (!publishEvents)
                return;

            foreach (var @event in events)
            {
                var concreteEvent = EventFactory.CreateConcreteEvent(@event);
                await _eventPublisher.PublishAsync(concreteEvent);
            }
        }

        public async Task SendAsync<TCommand, TAggregate>(TCommand command, bool publishEvents = true) 
            where TCommand : ICommand 
            where TAggregate : IAggregateRoot
        {
            var commandHandler = GetHandler<ICommandHandlerAsync<TCommand>, TCommand>(command);

            var events = await commandHandler.HandleAsync(command);

            foreach (var @event in events)
            {
                var concreteEvent = EventFactory.CreateConcreteEvent(@event);

                await _eventStore.SaveEventAsync<TAggregate>((IDomainEvent)concreteEvent);

                if (!publishEvents)
                    continue;

                await _eventPublisher.PublishAsync(concreteEvent);
            }
        }

        private THandler GetHandler<THandler, TCommand>(TCommand command) where TCommand : ICommand 
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            var commandHandler = _resolver.Resolve<THandler>();

            if (commandHandler == null)
                throw new Exception($"No handler found for command '{command.GetType().FullName}'");

            return commandHandler;
        }
    }
}
