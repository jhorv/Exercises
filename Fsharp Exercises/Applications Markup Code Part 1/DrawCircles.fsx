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
    win.Title <- "Draw Circles"

    let canv = Canvas()
    win.Content <- canv

    let mutable isDrawing = false
    let mutable isDragging = false
    let mutable ptCenter, ptElementStart, ptMouseStart  = Point(), Point(), Point()
    let mutable elips = Ellipse()
    let mutable elDragging = FrameworkElement()
    win.MouseLeftButtonDown.Add <| fun args ->
        if not isDragging then
            // Create a new Ellipse object and add it to canvas.
            ptCenter <- args.GetPosition canv
            elips <- Ellipse()
            elips.Stroke <- SystemColors.WindowTextBrush
            elips.StrokeThickness <- 1.0
            elips.Width <- 0.0
            elips.Height <- 0.0
            canv.Children.Add elips |> ignore
            Canvas.SetLeft(elips,ptCenter.X)
            Canvas.SetTop(elips,ptCenter.Y)
            
            // Capture the mouse and prepare for future events.
            win.CaptureMouse() |> ignore
            isDrawing <- true

    win.MouseRightButtonDown.Add <| fun args ->
        if not isDrawing then
            // Get the clicked element and prepare for future events.
            ptMouseStart <- args.GetPosition canv
            elDragging <- canv.InputHitTest ptMouseStart :?> FrameworkElement
            if elDragging <> null then
                ptElementStart <- Point(Canvas.GetLeft(elDragging),Canvas.GetTop(elDragging))
                isDragging <- true

    win.MouseDown.Add <| fun args ->
        if args.ChangedButton = MouseButton.Middle then
            let shape = canv.InputHitTest(args.GetPosition canv) :?> Shape
            if shape <> null then
                shape.Fill <- if shape.Fill = (Brushes.Red :> Brush) then Brushes.Transparent else Brushes.Red

    win.MouseMove.Add <| fun args ->
        let ptMouse = args.GetPosition canv
        // Move and resize the Ellipse.
        if isDrawing then
            let dRadius = 
                let x = ptCenter.X - ptMouse.X
                let y = ptCenter.Y - ptMouse.Y
                sqrt(x*x + y*y)
            Canvas.SetLeft(elips,ptCenter.X-dRadius)
            Canvas.SetTop(elips,ptCenter.Y-dRadius)
            elips.Width <- 2.0 * dRadius
            elips.Height <- 2.0 * dRadius
        // Move the Ellipse.
        elif isDragging then
            Canvas.SetLeft(elDragging,ptElementStart.X + ptMouse.X - ptMouseStart.X)
            Canvas.SetTop(elDragging,ptElementStart.Y + ptMouse.Y - ptMouseStart.Y)

    win.MouseUp.Add <| fun args ->
        // End the drawing operation.
        if isDrawing && args.ChangedButton = MouseButton.Left then
            elips.Stroke <- Brushes.Blue
            elips.StrokeThickness <- min 24.0 (elips.Width / 2.0)
            elips.Fill <- Brushes.Red
            isDrawing <- false
            win.ReleaseMouseCapture()
        // End the capture operation.
        elif isDragging && args.ChangedButton = MouseButton.Right then
            isDragging <- false

    win.TextInput.Add <| fun args ->
        // End drawing or dragging with press of Escape key.
        if args.Text.IndexOf '\x1B' <> -1 then
            if isDrawing then win.ReleaseMouseCapture()
            elif isDragging then
                Canvas.SetLeft(elDragging,ptElementStart.X)
                Canvas.SetTop(elDragging,ptElementStart.Y)
                isDragging <- false

    win.LostMouseCapture.Add <| fun args ->
        // Abnormal end of drawing: Remove child Ellipse.
        if isDrawing then
            canv.Children.Remove elips
            isDrawing <- false

    win.Show()
    //Application().Run(win) |> ignore
with e -> MessageBox.Show(e.Message,"Exception Error") |> ignore
        


