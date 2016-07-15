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

type DiagonalPanel() =
    inherit Panel()

    static let backgroundProperty = DependencyProperty.Register("Background",typeof<Brush>,typeof<DiagonalPanel>,FrameworkPropertyMetadata(null,FrameworkPropertyMetadataOptions.AffectsRender))

    let children = ResizeArray<UIElement>()
    let mutable sizeChildrenTotal = Size(0.0,0.0)

    member t.BackgroundProperty = backgroundProperty
    member t.Background
        with get() = t.GetValue(backgroundProperty) :?> Brush
        and set(v: Brush) = t.SetValue(backgroundProperty,v)

    member t.Add el =
        children.Add el
        t.AddVisualChild el
        t.AddLogicalChild el
        t.InvalidateMeasure()

    member t.Remove el =
        children.Remove el |> ignore
        t.RemoveVisualChild el
        t.RemoveLogicalChild el
        t.InvalidateMeasure()

    member t.IndexOf el = children.IndexOf el

    override t.VisualChildrenCount = children.Count
    override t.GetVisualChild i = children.[i] :> Visual // The ResizeArray already has bounds checking.

    override t.MeasureOverride sizeAvailable =
        let p = Size(Double.PositiveInfinity,Double.PositiveInfinity)
        sizeChildrenTotal <- Size(0.0,0.0)
        for child in children do
            child.Measure p
            sizeChildrenTotal <- 
                Size(sizeChildrenTotal.Width+child.DesiredSize.Width,
                     sizeChildrenTotal.Height+child.DesiredSize.Height)
        sizeChildrenTotal

    override t.ArrangeOverride sizeFinal =
        let mutable x,y = 0.0,0.0
        for child in children do
            let w = child.DesiredSize.Width * (sizeFinal.Width / sizeChildrenTotal.Width)
            let h = child.DesiredSize.Height * (sizeFinal.Height / sizeChildrenTotal.Height)
            child.Arrange(Rect(x,y,w,h))
            x <- x + w; y <- y + h
        sizeFinal

    override t.OnRender dc =
        dc.DrawRectangle(t.Background,null,Rect(Point(0.0,0.0),t.RenderSize))

[<STAThreadAttribute>]
try 
    let win = Window()
    win.Title <- "Diagonalize The Buttons"

    let pnl = DiagonalPanel()
    win.Content <- pnl

    let rand = Random()

    for i=1 to 5 do
        let btn = Button()
        btn.Content <- sprintf "Button Number %i" i
        btn.FontSize <- btn.FontSize + float(rand.Next(20))
        pnl.Add btn

    win.Show()
    //Application().Run(win) |> ignore
with e -> MessageBox.Show(e.Message,"Exception Error") |> ignore
