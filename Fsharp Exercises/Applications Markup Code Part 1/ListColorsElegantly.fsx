
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

type ColorListBoxItem() as t =
    inherit ListBoxItem()
    
    // Create StackPanel for Rectangle and TextBlock.
    let stack = StackPanel()
    do  stack.Orientation <- Orientation.Horizontal
        t.Content <- stack

    // Create Rectangle to display color.
    let rect = Rectangle()
    do  rect.Width <- 16.0
        rect.Height <- 16.0
        rect.Margin <- Thickness 2.0
        rect.Stroke <- SystemColors.WindowTextBrush
        stack.Children.Add rect |> ignore
    
    let text = TextBlock()
    do  text.VerticalAlignment <- VerticalAlignment.Center
        stack.Children.Add text |> ignore

    let mutable str = ""

    member t.Text
        with get() = str
        and set(v: string) =
            str <- v
            let spaced =
                let b = StringBuilder(str.Length)
                let inline append (x: ^a) = b.Append x |> ignore
                append str.[0]
                for i=1 to str.Length-1 do
                    let s = str.[i]
                    if Char.IsUpper(s) then append " " 
                    append s
                b.ToString()
            text.Text <- spaced
        
    member t.Color
        with get() = 
            let brush = rect.Fill :?> SolidColorBrush
            if brush = null then Colors.Transparent else brush.Color
        and set(v: Color) = rect.Fill <- SolidColorBrush v

    override t.OnSelected args =
        base.OnSelected args
        text.FontWeight <- FontWeights.Bold

    override t.OnUnselected args =
        base.OnUnselected args
        text.FontWeight <- FontWeights.Regular

    override t.ToString() = str

type ColorListBox() as t =
    inherit ListBox()

    do
        typeof<Colors>.GetProperties()
        |> Array.iter (fun x ->
            let item = ColorListBoxItem()
            item.Text <- x.Name
            item.Color <- x.GetValue(null) :?> Color
            t.Items.Add item |> ignore
            )
        t.SelectedValuePath <- "Color"

    member t.SelectedColor
        with get() = t.SelectedValue :?> Color
        and set(v: Color) = t.SelectedValue <- v

[<STAThreadAttribute>]
try 
    let win = Window()
    win.Title <- "List Colors Elegantly"

    let lstbox = ColorListBox()
    lstbox.Width <- 150.0
    lstbox.Height <- 150.0
    win.Content <- lstbox

    lstbox.SelectedColor <- SystemColors.WindowColor
    lstbox.SelectionChanged.Add (fun args -> win.Background <- SolidColorBrush lstbox.SelectedColor)

    win.Show()
    //Application().Run(win) |> ignore
with e -> MessageBox.Show(e.Message,"Exception Error") |> ignore

