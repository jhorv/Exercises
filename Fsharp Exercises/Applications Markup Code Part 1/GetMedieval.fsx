// In the book OnKnock and OnPreviewKnock methods raise the wrong events.
// This has been fixed here.

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
open System.Globalization
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

type MedievalButton() as t =
    inherit Control()

    static let textProperty = DependencyProperty.Register("Text",typeof<string>,typeof<MedievalButton>,FrameworkPropertyMetadata(" ", FrameworkPropertyMetadataOptions.AffectsMeasure))
    static let knockEvent = EventManager.RegisterRoutedEvent("Knock",RoutingStrategy.Bubble,typeof<RoutedEventHandler>,typeof<MedievalButton>)
    static let previewKnockEvent = EventManager.RegisterRoutedEvent("PreviewKnock",RoutingStrategy.Tunnel,typeof<RoutedEventHandler>,typeof<MedievalButton>)
    static let customEvent add remove =
      { new IDelegateEvent<RoutedEventHandler> with
            member this.AddHandler del = add del
            member this.RemoveHandler del = remove del }

    let evKnock = customEvent (fun v -> t.AddHandler(knockEvent,v)) (fun v -> t.RemoveHandler(knockEvent,v))
    let evPreviewKnock = customEvent (fun v -> t.AddHandler(previewKnockEvent,v)) (fun v -> t.RemoveHandler(previewKnockEvent,v))
    let mutable formtxt: FormattedText = null
    let mutable isMouseReallyOver = false

    static member inline TextProperty = textProperty
    static member inline KnockEvent = knockEvent
    static member inline PreviewKnockEvenet = previewKnockEvent

    member t.Text
        with get() = t.GetValue(textProperty) :?> string
        and set v = t.SetValue(textProperty, if v = null then " " else v)

    [<CLIEvent>] member t.Knock = evKnock
    [<CLIEvent>] member t.PreviewKnock = evPreviewKnock

    // MeasureOverride called whenever the size of the button might change.
    override t.MeasureOverride(sizeAvailable) =
        formtxt <- 
            let tf = Typeface(t.FontFamily,t.FontStyle,t.FontWeight,t.FontStretch)
            FormattedText(t.Text,CultureInfo.CurrentCulture,t.FlowDirection,tf,t.FontSize,t.Foreground)

        // Take account of Padding when calculating the size.
        let paddingx = t.Padding.Left + t.Padding.Right
        let paddingy = t.Padding.Top + t.Padding.Bottom
        Size((max 48.0 formtxt.Width) + 4.0 + paddingx,formtxt.Height + 4.0 + paddingy)

    // OnRender called to redraw the button.
    override t.OnRender(dc) =
        let brushBackground = SystemColors.ControlBrush
        
        // Determine pen width.
        let pen = Pen(t.Foreground, if t.IsMouseOver then 2.0 else 1.0) 

        // Draw filled rounded rectangle.
        dc.DrawRoundedRectangle(brushBackground,pen,Rect(Point(0.0,0.0),t.RenderSize), 4.0, 4.0)

        // Determine foreground color.
        formtxt.SetForegroundBrush(if t.IsEnabled then t.Foreground else SystemColors.ControlDarkBrush :> Brush)

        // Determine start point of text.
        let ptText = 
            let ptTextx =
                match t.HorizontalContentAlignment with
                | HorizontalAlignment.Left -> t.Padding.Left
                | HorizontalAlignment.Right -> t.RenderSize.Width - formtxt.Width - t.Padding.Right
                | HorizontalAlignment.Center | HorizontalAlignment.Stretch -> 
                    (t.RenderSize.Width - formtxt.Width - t.Padding.Left - t.Padding.Right) / 2.0
                | _ -> failwith "Invalid enum in ptText calculation"
            let ptTexty =
                match t.VerticalContentAlignment with
                | VerticalAlignment.Top -> t.Padding.Top
                | VerticalAlignment.Bottom -> t.RenderSize.Height - formtxt.Height - t.Padding.Bottom
                | VerticalAlignment.Center | VerticalAlignment.Stretch ->
                    (t.RenderSize.Height - formtxt.Height - t.Padding.Top - t.Padding.Bottom) / 2.0
                | _ -> failwith "Invalid enum in ptText calculation"
            Point(ptTextx+2.0,ptTexty+2.0)

        // Draw the text.
        dc.DrawText(formtxt,ptText)
            
    override t.OnMouseEnter args  =
        base.OnMouseEnter args
        t.InvalidateVisual()

    override t.OnMouseLeave args =
        base.OnMouseLeave args
        t.InvalidateVisual()

    override t.OnMouseMove args =
        base.OnMouseMove args
        
        // Determine if mouse has really moved inside or out.
        let pt = args.GetPosition t
        let isReallyOverNow =
            pt.X >= 0.0 && pt.X < t.ActualWidth &&
            pt.Y >= 0.0 && pt.Y < t.ActualHeight

        if isReallyOverNow <> isMouseReallyOver then
            isMouseReallyOver <- isReallyOverNow
            t.InvalidateVisual()

    // OnPreviewKnock method raises the 'PreviewKnock' event.
    abstract member OnPreviewKnock: unit -> unit
    default t.OnPreviewKnock()=
        let argsEvent = RoutedEventArgs()
        argsEvent.RoutedEvent <- previewKnockEvent
        argsEvent.Source <- t
        t.RaiseEvent(argsEvent)

    // OnKnock method raises the 'Knock' event.
    abstract member OnKnock: unit -> unit
    default t.OnKnock()=
        let argsEvent = RoutedEventArgs()
        argsEvent.RoutedEvent <- knockEvent
        argsEvent.Source <- t
        t.RaiseEvent(argsEvent)


    override t.OnMouseLeftButtonDown args =
        base.OnMouseLeftButtonDown args
        t.CaptureMouse() |> ignore
        t.InvalidateVisual()
        args.Handled <- true

    // This event actually triggers the 'Knock' event.
    override t.OnMouseLeftButtonUp args =
        base.OnMouseLeftButtonUp args
        if t.IsMouseCaptured then
            if isMouseReallyOver then
                t.OnPreviewKnock()
                t.OnKnock()
            args.Handled <- true
            Mouse.Capture(null) |> ignore
                    
    // If lose mouse capture (either internally or externally), redraw.
    override t.OnLostMouseCapture args =
        base.OnLostMouseCapture args
        t.InvalidateVisual()

    // The keyboard Space key or Enter also triggers the button.
    override t.OnKeyDown args =
        base.OnKeyDown args
        match args.Key with
        | Key.Space | Key.Enter -> args.Handled <- true
        | _ -> ()

    override t.OnKeyUp args =
        base.OnKeyUp args
        match args.Key with
        | Key.Space | Key.Enter -> 
            t.OnPreviewKnock()
            t.OnKnock()
            args.Handled <- true
        | _ -> ()

[<STAThreadAttribute>]
try 
    let win = Window()
    win.Title <- "Get Medieval"

    let btn = MedievalButton()
    btn.Text <- "Click This Button"
    //btn.Width <- 50.0
    btn.FontSize <- 24.0
    btn.HorizontalAlignment <- HorizontalAlignment.Center
    btn.VerticalAlignment <- VerticalAlignment.Center
    btn.Padding <- Thickness(5.0,20.0,5.0,20.0)
    win.Content <- btn

    btn.Knock.AddHandler(fun sender args ->
        MessageBox.Show(sprintf "The button labeled \"%s\" has been knocked." btn.Text,win.Title) |> ignore
        )

    win.Show()
    //Application().Run(win) |> ignore
with e -> MessageBox.Show(e.Message,"Exception Error") |> ignore


