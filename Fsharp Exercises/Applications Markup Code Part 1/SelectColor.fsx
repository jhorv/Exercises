// In the book Petzold forgot to register the SelectedColorChanged event handler.

// Edit: No, nevermind, that is just for RoutedEvents. I have no idea what is going 
// on there with that null. At any rate, F# Event class is the solution I was looking for.

// Also I noticed that in OnLostKeyboardFocus he calls base.OnGotKeyboardFocus(args)
// instead of base.OnLostKeyboardFocus(args) like he should.

// Edit2: This example has errors out the wazoo.
// In OnKeyDown, near the end, the last branch of the if statement lacks null checks.
//                cellHighlighted.IsHighlighted = false;
//                cellHighlighted = cells[y, x];
//                cellHighlighted.IsHighlighted = true;

// Also those overloaded mouse functions that cast to ColorCell I needed to turn into closures
// and move into the constructor becase they kept throwing exceptions due to trying to cast 
// UniformGrid to ColorCell. Maybe adding args.Handled <- true...no that would not have worked.

// Edit3: Another missing null check in OnKeyDown.
//        | Key.Enter | Key.Space ->
//            if cellSelected <> null then
//                cellSelected.IsSelected <- false
//                cellSelected <- cellHighlighted
//                cellSelected.IsSelected <- true

// That last line throws an error if cellHighlighted turns out to be null.

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

[<AllowNullLiteral>]
type ColorCell(clr: Color) as t =
    inherit FrameworkElement()

    static let sizeCell = Size(20.0,20.0)

    static let isSelectedProperty = DependencyProperty.Register("IsSelected",typeof<bool>,typeof<ColorCell>,FrameworkPropertyMetadata(false,FrameworkPropertyMetadataOptions.AffectsRender))
    static let isHighlighedProperty = DependencyProperty.Register("IsHighlighted",typeof<bool>,typeof<ColorCell>,FrameworkPropertyMetadata(false,FrameworkPropertyMetadataOptions.AffectsRender))

    let brush = SolidColorBrush clr
    let visColor =
        // Create a new DrawingVisual and store as field.
        let visColor = DrawingVisual()
        let dc = visColor.RenderOpen()

        // Draw a rectangle with the color argument.
        let rect = Rect(Point(0.0,0.0),sizeCell)
        rect.Inflate(-4.0,-4.0)
        let pen = Pen(SystemColors.ControlTextBrush,1.0)
        dc.DrawRectangle(brush,pen,rect)
        dc.Close()

        // AddVisualChild is necessary for event routing!
        t.AddVisualChild visColor
        t.AddLogicalChild visColor
        visColor

    static member IsSelectedProperty = isSelectedProperty
    static member IsHighlightedProperty = isHighlighedProperty

    member t.IsSelected
        with set (v: bool) = t.SetValue(isSelectedProperty,v)
        and get () = t.GetValue(isSelectedProperty) :?> bool

    member t.IsHighlighted
        with set(v: bool) = t.SetValue(isHighlighedProperty,v)
        and get() = t.GetValue(isHighlighedProperty) :?> bool

    member t.Brush with get() = brush

    override t.VisualChildrenCount = 1

    override t.GetVisualChild index = 
        if index <> 0 then
            raise <| ArgumentOutOfRangeException(sprintf "index(%i)" index)
        visColor :> Visual

    override t.MeasureOverride availableSize = sizeCell
    
    override t.OnRender dc =
        let rect = Rect(Point(0.0,0.0),t.RenderSize)
        rect.Inflate(-1.0,-1.0)
        let pen = Pen(SystemColors.HighlightBrush,1.0)

        if t.IsHighlighted then dc.DrawRectangle(SystemColors.ControlDarkBrush,pen,rect)
        elif t.IsSelected then dc.DrawRectangle(SystemColors.ControlLightBrush,pen,rect)
        else dc.DrawRectangle(Brushes.Transparent,null,rect)
            
type ColorGrid() as t =
    inherit Control()

    // Number of rows and columns
    static let yNum = 5
    static let xNum = 8

    static let strColorsNames, colors =
        [|
            [|"Black"; "Brown"; "DarkGreen"; "MidnightBlue"; "Navy"; "DarkBlue"; "Indigo"; "DimGray"|]
            [|"DarkRed"; "OrangeRed"; "Olive"; "Green"; "Teal"; "Blue"; "SlateGray"; "Gray"|]
            [|"Red"; "Orange"; "YellowGreen"; "SeaGreen"; "Aqua"; "LightBlue"; "Violet"; "DarkGray"|]
            [|"Pink"; "Gold"; "Yellow"; "Lime"; "Turquoise"; "SkyBlue"; "Plum"; "LightGray"|]
            [|"LightPink"; "Tan"; "LightYellow"; "LightGreen"; "LightCyan"; "LightSkyBlue"; "Lavender"; "White"|]
        |]
        |> fun x -> 
            Array2D.init yNum xNum (fun r c -> x.[r].[c]),
            Array2D.init yNum xNum (fun r c -> x.[r].[c] |> fun colorName -> typeof<Colors>.GetProperty(colorName).GetValue(null,null) :?> Color)

    let mutable clrSelected = Colors.Black
            
    let bord = 
        let bord = Border()
        bord.BorderBrush <- SystemColors.ControlDarkBrush
        bord.BorderThickness <- Thickness 1.0
        t.AddVisualChild bord
        t.AddLogicalChild bord
        bord

    let unigrid =
        let unigrid = UniformGrid()
        unigrid.Background <- SystemColors.WindowBrush
        unigrid.Columns <- xNum
        bord.Child <- unigrid
        unigrid

    let mutable cellSelected : ColorCell = null
    let mutable cellHighlighted : ColorCell = null
    let cells = 
        Array2D.init yNum xNum (fun y x ->
            let clr = colors.[y,x]
            let cell = ColorCell(clr)
            unigrid.Children.Add cell |> ignore
            if clr = clrSelected then
                cellSelected <- cell
                cell.IsSelected <- true
            let tip = ToolTip()
            tip.Content <- strColorsNames.[y,x]
            cell.ToolTip <- tip

            cell.MouseMove.Add(fun args ->
                if cell <> null then
                    if cellHighlighted <> null then cellHighlighted.IsHighlighted <- false
                    cellHighlighted <- cell
                    cellHighlighted.IsHighlighted <- true)

            cell.MouseDown.Add(fun args ->
                if cell <> null then
                    if cellSelected <> null then cellSelected.IsSelected <- false
                    cellSelected <- cell
                    cellSelected.IsHighlighted <- true
                t.Focus() |> ignore)

            cell.MouseUp.Add(fun args ->
                if cell <> null then
                    if cellSelected <> null then cellSelected.IsSelected <- false
                    cellSelected <- cell
                    cellSelected.IsHighlighted <- true

                    clrSelected <- cellSelected.Brush.Color
                    t.OnSelectedColorChanged EventArgs.Empty)

            cell)

    let selectedColorChangedEvent = Event<_>()

    [<CLIEvent>] member t.SelectedColorChanged = selectedColorChangedEvent.Publish

    member t.SelectedColor = clrSelected
    override t.VisualChildrenCount = 1

    override t.GetVisualChild index =
        if index <> 0 then
            raise <| ArgumentOutOfRangeException (sprintf "index(%i)" index)
        bord :> Visual

    override t.MeasureOverride sizeAvailable =
        bord.Measure sizeAvailable
        bord.DesiredSize

    override t.ArrangeOverride sizeFinal =
        bord.Arrange(Rect(Point(0.0,0.0),sizeFinal))
        sizeFinal

    override t.OnMouseEnter args =
        base.OnMouseEnter args
        if cellHighlighted <> null then
            cellHighlighted.IsHighlighted <- false
            cellHighlighted <- null

    override t.OnMouseLeave args =
        base.OnMouseLeave args
        if cellHighlighted <> null then
            cellHighlighted.IsHighlighted <- false
            cellHighlighted <- null

    abstract member OnSelectedColorChanged: EventArgs -> unit
    default t.OnSelectedColorChanged args = selectedColorChangedEvent.Trigger args

    override t.OnGotKeyboardFocus args =
        base.OnGotKeyboardFocus args
        if cellHighlighted <> null then
            if cellSelected <> null then 
                cellHighlighted <- cellSelected
            else
                cellHighlighted <- cells.[0,0]
            cellHighlighted.IsHighlighted <- true

    override t.OnLostKeyboardFocus args =
        base.OnLostKeyboardFocus args

        if cellHighlighted <> null then
            cellHighlighted.IsHighlighted <- false
            cellHighlighted <- null

    override t.OnKeyDown args =
        base.OnKeyDown args
        let index = unigrid.Children.IndexOf cellHighlighted
        let mutable y, x = index / xNum, index % xNum

        match args.Key with
        | Key.Home -> y <- 0; x <- 0
        | Key.End -> y <- yNum-1; x <- xNum-1
        | Key.Down -> 
            y <- (y+1) % yNum
            if y = 0 then x <- x+1
        | Key.Up ->
            y <- (y+yNum-1) % yNum
            if y = yNum-1 then x <- x-1
        | Key.Right -> 
            x <- (x+1) % xNum
            if x = 0 then y <- y+1
        | Key.Left ->
            x <- (x+xNum-1) % xNum
            if x = xNum-1 then y <- y-1
        | Key.Enter | Key.Space ->
            if cellSelected <> null then
                cellSelected.IsSelected <- false
                if cellHighlighted <> null then
                    cellSelected <- cellHighlighted
                    cellSelected.IsSelected <- true
                    clrSelected <- cellSelected.Brush.Color
                    t.OnSelectedColorChanged EventArgs.Empty
        | _ -> ()

        if x >= xNum || y >= yNum then
            t.MoveFocus(TraversalRequest(FocusNavigationDirection.Next)) |> ignore
        elif x < 0 || y < 0 then
            t.MoveFocus(TraversalRequest(FocusNavigationDirection.Previous)) |> ignore
        else
            if cellHighlighted <> null then cellHighlighted.IsHighlighted <- false
            cellHighlighted <- cells.[y,x]
            cellHighlighted.IsHighlighted <- true
        args.Handled <- true


[<STAThreadAttribute>]
try 
    let win = Window()
    win.Title <- "Select Color"
    win.SizeToContent <- SizeToContent.WidthAndHeight

    let stack = StackPanel()
    stack.Orientation <- Orientation.Horizontal
    win.Content <- stack

    Button()
    |> fun btn ->
        btn.Content <- "Do-nothing button\nto test tabbing"
        btn.Margin <- Thickness 24.0
        btn.HorizontalAlignment <- HorizontalAlignment.Center
        btn.VerticalAlignment <- VerticalAlignment.Center
        stack.Children.Add btn |> ignore

    let clrGrid = ColorGrid()
    clrGrid.Margin <- Thickness 24.0
    clrGrid.HorizontalAlignment <- HorizontalAlignment.Center
    clrGrid.VerticalAlignment <- VerticalAlignment.Center
    clrGrid.SelectedColorChanged.Add (fun _ -> win.Background <- clrGrid.SelectedColor |> SolidColorBrush)
    stack.Children.Add clrGrid |> ignore

    Button()
    |> fun btn ->
        btn.Content <- "Do-nothing button\nto test tabbing"
        btn.Margin <- Thickness 24.0
        btn.HorizontalAlignment <- HorizontalAlignment.Center
        btn.VerticalAlignment <- VerticalAlignment.Center
        stack.Children.Add btn |> ignore

    win.Show()
    Application().Run(win) |> ignore
with e -> MessageBox.Show(e.Message,"Exception Error") |> ignore


