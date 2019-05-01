using System;
using System.Linq;
using Microsoft.Extensions.Options;
using FuseCP.Data.Configuration;
using FuseCP.Framework.DependencyResolver;

namespace FuseCP.Data
{
    public class ContextFactory : IContextFactory
    {
        private Configuration.Data DataConfiguration { get; }
        private ConnectionStrings ConnectionStrings { get; }
        private readonly IResolver _resolver;

        public ContextFactory(IOptions<Configuration.Data> dataOptions,
            IResolver resolver, IOptions<ConnectionStrings> connectionStringsOption)
        {
            DataConfiguration = dataOptions.Value;
            ConnectionStrings = connectionStringsOption.Value;
            _resolver = resolver;
        }

        public FuseCPDbContext Create()
        {
            var dataProvider = _resolver.ResolveAll<IDataProvider>().SingleOrDefault(x => x.Provider == DataConfiguration.Provider);

            if (dataProvider == null)
                throw new Exception("The Data Provider entry in appsettings.json is empty or the one specified has not been found!");

            return dataProvider.CreateDbContext(ConnectionStrings.DefaultConnection);
        }
    }
}
