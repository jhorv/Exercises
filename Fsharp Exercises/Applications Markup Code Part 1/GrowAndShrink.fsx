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

type GrowAndShrink() as t =
    inherit Window()

    do 
        t.Height <- 200.0
        t.Width <- 200.0
        t.WindowStartupLocation <- WindowStartupLocation.CenterScreen

    override t.OnKeyDown(args) =
        base.OnKeyDown args

        match args.Key with
        | Key.Up ->
            t.Left <- t.Left-0.05 * t.Width
            t.Top <- t.Top-0.05 * t.Height
            t.Width <- t.Width * 1.1
            t.Height <- t.Height * 1.1
        | Key.Down ->
            t.Left <- t.Left+0.05 * t.Width
            t.Top <- t.Top+0.05 * t.Height
            t.Width <- t.Width / 1.1
            t.Height <- t.Height / 1.1
        | _ -> ()

[<STAThreadAttribute>]
do
    Application().Run(GrowAndShrink()) |> ignore



