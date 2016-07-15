// This one needs to be compiled as it uses a resource.

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
open System.ComponentModel
open System.Windows
open System.Windows.Input
open System.Windows.Controls
open System.Windows.Controls.Primitives
open System.Windows.Media
open System.Windows.Media.Imaging
open System.Windows.Shapes
open System.Windows.Documents

type DesignAButton() as t =
    inherit Window()

    do 
        t.MinHeight <- 300.0
        t.MinWidth <- 300.0
        t.WindowStartupLocation <- WindowStartupLocation.CenterScreen
        t.SizeToContent <- SizeToContent.WidthAndHeight

        t.Title <- "Design A Button"

        let btn = Button()
        btn.Click.Add(fun _ -> MessageBox.Show("The button has been clicked.",t.Title) |> ignore)
        btn.HorizontalAlignment <- HorizontalAlignment.Center
        btn.VerticalAlignment <- VerticalAlignment.Center

        t.Content <- btn

        let stack = StackPanel()
        btn.Content <- stack

        let zigzag offset =
            let poly = Polyline()
            poly.Stroke <- SystemColors.ControlTextBrush
            poly.Points <- PointCollection()

            let mutable x = 0.0
            while x <= 100.0 do
                poly.Points.Add(Point(x,(x + offset) % 20.0))
                x <- x+10.0

            poly

        stack.Children.Add(zigzag 10.0) |> ignore

        let img = 
            Uri "pack://application:,,/Images/alien.png" 
            |> BitmapImage 
            |> fun x -> 
                let img = Image()
                img.Source <- x
                img.Margin <- Thickness(0.0,10.0,0.0,0.0)
                img.Stretch <- Stretch.None
                img

        stack.Children.Add img |> ignore

        let lbl = Label()
        lbl.Content <- "_Read Books!"
        lbl.HorizontalAlignment <- HorizontalAlignment.Center
        stack.Children.Add lbl |> ignore

        stack.Children.Add(zigzag 0.0) |> ignore



//try DesignAButton().Show()
//with e -> MessageBox.Show(e.Message,"Exception Error") |> ignore

[<STAThreadAttribute>]
do
    try Application().Run(DesignAButton()) |> ignore
    with e -> MessageBox.Show(e.Message,"Exception Error") |> ignore


