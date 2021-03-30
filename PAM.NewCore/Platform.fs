module PAM.Core.Platform

open System
open System.Runtime.InteropServices
open System.Diagnostics

[<AutoOpen>]
module private Interop =
    [<Struct; StructLayout(LayoutKind.Sequential)>]
    type LastInputInfo =
        val mutable cbSize : uint
        val mutable dwTime : int32

    [<DllImport("user32.dll")>]
    extern IntPtr GetForegroundWindow()

    [<DllImport("user32.dll", CharSet=CharSet.Auto, SetLastError=true)>]
    extern int GetWindowThreadProcessId(HandleRef handle, int& processId)

    [<DllImport("user32.dll")>]
    extern bool GetLastInputInfo(LastInputInfo& plii)

    [<DllImport("Kernel32.dll")>]
    extern uint32 WTSGetActiveConsoleSessionId()

    let NO_SESSION_ID = ~~~0u

let getForegroundWindow() =
    let handle = GetForegroundWindow()
    let mutable processId = 0
    GetWindowThreadProcessId(HandleRef(null,handle), &processId) |> ignore
    if processId <> 0
    then Some <| Process.GetProcessById(processId)
    else None

let getLastInputInfo() =
    let mutable inputInfo = LastInputInfo(cbSize = uint(sizeof<LastInputInfo>))
    if not <| GetLastInputInfo(&inputInfo) then failwith "Cannot retrieve input info!"
    inputInfo.dwTime

let getActiveConsoleSessionId() =
    let id = WTSGetActiveConsoleSessionId()
    if id <> NO_SESSION_ID
    then Some id
    else None

type ActiveProcessState =
| NoActive
| Active of Process
| Inactive of Process

let getActiveProcess(idleTimeThreshold: TimeSpan) =
    match getForegroundWindow() with
    | Some p -> let idleTime = (Environment.TickCount - getLastInputInfo()) / 1000
                if idleTime < idleTimeThreshold.Seconds && getActiveConsoleSessionId().IsSome
                then Active p
                else Inactive p
    | None -> NoActive