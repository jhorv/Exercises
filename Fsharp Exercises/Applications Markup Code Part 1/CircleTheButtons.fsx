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

type RadialPanelOrientation = ByWidth | ByHeight

type RadialPanel() =
    inherit Panel()

    static let orientationProperty = DependencyProperty.Register("Orientation",typeof<RadialPanelOrientation>,typeof<RadialPanel>,FrameworkPropertyMetadata(ByWidth,FrameworkPropertyMetadataOptions.AffectsMeasure))

    let mutable showPieLines = false
    let mutable angleEach,sizeLargest = 360.0, Size(0.0,0.0)
    let mutable radius,outerEdgeFromCenter,innerEdgeFromCenter = 0.0,0.0,0.0
    member t.OrientationProperty = orientationProperty

    member t.Orientation
        with get() = t.GetValue(orientationProperty) :?> RadialPanelOrientation
        and set(v: RadialPanelOrientation) = t.SetValue(orientationProperty,v)

    member t.ShowPieLines
        with get() = showPieLines
        and set(v: bool) =
            if v <> showPieLines then
                t.InvalidateVisual()
            showPieLines <- v

    override t.MeasureOverride sizeAvailable =
        if t.InternalChildren.Count = 0 then Size(0.0,0.0)
        else
            angleEach <- 360.0 / float t.InternalChildren.Count
            sizeLargest <- Size(0.0,0.0)
            let p = Size(Double.PositiveInfinity,Double.PositiveInfinity)

            for child in t.Children do
                // Call Measure for each child ...
                child.Measure p
                // ... and then examine DesiredSize property of child.
                sizeLargest <- 
                    Size(max sizeLargest.Width child.DesiredSize.Width,
                         max sizeLargest.Height child.DesiredSize.Height)

            match t.Orientation with
            | ByWidth ->
                // Calculate the distance from the center to element edges.
                innerEdgeFromCenter <- sizeLargest.Width / 2.0 / tan (Math.PI * angleEach / 360.0)
                outerEdgeFromCenter <- innerEdgeFromCenter + sizeLargest.Height
                // Calculate the radius of the circle based on the largest child.
                radius <- 
                    let x = sizeLargest.Width / 2.0
                    sqrt (x*x + outerEdgeFromCenter*outerEdgeFromCenter)
            | ByHeight ->
                // Calculate the distance from the center to element edges.
                innerEdgeFromCenter <- sizeLargest.Height / 2.0 / tan (Math.PI * angleEach / 360.0)
                outerEdgeFromCenter <- innerEdgeFromCenter + sizeLargest.Width
                // Calculate the radius of the circle based on the largest child.
                radius <- 
                    let x = sizeLargest.Height / 2.0
                    sqrt (x*x + outerEdgeFromCenter*outerEdgeFromCenter)
            Size(2.0*radius,2.0*radius)

    override t.ArrangeOverride sizeFinal =
        let mutable angleChild = 0.0
        let ptCenter = Point(sizeFinal.Width/2.0,sizeFinal.Height/2.0)
        let multiplier = min (sizeFinal.Width/(2.0*radius)) (sizeFinal.Height/(2.0*radius))

        for child in t.Children do
            // Reset RenderTransform.
            child.RenderTransform <- Transform.Identity
            match t.Orientation with
            | ByWidth ->
                // Position the child at the top.
                Rect(ptCenter.X - multiplier * sizeLargest.Width / 2.0,
                        ptCenter.Y - multiplier * outerEdgeFromCenter,
                        multiplier * sizeLargest.Width,
                        multiplier * sizeLargest.Height)
                |> child.Arrange
            | ByHeight ->
                // Position the child at the right.
                Rect(ptCenter.X + multiplier * innerEdgeFromCenter,
                     ptCenter.Y + multiplier * sizeLargest.Height / 2.0,
                     multiplier * sizeLargest.Width,
                     multiplier * sizeLargest.Height)
                |> child.Arrange
            let pt = t.TranslatePoint(ptCenter,child)
            child.RenderTransform <- RotateTransform(angleChild,pt.X,pt.Y)
            angleChild <- angleChild + angleEach
        sizeFinal

    override t.OnRender dc =
        base.OnRender dc

        if showPieLines then
            let ptCenter = Point(t.RenderSize.Width / 2.0, t.RenderSize.Height / 2.0)
            let multiplier = min (t.RenderSize.Width / (2.0 * radius)) (t.RenderSize.Height / (2.0 * radius))
            let pen = Pen(SystemColors.WindowTextBrush, 1.0)
            pen.DashStyle <- DashStyles.Dash

            dc.DrawEllipse(null, pen, ptCenter, multiplier*radius, multiplier*radius)
            let mutable angleChild = -angleEach / 2.0 + (if t.Orientation = ByWidth then 90.0 else 0.0)

            // Loop through each child to draw radial lines from center.
            for child in t.Children do
                let angle = Math.PI * angleChild / 180.0
                let x = ptCenter.X + multiplier * radius * cos angle
                let y = ptCenter.Y + multiplier * radius * sin angle
                dc.DrawLine(pen,ptCenter,Point(x,y))
                angleChild <- angleChild + angleEach

                
[<STAThreadAttribute>]
try 
    let win = Window()
    win.Title <- "Circle The Buttons"
    win.SizeToContent <- SizeToContent.WidthAndHeight

    let pnl = RadialPanel()
    pnl.Orientation <- RadialPanelOrientation.ByHeight
    pnl.ShowPieLines <- true
    win.Content <- pnl

    let rand = Random()

    for i=1 to 10 do
        let btn = Button()
        btn.Content <- sprintf "Button Number %i" i
        btn.FontSize <- btn.FontSize + float (rand.Next(10))
        pnl.Children.Add btn |> ignore

    win.Show()
    //Application().Run(win) |> ignore
with e -> MessageBox.Show(e.Message,"Exception Error") |> ignore
