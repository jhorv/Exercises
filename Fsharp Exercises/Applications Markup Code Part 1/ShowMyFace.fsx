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

type ShowMyFace() as t =
    inherit Window()

    do 
        t.Height <- 200.0
        t.Width <- 200.0
        t.WindowStartupLocation <- WindowStartupLocation.CenterScreen
        t.SizeToContent <- SizeToContent.WidthAndHeight

        let img = 
            let uri = Uri(Path.Combine("E:\Downloads","1467638105256.jpg"))
            let bitmap = BitmapImage(uri)
            Image() |> fun x -> x.Source <- bitmap; x
        t.Content <- img

        img.LayoutTransform <- RotateTransform(45.0)
            

ShowMyFace().Show()
//[<STAThreadAttribute>]
//do
//    Application().Run(ShowMyFace()) |> ignore





