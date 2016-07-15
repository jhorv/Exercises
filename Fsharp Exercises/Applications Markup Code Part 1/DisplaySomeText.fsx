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

type DisplaySomeText() as t =
    inherit Window()

    do 
        t.Height <- 400.0
        t.Width <- 400.0
        t.WindowStartupLocation <- WindowStartupLocation.CenterScreen
        t.Content <- DateTime.Now
        t.FontFamily <- FontFamily("Times New Roman")
        t.FontSize <- 26.0
        t.FontWeight <- FontWeights.UltraLight
        t.FontStyle <- FontStyles.Italic
        t.Background <- LinearGradientBrush(Colors.Black,Colors.White,0.0)
        t.Foreground <- LinearGradientBrush(Colors.White,Colors.Black,0.0)
        t.SizeToContent <- SizeToContent.WidthAndHeight
        t.ResizeMode <- ResizeMode.NoResize

        t.BorderBrush <- Brushes.SaddleBrown
        t.BorderThickness <- Thickness(5.0)

DisplaySomeText().Show()


