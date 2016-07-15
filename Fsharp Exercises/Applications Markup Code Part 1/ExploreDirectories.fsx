#if INTERACTIVE
#r "PresentationCore.dll"
#r "PresentationFramework.dll"
#r "System.Xaml.dll"
#r "WindowsBase.dll"
#r "UIAutomationTypes.dll"
#endif

open Microsoft.Win32
open System
open System.IO
open System.Diagnostics
open System.ComponentModel
open System.Windows
open System.Windows.Input
open System.Windows.Controls
open System.Windows.Controls.Primitives
open System.Windows.Media
open System.Windows.Media.Imaging
open System.Windows.Shapes
open System.Windows.Documents

type FileSystemInfoButton(info: FileSystemInfo, str: string option) as t =
    inherit Button()

    do 
        t.Content <- match str with Some x -> box x | None -> box info
        match info with :? DirectoryInfo -> t.FontWeight <- FontWeights.Bold | _ -> ()
        t.Margin <- Thickness 10.0

    new () = FileSystemInfoButton(DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)),None)
    new (info: FileSystemInfo) = FileSystemInfoButton(info,None)
    new (info: FileSystemInfo, str: string) = FileSystemInfoButton(info,Some str)
        
    override t.OnClick() =
        match info with
        | :? FileInfo as info -> Process.Start(info.FullName) |> ignore
        | :? DirectoryInfo as dir -> 
            //Application.Current.MainWindow.Title <- dir.FullName // Does not work in F# Interactive
            let pnl = t.Parent :?> Panel

            pnl.Parent :?> ScrollViewer
            |> fun x -> x.Parent :?> Window
            |> fun x -> x.Title <- dir.FullName

            pnl.Children.Clear()
            if dir.Parent <> null then
                pnl.Children.Add(FileSystemInfoButton(dir.Parent,"..")) |> ignore
            for inf in dir.GetFileSystemInfos() do
                pnl.Children.Add(FileSystemInfoButton inf) |> ignore
            base.OnClick()
        | _ -> failwith "Wrong type."

type ExploreDirectories() as t =
    inherit Window()

    do 
        t.MinHeight <- 300.0
        t.MinWidth <- 300.0
        t.MaxHeight <- 600.0
        t.MaxWidth <- 600.0
        t.WindowStartupLocation <- WindowStartupLocation.CenterScreen
        t.SizeToContent <- SizeToContent.WidthAndHeight

        t.Title <- "Explore Directories"

        let scroll = ScrollViewer()
        t.Content <- scroll

        let wrap = WrapPanel()
        scroll.Content <- wrap

        wrap.Children.Add(FileSystemInfoButton()) |> ignore

try ExploreDirectories().Show()
with e -> MessageBox.Show(e.Message,"Exception Error") |> ignore

//[<STAThreadAttribute>]
//do
//    try Application().Run(TuneTheRadio()) |> ignore
//    with e -> MessageBox.Show(e.Message,"Exception Error") |> ignore
