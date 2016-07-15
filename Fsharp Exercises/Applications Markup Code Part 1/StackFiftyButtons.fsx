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

type StackFiftyButtons() as t =
    inherit Window()

    do 
        t.MinHeight <- 300.0
        t.MinWidth <- 300.0
        t.WindowStartupLocation <- WindowStartupLocation.CenterScreen
        t.SizeToContent <- SizeToContent.WidthAndHeight

        t.Title <- "Stack Thirty Buttons"
        let stackmain = StackPanel()
        stackmain.Background <- Brushes.Aquamarine
        stackmain.Margin <- Thickness 10.0
        stackmain.Orientation <- Orientation.Horizontal
        stackmain.HorizontalAlignment <- HorizontalAlignment.Center

        let scroll = ScrollViewer()
        scroll.Content <- stackmain

        t.Content <- scroll

        t.AddHandler(Button.ClickEvent, RoutedEventHandler(fun _ args ->
            MessageBox.Show(sprintf "You have clicked the button labeled %A." (args.Source :?> Button).Content ,"Button Click") |> ignore)
            )
        
        let rng = Random()
        for p=0 to 2 do
            let pan = StackPanel()
            pan.Background <- Brushes.Red
            pan.Margin <- Thickness 2.0
            for i=0 to 29 do
                let btn = Button()
                btn.Margin <- Thickness 2.0
                btn.FontSize <- rng.Next(10) |> float |> (+) btn.FontSize
                btn.Content <- sprintf "No.(%i,%i)" p i
                pan.Children.Add btn |> ignore
            stackmain.Children.Add pan |> ignore


try StackFiftyButtons().Show()
with e -> MessageBox.Show(e.Message,"Exception Error") |> ignore

//[<STAThreadAttribute>]
//do
//    try Application().Run(EditSomeRichText()) |> ignore
//    with e -> MessageBox.Show(e.Message,"Exception Error") |> ignore






