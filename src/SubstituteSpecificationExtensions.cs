namespace AutoNSubstitute.Shortcuts
{
    public static class SubstituteSpecificationExtensions
    {
        public static SubstituteSpecification.Builder With<T>(this SubstituteSpecification.Builder builder)
            where T : class
            => builder.With(typeof(T));

        public static SubstituteSpecification.Builder With<T1, T2>(this SubstituteSpecification.Builder builder)
            where T1 : class
            where T2 : class
            => builder.With(typeof(T1)).With(typeof(T2));

        public static SubstituteSpecification.Builder With<T1, T2, T3>(this SubstituteSpecification.Builder builder)
            where T1 : class
            where T2 : class
            where T3 : class
            => builder.With(typeof(T1)).With(typeof(T2)).With(typeof(T3));

        public static SubstituteSpecification.Builder Except<T>(this SubstituteSpecification.Builder builder)
            where T : class
            => builder.Except(typeof(T));

        public static SubstituteSpecification.Builder Except<T1, T2>(this SubstituteSpecification.Builder builder)
            where T1 : class
            where T2 : class
            => builder.Except(typeof(T1)).Except(typeof(T2));

        public static SubstituteSpecification.Builder Except<T1, T2, T3>(this SubstituteSpecification.Builder builder)
            where T1 : class
            where T2 : class
            where T3 : class
            => builder.Except(typeof(T1)).Except(typeof(T2)).Except(typeof(T3));
    }
}
