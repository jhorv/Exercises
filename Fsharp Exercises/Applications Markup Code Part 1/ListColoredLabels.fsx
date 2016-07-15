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
    win.Title <- "List Color Labels"

    let lstbox = ListBox()
    lstbox.Width <- 150.0
    lstbox.Height <- 150.0
    win.Content <- lstbox

    typeof<Colors>.GetProperties()
    |> Array.iter (fun x ->
        let clr = x.GetValue(null) :?> Color
        
        let lbl = Label()
        lbl.Content <- x.Name
        lbl.Background <- SolidColorBrush clr
        lbl.Foreground <- 
            let isBlack = 0.222 * float clr.R + 0.707 * float clr.G + 0.071 * float clr.B > 128.0
            if isBlack then Brushes.Black else Brushes.White

        lbl.Width <- 100.0
        lbl.Margin <- Thickness(15.0,0.0,0.0,0.0)
        lbl.Tag <- clr
        lstbox.Items.Add lbl |> ignore
        )

    lstbox.SelectionChanged.Add (fun x ->
        let lbl = lstbox.SelectedItem :?> Label
        if lbl <> null then
            let clr = lbl.Tag :?> Color
            win.Background <- SolidColorBrush clr
        )

    win.Show()
    //Application().Run(win) |> ignore
with e -> MessageBox.Show(e.Message,"Exception Error") |> ignore

