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
open System.ComponentModel
open System.Windows
open System.Windows.Input
open System.Windows.Controls
open System.Windows.Controls.Primitives
open System.Windows.Media
open System.Windows.Media.Imaging
open System.Windows.Shapes
open System.Windows.Documents

type TuneTheRadio() as t =
    inherit Window()

    do 
        t.MinHeight <- 300.0
        t.MinWidth <- 300.0
        t.WindowStartupLocation <- WindowStartupLocation.CenterScreen
        t.SizeToContent <- SizeToContent.WidthAndHeight

        t.Title <- "Tune The Radio"

        let group = GroupBox()
        group.Header <- "Windows Control"
        group.Margin <- Thickness(96.0)
        group.Padding <- Thickness(5.0)
        t.Content <- group

        let stack = StackPanel()
        group.Content <- stack

        let CreateRadioButton str style =
            let radio = RadioButton()
            radio.Content <- str
            radio.Tag <- style
            radio.Margin <- Thickness 5.0
            radio.IsChecked <- (style = t.WindowStyle) |> Nullable
            radio

        stack.Children.Add(CreateRadioButton "No border or caption" WindowStyle.None) |> ignore
        stack.Children.Add(CreateRadioButton "Single border window" WindowStyle.SingleBorderWindow) |> ignore
        stack.Children.Add(CreateRadioButton "3D-border window" WindowStyle.ThreeDBorderWindow) |> ignore
        stack.Children.Add(CreateRadioButton "Tool window" WindowStyle.ToolWindow) |> ignore

        stack.AddHandler(RadioButton.CheckedEvent,RoutedEventHandler(fun _ args ->
            let radio = args.Source :?> RadioButton
            t.WindowStyle <- radio.Tag :?> WindowStyle
            ))

try TuneTheRadio().Show()
with e -> MessageBox.Show(e.Message,"Exception Error") |> ignore

//[<STAThreadAttribute>]
//do
//    try Application().Run(TuneTheRadio()) |> ignore
//    with e -> MessageBox.Show(e.Message,"Exception Error") |> ignore
