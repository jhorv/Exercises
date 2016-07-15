// This is the previous rewritten to be more idiomatic F#. Those ugly floating mutable states
// are neatly enapsulated using discriminated union types. Pure beauty.

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

type State = Free | Drawing of Ellipse * center: Point | Dragging of Ellipse * center: Point * mouse: Point

[<STAThreadAttribute>]
try 
    let win = Window()
    win.Title <- "Draw Circles"

    let canv = Canvas()
    win.Content <- canv

    let mutable state = Free
    win.MouseLeftButtonDown.Add <| fun args ->
        if state = Free then
            // Create a new Ellipse object and add it to canvas.
            let ptCenter = args.GetPosition canv
            let elips = Ellipse()
            elips.Stroke <- SystemColors.WindowTextBrush
            elips.StrokeThickness <- 1.0
            elips.Width <- 0.0
            elips.Height <- 0.0
            canv.Children.Add elips |> ignore
            Canvas.SetLeft(elips,ptCenter.X)
            Canvas.SetTop(elips,ptCenter.Y)

            // Capture the mouse and prepare for future events.
            win.CaptureMouse() |> ignore
            state <- Drawing(elips,ptCenter)

    win.MouseRightButtonDown.Add <| fun args ->
        if state = Free then
            // Get the clicked element and prepare for future events.
            let ptMouseStart = args.GetPosition canv
            let elDragging = canv.InputHitTest ptMouseStart :?> Ellipse
            if elDragging <> null then
                state <- Dragging(elDragging, Point(Canvas.GetLeft(elDragging),Canvas.GetTop(elDragging)), ptMouseStart)
                
    win.MouseDown.Add <| fun args ->
        if state = Free then
            if args.ChangedButton = MouseButton.Middle then
                let shape = canv.InputHitTest(args.GetPosition canv) :?> Shape
                if shape <> null then
                    shape.Fill <- if shape.Fill = (Brushes.Red :> Brush) then Brushes.Transparent else Brushes.Red

    win.MouseMove.Add <| fun args ->
        let ptMouse = args.GetPosition canv
        match state with
        // Move and resize the Ellipse.
        | Drawing(elips,ptCenter) ->
            let dRadius = 
                let x = ptCenter.X - ptMouse.X
                let y = ptCenter.Y - ptMouse.Y
                sqrt(x*x + y*y)
            Canvas.SetLeft(elips,ptCenter.X-dRadius)
            Canvas.SetTop(elips,ptCenter.Y-dRadius)
            elips.Width <- 2.0 * dRadius
            elips.Height <- 2.0 * dRadius
        // Move the Ellipse.
        | Dragging(elDragging,ptElementStart,ptMouseStart) ->
            Canvas.SetLeft(elDragging,ptElementStart.X + ptMouse.X - ptMouseStart.X)
            Canvas.SetTop(elDragging,ptElementStart.Y + ptMouse.Y - ptMouseStart.Y)
        | _ -> ()

    win.MouseUp.Add <| fun args ->
        match state with
        | Drawing(elips,ptCenter) ->
            // End the drawing operation.
            if args.ChangedButton = MouseButton.Left then
                elips.Stroke <- Brushes.Blue
                elips.StrokeThickness <- min 24.0 (elips.Width / 2.0)
                elips.Fill <- Brushes.Red
                state <- Free
                win.ReleaseMouseCapture()
        // End the capture operation.
        | Dragging _ ->
            if args.ChangedButton = MouseButton.Right then
                state <- Free
        | _ -> ()

    win.TextInput.Add <| fun args ->
        // End drawing or dragging with press of Escape key.
        if args.Text.IndexOf '\x1B' <> -1 then
            match state with
            | Drawing _ -> win.ReleaseMouseCapture()
            | Dragging(elDragging,ptElementStart,_) ->
                Canvas.SetLeft(elDragging,ptElementStart.X)
                Canvas.SetTop(elDragging,ptElementStart.Y)
                state <- Free
            | _ -> ()

    win.LostMouseCapture.Add <| fun args ->
        // Abnormal end of drawing: Remove child Ellipse.
        match state with
        | Drawing(elips,_) ->
            canv.Children.Remove elips
            state <- Free
        | _ -> ()

    win.Show()
    //Application().Run(win) |> ignore
with e -> MessageBox.Show(e.Message,"Exception Error") |> ignore
        



