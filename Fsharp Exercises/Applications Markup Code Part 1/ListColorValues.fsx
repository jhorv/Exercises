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

[<STAThreadAttribute>]
try 
    let win = Window()
    win.Title <- "List Color Values"

    let lstbox = ListBox()
    lstbox.Width <- 150.0
    lstbox.Height <- 150.0
    lstbox.SelectionChanged.Add <|fun args ->
        if lstbox.SelectedIndex <> -1 then
            let clr = lstbox.SelectedItem :?> Color
            win.Background <- clr |> SolidColorBrush

    win.Content <- lstbox

    typeof<Colors>.GetProperties()
    |> Array.iter (fun x -> lstbox.Items.Add(x.GetValue(null)) |> ignore)

    win.Show()
    //Application().Run(win) |> ignore
with e -> MessageBox.Show(e.Message,"Exception Error") |> ignore


