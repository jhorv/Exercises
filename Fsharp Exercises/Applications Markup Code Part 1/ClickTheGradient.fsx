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

    do 
        t.Height <- 400.0
        t.Width <- 400.0
        t.WindowStartupLocation <- WindowStartupLocation.CenterScreen
        t.Background <- brush

    override t.OnMouseDown args =
        base.OnMouseDown args

        let width = t.ActualWidth - 2.0 * SystemParameters.ResizeFrameVerticalBorderWidth
        let height = t.ActualHeight - 2.0 * SystemParameters.ResizeFrameHorizontalBorderHeight - SystemParameters.CaptionHeight

        let ptMouse = 
            let p = args.GetPosition(t)
            Point(p.X / width, p.Y / height)

        match args.ChangedButton with
        | MouseButton.Left ->
            brush.Center <- ptMouse
            brush.GradientOrigin <- ptMouse
        | MouseButton.Right ->
            brush.GradientOrigin <- ptMouse
        | _ -> ()
            
        
ClickTheRadientCenter().Show()


