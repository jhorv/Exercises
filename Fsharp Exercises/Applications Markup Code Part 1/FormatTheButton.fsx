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

type FormatTheButton() as t =
    inherit Window()

    do 
        t.MinHeight <- 300.0
        t.MinWidth <- 300.0
        t.WindowStartupLocation <- WindowStartupLocation.CenterScreen
        t.SizeToContent <- SizeToContent.WidthAndHeight

        let runButton = Run("button")
        let btn = Button()
        btn.HorizontalContentAlignment <- HorizontalAlignment.Center
        btn.VerticalContentAlignment <- VerticalAlignment.Center
        btn.MouseEnter.Add(fun _ -> runButton.Foreground <- Brushes.Red)
        btn.MouseLeave.Add(fun _ -> runButton.Foreground <- SystemColors.ControlTextBrush)

        let txtblk = TextBlock()
        txtblk.FontSize <- 24.0
        txtblk.TextAlignment <- TextAlignment.Center
        btn.Content <- txtblk

        txtblk.Inlines.Add(Italic <| Run "Click")
        txtblk.Inlines.Add(" the ")
        txtblk.Inlines.Add(runButton)
        txtblk.Inlines.Add(LineBreak())
        txtblk.Inlines.Add "To launch the "
        txtblk.Inlines.Add(Bold <| Run "rocket")

        t.Content <- btn
        

FormatTheButton().Show()
//[<STAThreadAttribute>]
//do
//    Application().Run(ClickTheButton()) |> ignore





