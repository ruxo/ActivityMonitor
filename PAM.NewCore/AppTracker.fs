namespace PAM.Core.Tracker

open System
open System.Collections.Generic
open PAM.Core
open System.Diagnostics
open System.Windows.Media
open System.Collections.ObjectModel
open System.ComponentModel
open System.Drawing
open System.Drawing.Imaging
open System.IO
open System.Windows.Media.Imaging

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

type ApplicationInfo(id: int, name: string, title: string, path: string option, icon: ImageSource option) =
    let usage = List<ApplicationUsage>()
    let mutable trackingId = None

    member _.Id = id
    member _.Name = name
    member _.Title = title
    member _.Path = path
    member _.Icon = icon
    member _.Usages = usage :> ICollection<ApplicationUsage>
    member _.TotalUsageTime = usage |> Seq.fold (fun sum u -> sum + u.TotalTime) TimeSpan.Zero

    member this.StartTrack(pid) =
        this.StopTrack()
        let u = { Pid=pid; Begin=DateTime.Now; End=None }
        trackingId <- Some usage.Count
        usage.Add(u)

    member this.StopTrack() =
        match trackingId with
        | Some id -> usage.[id] <- { usage.[id] with End = Some DateTime.Now }; trackingId <- None
        | None -> ()


type AppTrackerContext() =
    let mutable active :ApplicationInfo option = None
    let propertyChanged = Event<_,_>()

    member this.Active
        with get() = active
        and set v = active <- v; propertyChanged.Trigger(this, PropertyChangedEventArgs("Active"))

    member val Tracked = ObservableCollection<ApplicationInfo>() with get

    interface INotifyPropertyChanged with
        [<CLIEvent>]
        member this.PropertyChanged = propertyChanged.Publish

    member this.TryGet processName = this.Tracked |> Seq.filter (fun ai -> ai.Name = processName) |> Seq.tryHead
    member this.Add(appInfo: ApplicationInfo) =
        match this.TryGet appInfo.Name with
        | Some ai -> failwithf "App %s is already existed." appInfo.Name
        | None -> this.Tracked.Add appInfo

module AppTracker =
    let private loadIcon path =
        use icon = Icon.ExtractAssociatedIcon path
        if icon = null
        then None
        else
            use bmp = icon.ToBitmap()
            let stream = new MemoryStream()
            bmp.Save(stream, ImageFormat.Png)

            let bmpImage = BitmapImage()
            bmpImage.BeginInit()
            stream.Seek(0L, SeekOrigin.Begin) |> ignore
            bmpImage.StreamSource <- stream
            bmpImage.EndInit()
            Some (bmpImage :> ImageSource)

    let private toApplicationInfo(p: Process) =
        let path = Utils.tryOption (fun () -> p.MainModule.FileName)
        in ApplicationInfo(p.Id, p.ProcessName, p.MainWindowTitle, path, path |> Option.bind loadIcon)

    let private noActive(ctx: AppTrackerContext) =
        match ctx.Active with
        | Some a -> a.StopTrack()
                    ctx.Active <- None
        | None -> ()

    let private track(ctx: AppTrackerContext, p: Process) =
        let processName = p.ProcessName
        let currentActiveName = ctx.Active |> Option.map (fun ai -> ai.Name) |> Option.defaultValue processName
        if processName <> currentActiveName then noActive(ctx)
        let appInfo = ctx.TryGet(processName)
                      |> Option.defaultWith (fun _ -> let appInfo = toApplicationInfo(p)
                                                      in ctx.Add(appInfo)
                                                      appInfo)
        appInfo.StartTrack(p.Id)
        ctx.Active <- Some appInfo

    let update(processState) (ctx: AppTrackerContext) =
        match processState with
        | Platform.NoActive -> noActive(ctx)
        | Platform.Active p -> use v = p in track(ctx, v)
        | Platform.Inactive p -> noActive(ctx); p.Dispose()