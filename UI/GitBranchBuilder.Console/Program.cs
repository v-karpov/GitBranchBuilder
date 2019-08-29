using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using Autofac;

using GitBranchBuilder.Components.Holders;
using GitBranchBuilder.Providers;

using MoreLinq;

namespace GitBranchBuilder
{
    class Program
    {
#if DEBUG
        static IList<string> loadedTypes = new List<string>();
#endif
        static IContainer BuildAutofacContainer()
        {
            var builder = new ContainerBuilder();

            builder.RegisterGeneric(typeof(DefaultProvider<>))
                .As(typeof(IProvider<>))
                .SingleInstance();

            void RegisterAssembly(Assembly assembly)
            {
                static IEnumerable<Type> SelectTargetTypes(Type type)
                {
#if DEBUG
                    loadedTypes.Add(type.FullName);
#endif
                    var interfaces = type.GetInterfaces()
                        .Where(iface => !iface.FullName.StartsWith("System"));

                    return interfaces;
                }

                builder.RegisterAssemblyTypes(assembly)
                    .Where(x => !(x.IsInterface || x.IsAbstract))
                    .As(SelectTargetTypes)
                    .AsSelf()
                    .SingleInstance()
                    .PropertiesAutowired();
            }

            void RegisterGenericSingleton(Type type)
                => builder.RegisterGeneric(type).SingleInstance();

            var generics = new[]
            {
                typeof(Holder<>),
                typeof(MultiHolder<>),
                typeof(MaybeHolder<>),
                typeof(MaybeHolder<>.MaybeProvider),
                typeof(Jobs.FetchJob<>),
                typeof(Jobs.RetryBuildJob<>),
            };

            var plugins = Directory
              .GetFiles($"{Environment.CurrentDirectory}", "GitBranchBuilder*.dll", SearchOption.TopDirectoryOnly)
              .Select(Assembly.LoadFile);

            generics.ForEach(RegisterGenericSingleton);
            plugins.ForEach(RegisterAssembly);

            RegisterAssembly(Assembly.GetEntryAssembly());

            return builder.Build();
        }
     
        static async Task Main(string[] args)
        {
            using var container = BuildAutofacContainer();
            var application = container.Resolve<ConsoleApplication>();

            await application.Run(loadedTypes);
        }
    }
}
