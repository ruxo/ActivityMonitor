using System;
using Microsoft.FSharp.Core;

namespace PAM.Core
{
    public static class FSharpInterop
    {
        public static bool IsSome<T>(this FSharpOption<T> opt) => FSharpOption<T>.get_IsSome(opt);
        public static bool IsNone<T>(this FSharpOption<T> opt) => FSharpOption<T>.get_IsNone(opt);

        public static T GetOrElse<T>(this FSharpOption<T> opt, T def) => OptionModule.DefaultValue(def, opt);

        public static FSharpOption<B> Bind<A, B>(this FSharpOption<A> opt, Func<A, FSharpOption<B>> mapper) =>
            opt.IsSome() ? mapper(opt.Value) : FSharpOption<B>.None;
        public static FSharpOption<B> Map<A, B>(this FSharpOption<A> opt, Func<A, B> mapper) =>
            opt.IsSome() ? FSharpOption<B>.Some(mapper(opt.Value)) : FSharpOption<B>.None;
    }
}