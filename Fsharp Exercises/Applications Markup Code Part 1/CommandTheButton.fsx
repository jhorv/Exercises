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

type CommandTheButton() as t =
    inherit Window()

    do 
        t.MinHeight <- 300.0
        t.MinWidth <- 300.0
        t.WindowStartupLocation <- WindowStartupLocation.CenterScreen
        t.SizeToContent <- SizeToContent.WidthAndHeight

        let btn = Button()
        t.Content <- btn

        btn.HorizontalContentAlignment <- HorizontalAlignment.Center
        btn.VerticalContentAlignment <- VerticalAlignment.Center
        btn.Command <- ApplicationCommands.Paste
        btn.Content <- ApplicationCommands.Paste.Text

        t.CommandBindings.Add(
            CommandBinding(
                ApplicationCommands.Paste,
                (fun sender args -> t.Title <- Clipboard.GetText()),
                fun sender args -> args.CanExecute <- Clipboard.ContainsText()
                ))
        |> ignore

    override t.OnMouseDown args=
        base.OnMouseDown args
        t.Title <- "Command The Button"
        
CommandTheButton().Show()
//[<STAThreadAttribute>]
//do
//    Application().Run(CommandTheButton()) |> ignore


