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

type SpaceButton() =
    inherit Button()

    let mutable txt = ""

    static let SpaceProperty' =
        let metadata = FrameworkPropertyMetadata()
        metadata.DefaultValue <- 1
        metadata.AffectsMeasure <- true
        metadata.Inherits <- true
        metadata.PropertyChangedCallback <- PropertyChangedCallback(SpaceButton.OnSpacePropertyChanged)
            
        DependencyProperty.Register(
            "Space", typeof<int>, 
            typeof<SpaceButton>, metadata,ValidateValueCallback SpaceButton.ValidateSpaceValue)

    static member inline SpaceProperty = SpaceProperty'

    member t.Text
        with get() = txt
        and set v = txt <- v; t.Content <- t.SpaceOutText txt

    member t.Space
        with get() = t.GetValue(SpaceButton.SpaceProperty) :?> int
        and set (v: int) = t.SetValue(SpaceButton.SpaceProperty,v)

    member t.SpaceOutText(str: string) =
        if str = null then null
        else
            let build = StringBuilder()
            for x in str do
                build.Append(x).Append(' ',t.Space) |> ignore
            build.ToString()

    static member ValidateSpaceValue(x:obj) = (x :?> int) >= 0

    static member OnSpacePropertyChanged(obj: DependencyObject)(args: DependencyPropertyChangedEventArgs) =
        let btn = obj :?> SpaceButton
        btn.Content <- btn.SpaceOutText(btn.Text)

type SpaceWindow() =
    inherit Window()

    static let SpaceProperty' = 
        let metadata = FrameworkPropertyMetadata()
        metadata.Inherits <- true
        let x = SpaceButton.SpaceProperty.AddOwner(typeof<SpaceWindow>)
        SpaceButton.SpaceProperty.OverrideMetadata(typeof<SpaceWindow>,metadata)
        x

    static member inline SpaceProperty = SpaceProperty'

    member t.Space
        with get() = t.GetValue(SpaceWindow.SpaceProperty) :?> int
        and set (v: int) = t.SetValue(SpaceWindow.SpaceProperty,v)

[<STAThreadAttribute>]
try 
    let win = SpaceWindow()
    win.Content <- "Set Space Property"
    win.SizeToContent <- SizeToContent.WidthAndHeight
    win.ResizeMode <- ResizeMode.CanMinimize
    let iSpaces = [|0;1;2|]

    let grid = Grid()
    win.Content <- grid

    for i=0 to 1 do
        let rowdef = RowDefinition()
        rowdef.Height <- GridLength.Auto
        grid.RowDefinitions.Add rowdef

    iSpaces
    |> Array.iter(fun space ->
        let coldef = ColumnDefinition()
        coldef.Width <- GridLength.Auto
        grid.ColumnDefinitions.Add coldef
        )
    iSpaces
    |> Array.iteri(fun i space ->
        SpaceButton()
        |> fun btn ->
            btn.Text <- sprintf "Set window space to %i" space
            btn.Tag <- space
            btn.HorizontalAlignment <- HorizontalAlignment.Center
            btn.VerticalAlignment <- VerticalAlignment.Center
            btn.Click.Add (fun x -> win.Space <- btn.Tag :?> int)
            grid.Children.Add btn |> ignore
            Grid.SetRow(btn,0)
            Grid.SetColumn(btn,i)

        SpaceButton()
        |> fun btn ->
            btn.Text <- sprintf "Set button space to %i" space
            btn.Tag <- space
            btn.HorizontalAlignment <- HorizontalAlignment.Center
            btn.VerticalAlignment <- VerticalAlignment.Center
            btn.Click.Add (fun x -> btn.Space <- btn.Tag :?> int)
            grid.Children.Add btn |> ignore
            Grid.SetRow(btn,1)
            Grid.SetColumn(btn,i)
        )

    win.Show()
    //Application().Run(win) |> ignore
with e -> MessageBox.Show(e.Message,"Exception Error") |> ignore
        


