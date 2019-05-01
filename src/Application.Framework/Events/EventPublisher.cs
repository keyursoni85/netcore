/*
Application - The Multi server multi operating system control panel © copyright 2018.

Application is free software; you can redistribute it and/or modify it under the terms of the GNU Lesser General Public License 3.0 as published by the Free Software Foundation.

This file is part of the Application project and is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the GNU Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public License along with this file; if not, please contact Application at https://www.Application.com/contact

https://www.gnu.org/licenses/lgpl-3.0.txt
*/
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Framework.DependencyResolver;

namespace Application.Framework.Events
{
    public class EventPublisher : IEventPublisher
    {
        private readonly IResolver _resolver;

        public EventPublisher(IResolver resolver)
        {
            _resolver = resolver;
        }

        public void Publish<TEvent>(TEvent @event) where TEvent : IEvent
        {
            var eventHandlers = GetHandlers<IEventHandler<TEvent>, TEvent>(@event);

            foreach (var handler in eventHandlers)
                handler.Handle(@event);
        }

        public async Task PublishAsync<TEvent>(TEvent @event) where TEvent : IEvent
        {
            var eventHandlers = GetHandlers<IEventHandlerAsync<TEvent>, TEvent>(@event);

            foreach (var handler in eventHandlers)
                await handler.HandleAsync(@event);
        }

        private IEnumerable<THandler> GetHandlers<THandler, TEvent>(TEvent @event) where TEvent : IEvent
        {
            if (@event == null)
                throw new ArgumentNullException(nameof(@event));

            var eventHandlers = _resolver.ResolveAll<THandler>();

            return eventHandlers;
        }
    }
}
