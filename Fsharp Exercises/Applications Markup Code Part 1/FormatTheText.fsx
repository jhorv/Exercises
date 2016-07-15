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

type ShowMyFace() as t =
    inherit Window()

    do 
        t.Height <- 300.0
        t.Width <- 300.0
        t.WindowStartupLocation <- WindowStartupLocation.CenterScreen
        //t.SizeToContent <- SizeToContent.WidthAndHeight

        let txt = TextBlock()
        txt.FontSize <- 32.0
        txt.Inlines.Add("This is some ")
        txt.Inlines.Add(Italic(Run("italic")))
        txt.Inlines.Add(" text and this is some ")
        txt.Inlines.Add(Bold(Run("bold")))
        txt.Inlines.Add(" text, and let's cap it off with some ")
        txt.Inlines.Add(Italic(Bold(Run("bold italic"))))
        txt.Inlines.Add(" text.")
        txt.TextWrapping <- TextWrapping.Wrap

        t.Content <- txt
        t.Foreground <- Brushes.CornflowerBlue


ShowMyFace().Show()
//[<STAThreadAttribute>]
//do
//    Application().Run(ShowMyFace()) |> ignore







