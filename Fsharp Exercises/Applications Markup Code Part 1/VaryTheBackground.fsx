#if INTERACTIVE
#r "PresentationCore.dll"
#r "PresentationFramework.dll"
#r "System.Xaml.dll"
#r "WindowsBase.dll"
#r "UIAutomationTypes.dll"
#endif

open System
open System.Windows
open System.Windows.Input
open System.Windows.Controls
open System.Windows.Media

type VaryTheBackground() as t =
    inherit Window()

    let brush = SolidColorBrush(Colors.Black)

    do 
        t.Height <- 400.0
        t.Width <- 400.0
        t.WindowStartupLocation <- WindowStartupLocation.CenterScreen
        t.Background <- brush

    override t.OnMouseMove(args) =
        base.OnMouseMove args

        let width = t.ActualWidth - 2.0 * SystemParameters.ResizeFrameVerticalBorderWidth
        //printfn "ActualWidth = %f, ActualHeight = %f" t.ActualWidth t.ActualHeight
        //printfn "ResizeFrameVerticalBorderWidth = %f, ResizeFrameHorizontalBorderHeight = %f, CaptionHeight = %f" SystemParameters.ResizeFrameVerticalBorderWidth SystemParameters.ResizeFrameHorizontalBorderHeight SystemParameters.CaptionHeight
        let height = t.ActualHeight - 2.0 * SystemParameters.ResizeFrameHorizontalBorderHeight - SystemParameters.CaptionHeight

        let ptMouse = args.GetPosition(t)
        let ptCenter = Point(width/2.0, height/2.0)
        let vectMouse = ptMouse - ptCenter
        let angle = atan2 vectMouse.Y vectMouse.X
        let vectEclipse = Vector(width / 2.0 * (cos angle), height / 2.0 * (sin angle))
        printfn "vectMouse.Length = %A, vectEclipse.Length = %A" vectMouse.Length vectEclipse.Length
        let byLevel = 255.0 * (1.0 - min 1.0 (vectMouse.Length / vectEclipse.Length)) |> byte
        brush.Color <- Color.FromRgb(byLevel,byLevel,byLevel)

VaryTheBackground().Show()

