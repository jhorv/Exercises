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

    let brush = 
        LinearGradientBrush(Colors.Blue,Colors.Red,0.0)
        |> fun x -> x.MappingMode <- BrushMappingMode.Absolute; x

    do 
        t.Height <- 400.0
        t.Width <- 400.0
        t.WindowStartupLocation <- WindowStartupLocation.CenterScreen
        t.Background <- brush

        let sizeChanged args =
            let width = t.ActualWidth - 2.0 * SystemParameters.ResizeFrameVerticalBorderWidth
            let height = t.ActualHeight - 2.0 * SystemParameters.ResizeFrameHorizontalBorderHeight - SystemParameters.CaptionHeight

            let ptCenter = new Point(width / 2.0, height / 2.0)
            let vectDiag = new Vector(width,-height);
            let mutable vectPerp = new Vector(vectDiag.Y, -vectDiag.X);

            vectPerp.Normalize();
            vectPerp <- vectPerp * (width * height / vectDiag.Length);

            brush.StartPoint <- ptCenter + vectPerp;
            brush.EndPoint <- ptCenter - vectPerp;

        t.SizeChanged.Add sizeChanged


VaryTheBackground().Show()


