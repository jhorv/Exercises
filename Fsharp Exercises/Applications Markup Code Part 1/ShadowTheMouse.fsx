// The example does not exist in the book, so I've converted the ShadowTheStylus to use the mouse.
// I prettied it up a bit as well.

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

type State = Free | Drawing of mouse: Polyline * shadow: Polyline

[<STAThreadAttribute>]
try 
    let win = Window()
    win.Title <- "Shadow The Mouse"

    // Define some constants for the Mouse polylines.
    let brushMouse = Brushes.Blue
    let brushShadow = Brushes.LightBlue
    let widthStroke = 96.0 / 2.54 // 1cm
    let vectShadow = Vector(widthStroke / 4.0, widthStroke / 4.0)

    let canv = Canvas()
    win.Content <- canv

    // More fields for mouse-move operations.
    let mutable state = Free

    win.MouseDown.Add <| fun args ->
        let ptMouse = args.GetPosition canv

        let inline f (poly: Polyline, brush, shad) =
            poly.Stroke <- brush
            poly.StrokeThickness <- widthStroke
            poly.StrokeStartLineCap <- PenLineCap.Round
            poly.StrokeEndLineCap <- PenLineCap.Round
            poly.StrokeLineJoin <- PenLineJoin.Round
            poly.Points <- PointCollection()
            poly.Points.Add(ptMouse + shad)
            poly

        // Create a Polyline with rounded ends and joins for the foreground.
        // And another Polyline for the shadow.
        let polyMouse = f (Polyline(),brushMouse,Vector(0.0,0.0))
        let polyShadow = f (Polyline(),brushShadow,vectShadow)
        
        // Insert shadow before all foreground polylines.
        canv.Children.Insert(canv.Children.Count / 2, polyShadow)

        // Foreground can go at end.
        canv.Children.Add polyMouse |> ignore
        win.CaptureMouse() |> ignore
        state <- Drawing(polyMouse,polyShadow)
        args.Handled <- true

    win.MouseMove.Add <| fun args ->
        match state with
        | Drawing(polyMouse,polyShadow) ->
            let ptMouse = args.GetPosition canv
            polyMouse.Points.Add ptMouse
            polyShadow.Points.Add (ptMouse + vectShadow)
            args.Handled <- true
        | _ -> ()

    win.MouseUp.Add <| fun args ->
        match state with
        | Drawing(polyMouse,polyShadow) ->
            state <- Free
            win.ReleaseMouseCapture()
            args.Handled <- true
        | _ -> ()

    win.TextInput.Add <| fun args ->
        match state with
        | Drawing(polyMouse,polyShadow) ->
            if args.Text.IndexOf '\x1B' <> -1 then
                win.ReleaseMouseCapture()
                args.Handled <- true
        | _ -> ()

    win.LostMouseCapture.Add <| fun args ->
        match state with
        | Drawing(polyMouse,polyShadow) ->
            canv.Children.Remove polyMouse
            canv.Children.Remove polyShadow
            state <- Free
        | _ -> ()

    win.Show()
    //Application().Run(win) |> ignore
with e -> MessageBox.Show(e.Message,"Exception Error") |> ignore
