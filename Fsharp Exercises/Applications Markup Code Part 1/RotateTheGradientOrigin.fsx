#if INTERACTIVE
#r "PresentationCore.dll"
#r "PresentationFramework.dll"
#r "System.Xaml.dll"
#r "WindowsBase.dll"
#r "UIAutomationTypes.dll"
#endif

open System
open System.Windows
open System.Windows.Input
open System.Windows.Controls
open System.Windows.Media
open System.Windows.Threading

type ClickTheRadientCenter () as t =
    inherit Window()

    let brush = 
        RadialGradientBrush()
        |> fun x -> x.GradientStops.Add(GradientStop(Colors.Red,0.0)); x
        |> fun x -> x.GradientStops.Add(GradientStop(Colors.Orange,0.17)); x
        |> fun x -> x.GradientStops.Add(GradientStop(Colors.Yellow,0.33)); x
        |> fun x -> x.GradientStops.Add(GradientStop(Colors.Green,0.5)); x
        |> fun x -> x.GradientStops.Add(GradientStop(Colors.Blue,0.67)); x
        |> fun x -> x.GradientStops.Add(GradientStop(Colors.Indigo,0.84)); x
        |> fun x -> x.GradientStops.Add(GradientStop(Colors.Violet,1.0)); x
        |> fun x -> x.RadiusX <- 0.1; x
        |> fun x -> x.RadiusY <- 0.1; x
        |> fun x -> x.SpreadMethod <- GradientSpreadMethod.Repeat; x

    do 
        t.Height <- 400.0
        t.Width <- 400.0
        t.WindowStartupLocation <- WindowStartupLocation.CenterScreen
        t.Background <- brush

        let tmr = DispatcherTimer()            
        tmr.Interval <- TimeSpan.FromMilliseconds(10.0)

        let mutable angle = 0.0
        tmr.Tick.Add(fun _ ->
            let pt = Point(0.5 + 0.05 * cos angle, 0.5 + 0.05 * sin angle)
            brush.GradientOrigin <- pt
            angle <- angle + Math.PI / 60.0
        )

        tmr.Start()
        
ClickTheRadientCenter().Show()



