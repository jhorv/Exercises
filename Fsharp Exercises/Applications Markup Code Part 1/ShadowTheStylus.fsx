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

[<STAThreadAttribute>]
try 
    let win = Window()
    win.Title <- "Shadow The Stylus"

    // Define some constants for the stylus polylines.
    let brushStylus = Brushes.Blue
    let brushShadow = Brushes.LightBlue
    let widthStroke = 96.0 / 2.54 // 1cm
    let vectShadow = Vector(widthStroke / 4.0, widthStroke / 4.0)

    let canv = Canvas()
    win.Content <- canv

    // More fields for stylus-move operations.
    let mutable polyStylus, polyShadow = Polyline(), Polyline()
    let mutable isDrawing = false

    win.StylusDown.Add <| fun args ->
        let ptStylus = args.GetPosition canv

        // Create a Polyline with rounded ends and joins for the foreground.
        polyStylus <- Polyline()
        polyStylus.Stroke <- brushStylus
        polyStylus.StrokeThickness <- widthStroke
        polyStylus.StrokeStartLineCap <- PenLineCap.Round
        polyStylus.StrokeEndLineCap <- PenLineCap.Round
        polyStylus.StrokeLineJoin <- PenLineJoin.Round
        polyStylus.Points <- PointCollection()
        polyStylus.Points.Add(ptStylus + vectShadow)
        
        // Another Polyline for the shadow.
        polyShadow <- Polyline()
        polyShadow.Stroke <- brushShadow
        polyShadow.StrokeThickness <- widthStroke
        polyShadow.StrokeStartLineCap <- PenLineCap.Round
        polyShadow.StrokeEndLineCap <- PenLineCap.Round
        polyShadow.StrokeLineJoin <- PenLineJoin.Round
        polyShadow.Points <- PointCollection()
        polyShadow.Points.Add(ptStylus + vectShadow)

        // Insert shadow before all foreground polylines.
        canv.Children.Insert(canv.Children.Count / 2, polyShadow)

        // Foreground can go at end.
        canv.Children.Add polyStylus |> ignore
        win.CaptureStylus() |> ignore
        isDrawing <- true
        args.Handled <- true

    win.StylusMove.Add <| fun args ->
        if isDrawing then
            let ptStylus = args.GetPosition canv
            polyStylus.Points.Add ptStylus
            polyShadow.Points.Add (ptStylus + vectShadow)
            args.Handled <- true

    win.StylusUp.Add <| fun args ->
        if isDrawing then
            isDrawing <- false
            win.ReleaseStylusCapture()
            args.Handled <- true

    win.TextInput.Add <| fun args ->
        if isDrawing && args.Text.IndexOf '\x1B' <> -1 then
            win.ReleaseStylusCapture()
            args.Handled <- true

    win.LostStylusCapture.Add <| fun args ->
        if isDrawing then
            canv.Children.Remove polyStylus
            canv.Children.Remove polyShadow
            isDrawing <- false

    win.Show()
    //Application().Run(win) |> ignore
with e -> MessageBox.Show(e.Message,"Exception Error") |> ignore