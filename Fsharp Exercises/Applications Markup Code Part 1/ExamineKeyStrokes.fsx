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

let strHeader =
    let comb = "{0,-10}{1,-20}{2,-10}{3,-10}{4,-15}{5,-8}{6,-7}{7,-10}{8,-10}{9,-10}{10,-10}{11,-10}"
    let strs = [|"Event";"Key";"Sys-Key";"Text";"Ctrl-Text";"Sys-Text";"Ime";"KeyStates";"IsDown";"IsUp";"IsToggled";"IsRepeat"|] |> Array.map box
    String.Format(comb,strs)
let strFormatKey = "{0,-10}{1,-20}{2,-10}{3,-10}{4,-15}{5,-8}{6,-7}{7,-10}{8,-10}"
let strFormatText = "{0,-10}{1,-10}{2,-10}{3,-10}"

[<STAThreadAttribute>]
try 
    let win = Window()
    win.Title <- "Examine Keystrokes"

    win.FontFamily <- FontFamily("Courier New")
    let grid = Grid()
    win.Content <- grid

    for l in [|GridLength.Auto;GridLength(1.0,GridUnitType.Star)|] do
        let rowdef = RowDefinition()
        rowdef.Height <- l
        grid.RowDefinitions.Add rowdef

    let txtHeader = TextBlock()
    txtHeader.FontWeight <- FontWeights.Bold
    txtHeader.Text <- strHeader
    grid.Children.Add txtHeader |> ignore

    let scroll = ScrollViewer()
    grid.Children.Add scroll |> ignore
    Grid.SetRow(scroll,1)

    let stack = StackPanel()
    scroll.Content <- stack

    let displayInfo str =
        let text = TextBlock()
        text.Text <- str
        stack.Children.Add text |> ignore
        scroll.ScrollToBottom()

    let displayKeyInfo (args: KeyEventArgs) =
        String.Format(strFormatKey,
            args.RoutedEvent.Name,args.Key,
            args.SystemKey,args.ImeProcessedKey,args.KeyStates,
            args.IsDown,args.IsUp,args.IsToggled,args.IsRepeat)
        |> displayInfo
        
    win.KeyDown.Add displayKeyInfo
    win.KeyUp.Add displayKeyInfo
    win.TextInput.Add (fun args ->
        String.Format(strFormatText,
            args.RoutedEvent.Name,args.Text,
            args.ControlText,args.SystemText)
        |> displayInfo
        )

    win.Show()
    //Application().Run(win) |> ignore
with e -> MessageBox.Show(e.Message,"Exception Error") |> ignore

