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

type SimpleElipse() =
    inherit FrameworkElement()

    override t.OnRender dc =
        let brush_size = 24.0
        dc.DrawEllipse(Brushes.Blue,Pen(Brushes.Red,brush_size),Point(t.RenderSize.Width/2.0,t.RenderSize.Height/2.0),t.RenderSize.Width/2.0-brush_size/2.0,t.RenderSize.Height/2.0-brush_size/2.0)


type RenderTheGraphics() as t =
    inherit Window()

    do 
        t.MinHeight <- 300.0
        t.MinWidth <- 300.0
        t.WindowStartupLocation <- WindowStartupLocation.CenterScreen
        t.SizeToContent <- SizeToContent.WidthAndHeight

        t.Content <- SimpleElipse()

RenderTheGraphics().Show()
//[<STAThreadAttribute>]
//do
//    Application().Run(ShowMyFace()) |> ignore



