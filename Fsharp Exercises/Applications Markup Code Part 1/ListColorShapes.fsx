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
    win.Title <- "List Color Shapes"

    let lstbox = ListBox()
    lstbox.Width <- 150.0
    lstbox.Height <- 150.0
    win.Content <- lstbox

    typeof<Brushes>.GetProperties()
    |> Array.iter (fun x ->
        let elips = Ellipse()
        elips.Width <- 100.0
        elips.Height <- 25.0
        elips.Margin <- Thickness(10.0,5.0,0.0,5.0)
        elips.Fill <- x.GetValue(null) :?> Brush
        lstbox.Items.Add elips |> ignore)

    lstbox.SelectionChanged.Add (fun x ->
        if lstbox.SelectedIndex <> -1 then
            win.Background <- (lstbox.SelectedItem :?> Shape).Fill
        )

    win.Show()
    //Application().Run(win) |> ignore
with e -> MessageBox.Show(e.Message,"Exception Error") |> ignore


