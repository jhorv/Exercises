#if INTERACTIVE
#r "PresentationCore.dll"
#r "PresentationFramework.dll"
#r "System.Xaml.dll"
#r "WindowsBase.dll"
#r "UIAutomationTypes.dll"
#endif

open System
open System.IO
open System.Windows
open System.Windows.Input
open System.Windows.Controls
open System.Windows.Media
open System.Windows.Media.Imaging
open System.Windows.Shapes
open System.Windows.Documents

type ToggleBoldAndItalic() as t =
    inherit Window()

    do 
        t.Height <- 300.0
        t.Width <- 300.0
        t.WindowStartupLocation <- WindowStartupLocation.CenterScreen
        t.SizeToContent <- SizeToContent.WidthAndHeight

        let txt = TextBlock()
        txt.FontSize <- 32.0
        txt.HorizontalAlignment <- HorizontalAlignment.Center
        txt.VerticalAlignment <- VerticalAlignment.Center
        t.Content <- txt
        t.Foreground <- Brushes.CornflowerBlue

        "To be or not to be, that is the question".Split()
        |> Array.iter (fun x ->
            let run = Run(x)
            run.MouseDown.Add (fun args ->
                match args.ChangedButton with
                | MouseButton.Left ->
                    run.FontStyle <- if run.FontStyle = FontStyles.Italic then FontStyles.Normal else FontStyles.Italic
                | MouseButton.Right ->
                    run.FontWeight <- if run.FontWeight = FontWeights.Bold then FontWeights.Normal else FontWeights.Bold
                | _ -> ()
                )
            txt.Inlines.Add(run)
            txt.Inlines.Add(" ")
            )



ToggleBoldAndItalic().Show()
//[<STAThreadAttribute>]
//do
//    Application().Run(ShowMyFace()) |> ignore



