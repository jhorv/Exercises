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

type TypeYourTitle() as t =
    inherit Window()

    do 
        t.Height <- 200.0
        t.Width <- 200.0
        t.WindowStartupLocation <- WindowStartupLocation.CenterScreen

    override t.OnTextInput(args) =
        base.OnTextInput args

        MessageBox.Show(args.Text) |> ignore

        if args.Text = "\b" && t.Title.Length > 0 then
            t.Title <- t.Title.Substring(0,t.Title.Length-1)
        elif args.Text.Length > 0 && (not <| Char.IsControl(args.Text.[0])) then
            t.Title <- t.Title+args.Text
            
[<STAThreadAttribute>]
do
    Application().Run(TypeYourTitle()) |> ignore




