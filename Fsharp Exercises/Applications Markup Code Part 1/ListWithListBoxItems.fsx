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
    win.Title <- "List With List Box Items"

    let lstbox = ListBox()
    lstbox.Width <- 150.0
    lstbox.Height <- 150.0
    win.Content <- lstbox

    typeof<Colors>.GetProperties()
    |> Array.iter (fun x ->
        let clr = x.GetValue(null) :?> Color
        
        let item = ListBoxItem()
        item.Content <- x.Name
        item.Background <- SolidColorBrush clr
        item.Foreground <- 
            let isBlack = 0.222 * float clr.R + 0.707 * float clr.G + 0.071 * float clr.B > 128.0
            if isBlack then Brushes.Black else Brushes.White

        item.HorizontalAlignment <- HorizontalAlignment.Center
        item.Padding <- Thickness 2.0
        lstbox.Items.Add item |> ignore
        )

    lstbox.SelectionChanged.Add (fun args ->
        if args.RemovedItems.Count > 0 then
            let item = args.RemovedItems.[0] :?> ListBoxItem
            let str = item.Content :?> string
            item.Content <- str.Substring(2,str.Length - 4)
            item.FontWeight <- FontWeights.Regular
        if args.AddedItems.Count > 0 then
            let item = args.AddedItems.[0] :?> ListBoxItem
            let str = item.Content :?> string
            item.Content <- sprintf "[ %s ]" str
            item.FontWeight <- FontWeights.Bold
        let item = lstbox.SelectedItem :?> ListBoxItem
        if item <> null then win.Background <- item.Background
        )

    win.Show()
    //Application().Run(win) |> ignore
with e -> MessageBox.Show(e.Message,"Exception Error") |> ignore


