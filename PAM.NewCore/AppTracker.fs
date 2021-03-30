namespace PAM.Core.Tracker

open System
open System.Collections.Generic
open PAM.Core
open System.Diagnostics
open System.Windows.Media
open PAM.Core.Utils

type ApplicationUsage = {
    Pid: int
    Begin: DateTime
    End: DateTime option
}
with
    member this.TotalTime = 
        let endTime = this.End |> Option.defaultWith (fun _ -> DateTime.Now)
        in endTime - this.Begin

    static member totalTime (au: ApplicationUsage) = au.TotalTime

type ApplicationInfo(id: int, name: string, title: string, path: string option) =
    let usage = List<ApplicationUsage>()
    let mutable trackingId = None

    member _.Id = id
    member _.Name = name
    member _.Title = title
    member _.Path = path
    member val Icon :ImageSource option = None with get,set
    member _.Usages = usage :> ICollection<ApplicationUsage>

    member this.StartTrack(pid) =
        this.StopTrack()
        let u = { Pid=pid; Begin=DateTime.Now; End=None }
        trackingId <- Some usage.Count
        usage.Add(u)

    member this.StopTrack() =
        match trackingId with
        | Some id -> usage.[id] <- { usage.[id] with End = Some DateTime.Now }; trackingId <- None
        | None -> ()

    member _.GetTotalUsageTime() = usage |> Seq.fold (fun sum u -> sum + u.TotalTime) TimeSpan.Zero

type AppTracker() =
    let mutable activeApp :ApplicationInfo option = None
    let trackedApps = Dictionary<string,ApplicationInfo>()

    let toApplicationInfo(p: Process) =
        let path = Utils.tryOption (fun () -> p.MainModule.FileName)
        in ApplicationInfo(p.Id, p.ProcessName, p.MainWindowTitle, path)

    let noActive() =
        match activeApp with
        | Some a -> a.StopTrack()
                    activeApp <- None
        | None -> ()

    let track(p: Process) =
        let processName = p.ProcessName
        let currentActiveName = activeApp |> Option.map (fun ai -> ai.Name) |> Option.defaultValue processName
        if processName <> currentActiveName then noActive()
        let appInfo = trackedApps.TryGet(processName)
                      |> Option.defaultWith (fun _ -> let appInfo = toApplicationInfo(p)
                                                      in trackedApps.Add(processName, appInfo)
                                                      appInfo)
        appInfo.StartTrack(p.Id)
        activeApp <- Some appInfo
        activeApp

    member _.Active = activeApp

    member _.Update(processState) =
        match processState with
        | Platform.NoActive -> noActive(); None
        | Platform.Active p -> use v = p in track(v)
        | Platform.Inactive p -> noActive(); p.Dispose(); None