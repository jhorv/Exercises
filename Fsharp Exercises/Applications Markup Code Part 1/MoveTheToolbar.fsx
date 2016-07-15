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
    let dock = DockPanel()
    let win = Window(Title = "Move The Toolbar", Content = dock)
    let trayTop = ToolBarTray() |> addChildren dock |> DockPanel.DockTop
    let trayLeft = ToolBarTray(Orientation=Orientation.Vertical) |> addChildren dock |> DockPanel.DockLeft
    let txtbox = TextBox() |> addChildren dock

    let createToolBar (tray: ToolBarTray) i =
        let toolbar = ToolBar(Header = sprintf "ToolBar %i" i)
        tray.ToolBars.Add toolbar

        for i=0 to 5 do
            Button(FontSize=16.0,Content=(byte 'A'+ byte i |> char)) 
            |> addItems toolbar |> ignore

    for i=1 to 3 do createToolBar trayTop i
    for i=4 to 6 do createToolBar trayLeft i

    win.Show()
//    Application().Run(win) |> ignore
with e -> MessageBox.Show(e.Message,"Exception Error") |> ignore
