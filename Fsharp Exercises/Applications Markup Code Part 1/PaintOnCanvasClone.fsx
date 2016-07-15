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

type CanvasClone() =
    inherit Panel()

    static let leftProperty = DependencyProperty.RegisterAttached("Left",typeof<float>,typeof<CanvasClone>,FrameworkPropertyMetadata(0.0,FrameworkPropertyMetadataOptions.AffectsParentArrange))
    static let topProperty = DependencyProperty.RegisterAttached("Top",typeof<float>,typeof<CanvasClone>,FrameworkPropertyMetadata(0.0,FrameworkPropertyMetadataOptions.AffectsParentArrange))

    static member SetLeft(o: DependencyObject, v: float) = o.SetValue(leftProperty,v)
    static member SetTop(o: DependencyObject, v: float) = o.SetValue(topProperty,v)

    static member GetLeft(o: DependencyObject) = o.GetValue(leftProperty) :?> float
    static member GetTop(o: DependencyObject) = o.GetValue(topProperty) :?> float

    member t.LeftProperty = leftProperty
    member t.TopProperty = topProperty

    override t.MeasureOverride sizeAvailable =
        let p = Size(Double.PositiveInfinity,Double.PositiveInfinity)
        for child in t.Children do
            child.Measure(p)
        base.MeasureOverride sizeAvailable

    override t.ArrangeOverride sizeFinal =
        for child in t.Children do
            child.Arrange(Rect(Point(CanvasClone.GetLeft child, CanvasClone.GetTop child),child.DesiredSize))
        sizeFinal

[<STAThreadAttribute>]
try 
    let win = Window()
    win.Title <- "Paint On Canvas Clone"

    let canv = CanvasClone()
    win.Content <- canv

    [|Brushes.Red;Brushes.Green;Brushes.Blue|]
    |> Array.iteri (fun i br ->
        let rect = Rectangle()
        rect.Fill <- br
        rect.Width <- 200.0
        rect.Height <- 200.0
        canv.Children.Add rect |> ignore
        CanvasClone.SetLeft(rect, 100 * (i+1) |> float)
        CanvasClone.SetTop(rect, 100 * (i+1) |> float)
        )

    win.Show()
    //Application().Run(win) |> ignore
with e -> MessageBox.Show(e.Message,"Exception Error") |> ignore




