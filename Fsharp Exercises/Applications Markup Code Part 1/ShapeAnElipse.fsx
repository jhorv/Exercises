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

type ShowMyFace() as t =
    inherit Window()

    do 
        t.Height <- 200.0
        t.Width <- 200.0
        t.WindowStartupLocation <- WindowStartupLocation.CenterScreen
        t.SizeToContent <- SizeToContent.WidthAndHeight

        let el = Ellipse()
        el.Fill <- Brushes.AliceBlue
        el.StrokeThickness <- 24.0
        el.Stroke <- LinearGradientBrush(Colors.CadetBlue,Colors.Chocolate,Point(1.0,0.0),Point(0.0,1.0))
        t.Content <- el

ShowMyFace().Show()
//[<STAThreadAttribute>]
//do
//    Application().Run(ShowMyFace()) |> ignore






