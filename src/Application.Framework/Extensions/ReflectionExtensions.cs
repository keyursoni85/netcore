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
using System.Linq;
using System.Reflection;
using Microsoft.DotNet.PlatformAbstractions;
using Microsoft.Extensions.DependencyModel;

namespace Application.Framework.Extensions
{
    public static class ReflectionExtensions
    {
        public static bool HasProperty(this object obj, string name)
        {
            return obj.GetType().GetRuntimeProperty(name) != null;
        }

        public static bool HasValue(this object obj, string name)
        {
            var currentProperty = obj.GetType().GetRuntimeProperty(name);
            if (currentProperty == null)
                return false;
            var caurrentValue = currentProperty.GetValue(obj);
            var defaultValue = Activator.CreateInstance(obj.GetType()).GetType().GetRuntimeProperty(name).GetValue(obj);
            return caurrentValue != defaultValue;
        }

        public static void SetValue(this object obj, string name, object value)
        {
            obj.GetType().GetRuntimeProperty(name).SetValue(obj, value);
        }

        public static IEnumerable<T> GetImplementationsOf<T>(this Assembly assembly)
        {
            var result = new List<T>();

            var types = assembly.GetTypes()
                .Where(t => t.GetTypeInfo().IsClass && !t.GetTypeInfo().IsAbstract && typeof(T).IsAssignableFrom(t))
                .ToList();

            foreach (var type in types)
            {
                var instance = (T)Activator.CreateInstance(type);
                result.Add(instance);
            }

            return result;
        }

        public static IEnumerable<T> GetImplementationsOf<T>(this IEnumerable<Assembly> assemblies)
        {
            var result = new List<T>();

            foreach (var assembly in assemblies)
            {
                var types = assembly.GetTypes()
                    .Where(t => t.GetTypeInfo().IsClass && !t.GetTypeInfo().IsAbstract && typeof(T).IsAssignableFrom(t))
                    .ToList();

                foreach (var type in types)
                {
                    var instance = (T)Activator.CreateInstance(type);
                    result.Add(instance);
                }
            }

            return result;
        }

        public static IEnumerable<Assembly> GetRuntimeAssemblies()
        {
            var result = new List<Assembly>();
            var runtimeId = RuntimeEnvironment.GetRuntimeIdentifier();
            var assemblyNames = DependencyContext.Default.GetRuntimeAssemblyNames(runtimeId);
            foreach (var assemblyName in assemblyNames)
            {
                var assembly = Assembly.Load(assemblyName);
                result.Add(assembly);
            }
            return result;
        }
    }
}
