#if INTERACTIVE
#r "PresentationCore.dll"
#r "PresentationFramework.dll"
#r "System.Xaml.dll"
#r "WindowsBase.dll"
#r "UIAutomationTypes.dll"
#endif

open System
open System.Reflection
open System.Windows
open System.Windows.Input
open System.Windows.Controls
open System.Windows.Media

type GradiateTheBrush () as t =
    inherit Window()

    do 
        t.Height <- 400.0
        t.Width <- 400.0
        t.WindowStartupLocation <- WindowStartupLocation.CenterScreen
        let brush = LinearGradientBrush(Colors.Blue,Colors.Orange,Point(0.0,0.0),Point(0.25,0.25))
        brush.SpreadMethod <- GradientSpreadMethod.Repeat
        t.Background <- brush

GradiateTheBrush().Show()



