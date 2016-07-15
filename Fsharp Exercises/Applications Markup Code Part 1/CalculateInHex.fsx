// Again he modified sizeAvailable pointlessly in the MeasureOverride?
// Am I missunderstanding something here?

// Edit: Well, I translated it directly and it works.
// It is amazing how poorly designed this program is - I am in awe.
// It might be due to lack of C# features in 2006, who knows.

// As the book is over 1000 pages, I do not feel like rewriting it
// in idiomatic Fsharp right now though. I indend to do a parsing
// calculator when I am done with it.

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

type RoundedButtonDecorator() =
    inherit Decorator()
    
    static let ADD_TO_RADIUS = 10.0 // The default 2.0 from the book might be too small without margin.
    static let isPressedProperty = DependencyProperty.Register("IsPressed",typeof<bool>,typeof<RoundedButtonDecorator>,FrameworkPropertyMetadata(false,FrameworkPropertyMetadataOptions.AffectsRender))

    member t.IsPressed
        with get() = t.GetValue(isPressedProperty) :?> bool
        and set (v: bool) = t.SetValue(isPressedProperty,v)

    // Returns the available size for the child element.
    override t.MeasureOverride sizeAvailable =
        if t.Child <> null then
            t.Child.Measure sizeAvailable
            t.Child.DesiredSize.Width, t.Child.DesiredSize.Height
        else 0.0,0.0
        |> fun (w,h) -> Size(w+ADD_TO_RADIUS,h+ADD_TO_RADIUS)

    // Return the position of the child within the control.
    override t.ArrangeOverride sizeArrange =
        if t.Child <> null then
            let ptChild =
                let x = max 1.0 ((sizeArrange.Width - t.DesiredSize.Width + ADD_TO_RADIUS) / 2.0)
                let y = max 1.0 ((sizeArrange.Height - t.DesiredSize.Height + ADD_TO_RADIUS) / 2.0)
                Point(x,y)
            t.Child.Arrange(Rect(ptChild,t.Child.DesiredSize))
        sizeArrange

    override t.OnRender dc =
        let brush = 
            let startColor = if t.IsPressed then SystemColors.ControlDarkColor else SystemColors.ControlLightLightColor
            RadialGradientBrush(startColor, SystemColors.ControlColor)
        brush.GradientOrigin <- if t.IsPressed then Point(0.75,0.75) else Point(0.25,0.25)
        dc.DrawRoundedRectangle(brush,Pen(SystemColors.ControlDarkDarkBrush,1.0),Rect(Point(0.0,0.0),t.RenderSize),t.RenderSize.Width/2.0,t.RenderSize.Height/2.0)

type RoundedButton() as t =
    inherit Control()

    static let clickEvent = EventManager.RegisterRoutedEvent("ClickEvent",RoutingStrategy.Bubble,typeof<RoutedEventHandler>,typeof<RoundedButton>)
    let evClick =
        {new IDelegateEvent<RoutedEventHandler> with
            member this.AddHandler v  = t.AddHandler(clickEvent,v)
            member this.RemoveHandler v = t.RemoveHandler(clickEvent,v)}

    let decorator = 
        let x = RoundedButtonDecorator()
        t.AddVisualChild x
        t.AddLogicalChild x
        x

    let isMouseReallyOver() =
        let pt = Mouse.GetPosition t
        pt.X >= 0.0 && pt.X < t.ActualWidth && pt.Y >= 0.0 && pt.Y < t.ActualHeight

    static member ClickEvent = clickEvent

    [<CLIEvent>] member t.Click = evClick
    
    member t.Child
        with get() = decorator.Child
        and set v = decorator.Child <- v

    member t.IsPressed
        with get() = decorator.IsPressed
        and set v = decorator.IsPressed <- v

    override t.VisualChildrenCount = 1

    override t.GetVisualChild index =
        if index <> 0 then
            raise <| ArgumentOutOfRangeException(sprintf "index(%i)" index)
        decorator :> Visual

    override t.MeasureOverride sizeAvailable =
        decorator.Measure sizeAvailable
        decorator.DesiredSize

    override t.ArrangeOverride sizeArrange =
        decorator.Arrange(Rect(Point(0.0,0.0),sizeArrange))
        sizeArrange

    override t.OnMouseMove args =
        base.OnMouseMove args
        if t.IsMouseCaptured then
            t.IsPressed <- isMouseReallyOver()

    abstract member OnClick: unit -> unit
    default t.OnClick() =
        let argsEvent = RoutedEventArgs()
        argsEvent.RoutedEvent <- clickEvent
        argsEvent.Source <- t
        t.RaiseEvent argsEvent
        

    override t.OnMouseLeftButtonDown args =
        base.OnMouseLeftButtonDown args
        t.CaptureMouse() |> ignore
        t.IsPressed <- true
        args.Handled <- true

    override t.OnMouseLeftButtonUp args =
        base.OnMouseLeftButtonUp args
        if t.IsMouseCaptured then
            if isMouseReallyOver() then t.OnClick()
            Mouse.Capture null |> ignore
            t.IsPressed <- false
            args.Handled <- true

[<STAThreadAttribute>]
try 
    let win = Window()
    win.Title <- "Calculate In Hex"
    win.SizeToContent <- SizeToContent.WidthAndHeight
    win.ResizeMode <- ResizeMode.CanMinimize

    let grid = Grid()
    grid.Margin <- Thickness 4.0
    win.Content <- grid

    for i=1 to 5 do
        let col = ColumnDefinition()
        col.Width <- GridLength.Auto
        grid.ColumnDefinitions.Add col

    for i=1 to 7 do
        let row = RowDefinition()
        row.Height <- GridLength.Auto
        grid.RowDefinitions.Add row

    let strButtons =
        [|
        "0"
        "D";"E";"F";"+";"&"
        "A";"B";"C";"-";"|"
        "7";"8";"9";"*";"^"
        "4";"5";"6";"/";"<<"
        "1";"2";"3";"%";">>"
        "0";"Back";"Equals"|]

    let mutable iRow, iCol = 0,0
    let mutable btnDisplay = None
    let mutable numDisplay, numFirst, bNewNumber = 0UL, 0UL, true
    let mutable chOperation = '='

    for str in strButtons do
        let btn = RoundedButton()
        btn.Focusable <- false
        btn.Height <- 32.0
        btn.Margin <- Thickness 4.0
        
        // Create TextBlock for Child of RoundedButton.
        let txt = TextBlock()
        txt.Text <- str
        btn.Child <- txt

        btn.Click.AddHandler(fun sender args ->
            let chButton = if str = "Equals" then '=' else str.[0]
            if btn = btnDisplay.Value then numDisplay <- 0UL
            elif str = "Back" then numDisplay <- numDisplay/16UL
            elif Char.IsLetterOrDigit chButton then
                if bNewNumber then
                    numFirst <- numDisplay
                    numDisplay <- 0UL
                    bNewNumber <- false
                elif numDisplay <= (UInt64.MaxValue >>> 4) then
                    numDisplay <- 16UL*numDisplay + uint64 (byte chButton - (if Char.IsDigit(chButton) then byte '0' else byte 'A' - 10uy))
            else
                if bNewNumber = false then
                    match chOperation with
                    | '=' -> ()
                    | '+' -> numDisplay <- numFirst + numDisplay
                    | '-' -> numDisplay <- numFirst - numDisplay
                    | '*' -> numDisplay <- numFirst * numDisplay
                    | '/' -> numDisplay <- if numDisplay <> 0UL then numFirst / numDisplay else UInt64.MaxValue
                    | '%' -> numDisplay <- if numDisplay <> 0UL then numFirst % numDisplay else UInt64.MaxValue
                    | '&' -> numDisplay <- numFirst &&& numDisplay
                    | '|' -> numDisplay <- numFirst ||| numDisplay
                    | '^' -> numDisplay <- numFirst ^^^ numDisplay
                    | '<' -> numDisplay <- numFirst <<< int numDisplay
                    | '>' -> numDisplay <- numFirst >>> int numDisplay
                    | _ -> numDisplay <- 0UL
                bNewNumber <- true
                chOperation <- chButton
            let txt = TextBlock()
            txt.Text <- String.Format("{0:X}",numDisplay)
            btnDisplay.Value.Child <- txt
            )

        // Add RoundedButton to Grid.
        grid.Children.Add btn |> ignore
        Grid.SetRow(btn,iRow)
        Grid.SetColumn(btn,iCol)

        // Make an exception for the Display button.
        if iRow = 0 && iCol = 0 then
            btnDisplay <- Some btn
            btn.Margin <- Thickness(4.0,4.0,4.0,6.0)
            Grid.SetColumnSpan(btn,5)
            iRow <- 1
        // Also for Back and Equals
        elif iRow = 6 && iCol > 0 then
            Grid.SetColumnSpan(btn, 2)
            iCol <- iCol+2
        // For all other buttons
        else
            btn.Width <- 32.0
            iCol <- (iCol+1) % 5
            if iCol = 0 then iRow <- iRow+1

    win.TextInput.Add(fun args ->
        if args.Text.Length > 0 then
            // Get character input.
            let chKey = Char.ToUpper args.Text.[0]

            // Loop through buttons.
            for child in grid.Children do
                let btn = child :?> RoundedButton
                let strButton = (btn.Child :?> TextBlock).Text
                // Messy logic to check for matching button.
                if  (chKey = strButton.[0] && btn <> btnDisplay.Value && strButton <> "Equals" && strButton <> "Back") ||
                    (chKey = '=' && strButton = "Equals") ||
                    (chKey = '\r' && strButton = "Equals") ||
                    (chKey = '\b' && strButton = "Back") ||
                    (chKey = '\x1B' && btn = btnDisplay.Value) then
                    // Simulate Click event to process keystroke.
                    let argsClick = RoutedEventArgs(RoundedButton.ClickEvent,btn)
                    btn.RaiseEvent argsClick
                    // Make the button appear as if it's pressed.
                    btn.IsPressed <- true

                    // Set timer to unpress button.
                    let tmr = DispatcherTimer()
                    tmr.Interval <- TimeSpan.FromMilliseconds(100.0)
                    //tmr.Tag <- btn // No need for this as I am using closure everywhere
                    tmr.Tick.Add(fun _ ->
                        // Unpress button.
                        btn.IsPressed <- false
                        // Turn off the timer (unlike in the example I've skipped removing the event handler)
                        tmr.Stop()
                        )
                    tmr.Start()
                    args.Handled <- true
        )

    win.Show()
    Application().Run(win) |> ignore
with e -> MessageBox.Show(e.Message,"Exception Error") |> ignore
