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

type BetterEllipse() =
    inherit FrameworkElement()

    static let fillProperty = DependencyProperty.Register("Fill",typeof<Brush>,typeof<BetterEllipse>,FrameworkPropertyMetadata(null,FrameworkPropertyMetadataOptions.AffectsRender))
    static let strokeProperty = DependencyProperty.Register("Stroke",typeof<Pen>,typeof<BetterEllipse>,FrameworkPropertyMetadata(null,FrameworkPropertyMetadataOptions.AffectsMeasure))

    static member FillProperty = fillProperty
    static member StrokeProperty = strokeProperty

    member t.Fill
        with get() = t.GetValue(fillProperty) :?> Brush
        and set (v: Brush) = t.SetValue(fillProperty,v)
        
    member t.Stroke
        with get() = t.GetValue(strokeProperty) :?> Pen
        and set (v: Pen) = t.SetValue(strokeProperty,v)

    override t.MeasureOverride(sizeAvailable)=
        let sizeDesired = base.MeasureOverride sizeAvailable
        let s = t.Stroke
        if s <> null then s.Thickness |> fun x -> Size(x,x) else sizeDesired

    override t.OnRender(dc) =
        base.OnRender dc
        
        let stroke = t.Stroke
        let size = 
            if stroke <> null then
                let size = t.RenderSize
                let x = stroke.Thickness
                Size(max 0.0 (size.Width - x), max 0.0 (size.Height - x))
            else t.RenderSize
        
        dc.DrawEllipse(t.Fill,stroke,Point(t.RenderSize.Width / 2.0, t.RenderSize.Height / 2.0),size.Width / 2.0,size.Height / 2.0)

[<STAThreadAttribute>]
try 
    let win = Window()
    win.Title <- "Render The Better Ellipse"

    let elips = BetterEllipse()
    elips.Fill <- Brushes.AliceBlue
    elips.Stroke <- Pen(LinearGradientBrush(Colors.CadetBlue,Colors.Chocolate,Point(1.0,0.0),Point(0.0,1.0)),24.0)
    win.Content <- elips

    win.Show()
    //Application().Run(win) |> ignore
with e -> MessageBox.Show(e.Message,"Exception Error") |> ignore


