using AutoFixture.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AutoNSubstitute.Shortcuts
{
    public class SubstituteSpecification : IRequestSpecification
    {
        public class Builder
        {
            private readonly SubstituteSpecification specification = new SubstituteSpecification();

            internal Builder() { }

            internal Builder With(Type type)
            {
                if (CanSubstitute(type))
                    specification.Included.Add(type);
                return this;
            }

            internal Builder Except(Type type)
            {
                specification.Excluded.Add(type);
                return this;
            }

            public SubstituteSpecification Create() => specification;

            private static bool CanSubstitute(Type type)
               => !type.IsSealed &&
                !type.IsValueType &&
                type.Namespace != nameof(System) &&
                type.GetConstructors(BindingFlags.Public | BindingFlags.Instance).Any() &&
                (type.GetMethods(BindingFlags.Public | BindingFlags.Instance).Any(method => method.IsVirtual) ||
                type.GetProperties(BindingFlags.Public | BindingFlags.Instance).Any(property => CanSubstitute(property)));

            private static bool CanSubstitute(PropertyInfo property)
                => property.GetMethod != null && property.GetMethod.IsVirtual || property.SetMethod != null && property.SetMethod.IsVirtual;
        }

        internal HashSet<Type> Included { get; } = new HashSet<Type>();
        internal HashSet<Type> Excluded { get; } = new HashSet<Type>();

        public bool IsSatisfiedBy(object request)
        {
            var type = request as Type ??
                request.GetType().GetProperty("ParameterType")?.GetValue(request) as Type;
            return !Excluded.Contains(type) && Included.Contains(type);
        }

        public static Builder For<T>() where T : class
        {
            var builder = new Builder().Except<T>();
            foreach (var type in GetAssociatedTypes(typeof(T)).Distinct().Where(t => !(t is T)))
                builder.With(type);
            return builder;
        }

        public static Builder ForNamespace(string @namespace)
        {
            var builder = new Builder();
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                try
                {
                    foreach (var type in assembly.GetTypes().Where(t => t.IsClass && t.Namespace != null && t.Namespace.StartsWith(@namespace)).Distinct())
                    {
                        builder.With(type);
                    }
                }
                catch (ReflectionTypeLoadException) { }
            }
            return builder;
        }

        public static Builder With<T>() where T : class => new Builder().With<T>();

        public static Builder Except<T>() where T : class => new Builder().Except<T>();

        private static IEnumerable<Type> GetAssociatedTypes(Type type)
        {
            foreach (var parameter in type.GetConstructors().SelectMany(ctor => ctor.GetParameters()))
                foreach (var t in GetAssociatedTypes(parameter.ParameterType))
                    yield return t;

            foreach (var propertyType in type.GetProperties(BindingFlags.Public).Select(p => p.PropertyType))
                foreach (var t in GetAssociatedTypes(propertyType))
                    yield return t;

            foreach (var method in type.GetMethods(BindingFlags.Public).Where(m => m.IsVirtual))
                foreach (var t in GetAssociatedTypes(method.ReturnType))
                    yield return t;

            yield return type;
        }
    }
}
