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
open System.Diagnostics
open System.ComponentModel
open System.Windows
open System.Windows.Input
open System.Windows.Controls
open System.Windows.Controls.Primitives
open System.Windows.Media
open System.Windows.Media.Imaging
open System.Windows.Shapes
open System.Windows.Documents

[<STAThreadAttribute>]
try 
    let win = Window()
    win.Title <- "Paint The Button"

    let btn = Button()
    btn.HorizontalAlignment <- HorizontalAlignment.Center
    btn.VerticalAlignment <- VerticalAlignment.Center
    win.Content <- btn

    let canv = Canvas()
    canv.Width <- 144.0
    canv.Height <- 144.0
    btn.Content <- canv

    let rect = Rectangle()
    rect.Width <- canv.Width
    rect.Height <- canv.Height
    rect.RadiusX <- 24.0
    rect.RadiusY <- 24.0
    rect.Fill <- Brushes.Blue
    canv.Children.Add rect |> ignore
    Canvas.SetLeft(rect, 0.0)
    Canvas.SetTop(rect, 0.0)

    let poly = Polygon()
    poly.Fill <- Brushes.Yellow
    poly.Points <- PointCollection()

    for i=0 to 4 do
        let angle = (float i) * 4.0 * Math.PI / 5.0
        poly.Points.Add <| Point(48.0 * sin angle, -48.0 * cos angle)

    canv.Children.Add poly |> ignore
    Canvas.SetLeft(poly,canv.Width / 2.0)
    Canvas.SetTop(poly,canv.Height / 2.0)
    
    win.Show()
    //Application().Run(win) |> ignore
with e -> MessageBox.Show(e.Message,"Exception Error") |> ignore
        

