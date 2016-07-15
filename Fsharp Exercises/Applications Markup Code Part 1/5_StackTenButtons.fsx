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

type StackTenButtons() as t =
    inherit Window()

    do 
        t.MinHeight <- 300.0
        t.MinWidth <- 300.0
        t.WindowStartupLocation <- WindowStartupLocation.CenterScreen
        t.SizeToContent <- SizeToContent.WidthAndHeight

        t.Title <- "Stack Ten Buttons"
        let pan = StackPanel()
        pan.Background <- Brushes.Aquamarine
        pan.Margin <- Thickness 10.0
        t.Content <- pan
        
        let rng = Random()
        for i=0 to 9 do
            let btn = Button()
            btn.Margin <- Thickness 2.0
            btn.Name <- 'A' + char i |> string
            btn.FontSize <- rng.Next(10) |> float |> (+) btn.FontSize
            btn.Content <- sprintf "Button %s says click me!" btn.Name
            btn.Click.Add(fun args ->
                MessageBox.Show(sprintf "Button %s has been clicked!" btn.Name,"Button Click") |> ignore
                )
            pan.Children.Add btn |> ignore


try StackTenButtons().Show()
with e -> MessageBox.Show(e.Message,"Exception Error") |> ignore

//[<STAThreadAttribute>]
//do
//    try Application().Run(EditSomeRichText()) |> ignore
//    with e -> MessageBox.Show(e.Message,"Exception Error") |> ignore




