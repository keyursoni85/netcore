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

namespace Application.Framework.Queries
{
    public class QueryDispatcher : IQueryDispatcher
    {
        private readonly IResolver _resolver;

        public QueryDispatcher(IResolver resolver)
        {
            _resolver = resolver;
        }

        public TResult Dispatch<TQuery, TResult>(TQuery query) where TQuery : IQuery
        {
            var queryHandler = GetHandler<IQueryHandler<TQuery, TResult>, TQuery>(query);

            return queryHandler.Retrieve(query);
        }

        public async Task<TResult> DispatchAsync<TQuery, TResult>(TQuery query) where TQuery : IQuery
        {
            var queryHandler = GetHandler<IQueryHandlerAsync<TQuery, TResult>, TQuery>(query);

            return await queryHandler.RetrieveAsync(query);
        }

        private THandler GetHandler<THandler, TQuery>(TQuery query) where TQuery : IQuery
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));

            var queryHandler = _resolver.Resolve<THandler>();

            if (queryHandler == null)
                throw new Exception($"No handler found for query '{query.GetType().FullName}'");

            return queryHandler;
        }
    }
}