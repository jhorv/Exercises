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

type NamedBrush(str: string,br: Brush) =
    static let nbrs =
        typeof<Brushes>.GetProperties()
        |> Array.map(fun x -> NamedBrush(x.Name,x.GetValue(null) :?> Brush))

    new() = NamedBrush(nbrs.[0].ToString(),nbrs.[0].Brush)

    static member NamedAll 
        with get() = nbrs

    member t.Brush
        with get() = br

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
    win.Title <- "List Named Brushes"

    let lstbox = ListBox()
    lstbox.Width <- 150.0
    lstbox.Height <- 150.0

    win.Content <- lstbox

    lstbox.ItemsSource <- NamedBrush.NamedAll
    lstbox.DisplayMemberPath <- "Name"
    lstbox.SelectedValuePath <- "Brush"

    lstbox.SetBinding(ListBox.SelectedValueProperty,"Background") |> ignore
    lstbox.DataContext <- win

    win.Show()
    //Application().Run(win) |> ignore
with e -> MessageBox.Show(e.Message,"Exception Error") |> ignore

