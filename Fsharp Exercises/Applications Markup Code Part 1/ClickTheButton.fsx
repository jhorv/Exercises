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
open System.Windows.Media
open System.Windows.Media.Imaging
open System.Windows.Shapes
open System.Windows.Documents

type ClickTheButton() as t =
    inherit Window()

    do 
        t.MinHeight <- 300.0
        t.MinWidth <- 300.0
        t.WindowStartupLocation <- WindowStartupLocation.CenterScreen
        t.SizeToContent <- SizeToContent.WidthAndHeight

        let btn = Button()
        btn.Content <- "_Click Me."
        btn.Padding <- Thickness(25.0)
        btn.Margin <- Thickness(10.0)
        btn.HorizontalContentAlignment <- HorizontalAlignment.Left
        btn.VerticalContentAlignment <- VerticalAlignment.Top
        btn.Click.Add(fun x -> MessageBox.Show("Button has been clicked.",t.Title) |> ignore)
        t.Content <- btn
        btn.Focus() |> ignore
        

ClickTheButton().Show()
//[<STAThreadAttribute>]
//do
//    Application().Run(ClickTheButton()) |> ignore




