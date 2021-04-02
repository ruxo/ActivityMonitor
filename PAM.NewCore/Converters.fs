namespace PAM.Core.Converters

open System.Windows.Data
open System

// Convert Option<T> -> T
type OptionConverter<'T>() =
    interface IValueConverter with
        member _.Convert(value, targetType, parameter, culture) =
            let v = value :?> 'T option
            if v.IsSome
            then v.Value :> obj
            else null :> obj

        member _.ConvertBack(value, targetType, parameter, culture) = null :> obj