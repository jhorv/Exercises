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

type ToggleTheButton() as t =
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
        btn.Content <- "Can _Resize"
        btn.IsChecked <- t.ResizeMode = ResizeMode.CanResize |> Nullable

        let checked_func _ = t.ResizeMode <- if btn.IsChecked.GetValueOrDefault true then ResizeMode.CanResize else ResizeMode.NoResize
        btn.Checked.Add checked_func
        btn.Unchecked.Add checked_func

        t.Content <- btn


ToggleTheButton().Show()
//[<STAThreadAttribute>]
//do
//    Application().Run(CommandTheButton()) |> ignore



