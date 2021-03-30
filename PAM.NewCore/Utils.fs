module PAM.Core.Utils

open System.Collections.Generic

let inline tryOption f =
    try
        Some <| f()
    with
    | _ -> None

type Dictionary<'k,'v> with
    member d.TryGet(key) =
        let ok, v = d.TryGetValue(key)
        if ok then Some(v) else None