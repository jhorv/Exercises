#if INTERACTIVE
#r "PresentationCore.dll"
#r "PresentationFramework.dll"
#r "System.Xaml.dll"
#r "WindowsBase.dll"
#r "UIAutomationTypes.dll"
#load "Bindings.fs"
#endif

open Bindings
open Microsoft.Win32
open System
open System.IO
open System.Text
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
open System.Windows.Threading
open System.Windows.Data

[<STAThreadAttribute>]
try 
    let dock = DockPanel(LastChildFill=false)
    let win = Window(Title = "Craft The Toolbar", Content = dock)
    let toolbar = ToolBar() |> addChildren dock |> DockPanel.DockTop
    
    let addCommands (comm: RoutedUICommand,imgName: string) =
        #if INTERACTIVE
        let bitmap = BitmapImage(Uri(Path.Combine(__SOURCE_DIRECTORY__,"Images","Images"imgName)))
        #else
        let bitmap = BitmapImage(Uri(Path.Combine("pack://application:,,/Images/",imgName)))
        #endif
        let img = Image(Source=bitmap,Stretch=Stretch.None)
        let tip = ToolTip(Content=comm.Text)
        Button(Command=comm,Content=img,ToolTip=tip) |> addItems toolbar |> ignore
        CommandBinding(comm,(fun _ _ -> MessageBox.Show(sprintf "%s not yet implemented" comm.Name, win.Title) |> ignore))
        |> win.CommandBindings.Add |> ignore
    [|
    ApplicationCommands.New,"NewDocumentHS.png"
    ApplicationCommands.Open,"openHS.png"
    ApplicationCommands.Save,"saveHS.png"
    ApplicationCommands.Print,"PrintHS.png"
    |] |> Array.iter addCommands
    toolbar.Items.Add (Separator()) |> ignore
    [|
    ApplicationCommands.Cut,"CutHS.png"
    ApplicationCommands.Copy,"CopyHS.png"
    ApplicationCommands.Paste,"PasteHS.png"
    ApplicationCommands.Delete,"DeleteHS.png"
    |] |> Array.iter addCommands

    win.Show()
    Application().Run(win) |> ignore
with e -> MessageBox.Show(e.Message,"Exception Error") |> ignore
