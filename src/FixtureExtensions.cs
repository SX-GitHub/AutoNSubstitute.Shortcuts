using AutoFixture;
using NSubstitute;
using NSubstitute.Core;
using NSubstitute.Exceptions;
using NSubstitute.Extensions;
using System;
using System.Threading.Tasks;

namespace AutoNSubstitute.Shortcuts
{
    public static class FixtureExtensions
    {
        public static ConfiguredCall Returns<T, TReturn>(this IFixture fixture, Func<T, Task> call, TReturn @return) where T : class
        {
            var substitue = fixture.Freeze<T>();
            if (substitue is ICallRouterProvider)
            {
                var auto = Task.FromResult(@return);
                return call(substitue.Configure()).ReturnsForAnyArgs((CallInfo _) => auto);
            }
            throw new NullSubstituteReferenceException();
        }

        public static ConfiguredCall Returns<T, TReturn>(this IFixture fixture, Func<T, object> call, TReturn @return) where T : class
        {
            var substitue = fixture.Freeze<T>();
            if (substitue is ICallRouterProvider)
            {
                return call(substitue.Configure()).ReturnsForAnyArgs((CallInfo _) => @return);
            }
            throw new NullSubstituteReferenceException();
        }

        public static ConfiguredCall ReturnsAuto<T, TReturn>(this IFixture fixture, Func<T, Task> call) where T : class
        {
            var substitue = fixture.Freeze<T>();
            if (substitue is ICallRouterProvider)
            {
                var auto = Task.FromResult(fixture.Create<TReturn>());
                return call(substitue.Configure()).ReturnsForAnyArgs((CallInfo _) => auto);
            }
            throw new NullSubstituteReferenceException();
        }

        public static ConfiguredCall ThrowsAsyncAuto<T, TException>(this IFixture fixture, Func<T, Task> call) where T : class
            where TException : Exception
        {
            var substitue = fixture.Freeze<T>();
            if (substitue is ICallRouterProvider)
            {
                var auto = fixture.Create<TException>();
                return call(substitue.Configure()).ReturnsForAnyArgs((CallInfo _) => throw auto);
            }
            throw new NullSubstituteReferenceException();
        }

        public static ConfiguredCall ReturnsAuto<T, TReturn>(this IFixture fixture, Func<T, object> call) where T: class
        { 
            var substitue = fixture.Freeze<T>();
            if (substitue is ICallRouterProvider)
            {
                var auto = fixture.Create<TReturn>();
                return call(substitue.Configure()).ReturnsForAnyArgs((CallInfo _) => auto);
            }
            throw new NullSubstituteReferenceException();
        }

        public static ConfiguredCall ThrowsAuto<T, TException>(this IFixture fixture, Func<T, object> call) where T: class where TException : Exception
        {
            var substitue = fixture.Freeze<T>();
            if (substitue is ICallRouterProvider)
            {
                var auto = fixture.Create<TException>();
                return call(substitue.Configure()).ReturnsForAnyArgs((CallInfo _) => throw auto);
            }
            throw new NullSubstituteReferenceException();
        }

        public static (T1, T2) Freeze<T1, T2>(this IFixture fixture)
            => (fixture.Freeze<T1>(), fixture.Freeze<T2>());

        public static (T1, T2, T3) Freeze<T1, T2, T3>(this IFixture fixture)
            => (fixture.Freeze<T1>(), fixture.Freeze<T2>(), fixture.Freeze<T3>());

        public static (T1, T2, T3, T4) Freeze<T1, T2, T3, T4>(this IFixture fixture)
            => (fixture.Freeze<T1>(), fixture.Freeze<T2>(), fixture.Freeze<T3>(), fixture.Freeze<T4>());
    }
}
