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

type NamedColor(str: string,clr: Color) =
    static let nclrs =
        typeof<Colors>.GetProperties()
        |> Array.map(fun x -> NamedColor(x.Name,x.GetValue(null) :?> Color))

    new() = NamedColor(nclrs.[0].ToString(),nclrs.[0].Color)

    static member NamedAll 
        with get() = nclrs

    member t.Color
        with get() = clr

    member t.Name
        with get() =
            let b = StringBuilder(str.Length)
            let inline append (x: ^a) = b.Append x |> ignore
            append str.[0]
            for i=1 to str.Length-1 do
                let s = str.[i]
                if Char.IsUpper(s) then append " " 
                append s
            b.ToString()

    override t.ToString() = str


[<STAThreadAttribute>]
try 
    let win = Window()
    win.Title <- "List Named Colors"

    let lstbox = ListBox()
    lstbox.Width <- 150.0
    lstbox.Height <- 150.0
    lstbox.SelectionChanged.Add <|fun args ->
        if lstbox.SelectedValue <> null then
            let clr = lstbox.SelectedValue :?> Color
            win.Background <- clr |> SolidColorBrush

    win.Content <- lstbox

    lstbox.ItemsSource <- NamedColor.NamedAll
    lstbox.DisplayMemberPath <- "Name"
    lstbox.SelectedValuePath <- "Color"

    win.Show()
    //Application().Run(win) |> ignore
with e -> MessageBox.Show(e.Message,"Exception Error") |> ignore



