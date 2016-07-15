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

try 
    let win = Window()
//    win.MinHeight <- 300.0
//    win.MinWidth <- 300.0
//    win.MaxHeight <- 600.0
//    win.MaxWidth <- 600.0
//    win.WindowStartupLocation <- WindowStartupLocation.CenterScreen
//    win.SizeToContent <- SizeToContent.WidthAndHeight

    win.Title <- "Dock Around The Block"
    let dock = DockPanel()
    win.Content <- dock
    
    //dock.LastChildFill <- false
    for i=0 to 16 do
        let btn = Button()
        btn.HorizontalAlignment <- HorizontalAlignment.Center
        btn.Content <- sprintf "Button No.%i" i
        dock.Children.Add btn |> ignore
        btn.SetValue(DockPanel.DockProperty,enum<Dock>(i % 4))
    win.Show()
with e -> MessageBox.Show(e.Message,"Exception Error") |> ignore

//[<STAThreadAttribute>]
//do
//    try Application().Run(TuneTheRadio()) |> ignore
//    with e -> MessageBox.Show(e.Message,"Exception Error") |> ignore

