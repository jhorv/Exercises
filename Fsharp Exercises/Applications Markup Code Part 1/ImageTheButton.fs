// This one needs to be compiled as the exercise is about embedding 
// resources directly into the executable.

module ImageTheButton

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

type ImageTheButton() as t =
    inherit Window()

    do 
        t.MinHeight <- 300.0
        t.MinWidth <- 300.0
        t.WindowStartupLocation <- WindowStartupLocation.CenterScreen
        t.SizeToContent <- SizeToContent.WidthAndHeight

        let uri = Uri "pack://application:,,/Images/cagliostro.jpg"
        let img = 
            let b = BitmapImage uri
            let img = Image()
            img.Source <- b
            img.Stretch <- Stretch.Uniform
            img


        let btn = Button()
        btn.HorizontalContentAlignment <- HorizontalAlignment.Center
        btn.VerticalContentAlignment <- VerticalAlignment.Center
        btn.Content <- img

        t.Content <- btn
        

[<STAThreadAttribute>]
do
    Application().Run(ImageTheButton()) |> ignore





