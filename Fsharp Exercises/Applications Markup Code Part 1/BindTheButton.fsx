#if INTERACTIVE
#r "PresentationCore.dll"
#r "PresentationFramework.dll"
#r "System.Xaml.dll"
#r "WindowsBase.dll"
#r "UIAutomationTypes.dll"
#endif

open System
open System.IO
open System.Windows
open System.Windows.Input
open System.Windows.Controls
open System.Windows.Controls.Primitives
open System.Windows.Media
open System.Windows.Media.Imaging
open System.Windows.Shapes
open System.Windows.Documents

type BindTheButton() as t =
    inherit Window()

    do 
        t.MinHeight <- 300.0
        t.MinWidth <- 300.0
        t.WindowStartupLocation <- WindowStartupLocation.CenterScreen
        t.SizeToContent <- SizeToContent.WidthAndHeight

        let btn = ToggleButton() // Can be replaced with CheckBox without changing anything else.
        
        btn.HorizontalContentAlignment <- HorizontalAlignment.Center
        btn.VerticalContentAlignment <- VerticalAlignment.Center
        btn.Margin <- Thickness(20.0)
        btn.Content <- "Make _Topmost"
        btn.SetBinding(ToggleButton.IsCheckedProperty,"Topmost") |> ignore
        btn.DataContext <- t

        let tip = ToolTip()
        tip.Content <- "Toggle the button on to make the window topmost on the desktop"
        btn.ToolTip <- tip

        t.Content <- btn


BindTheButton().Show()
//[<STAThreadAttribute>]
//do
//    Application().Run(CommandTheButton()) |> ignore

