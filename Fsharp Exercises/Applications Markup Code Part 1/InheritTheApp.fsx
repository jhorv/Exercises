#if INTERACTIVE
#r "PresentationCore.dll"
#r "PresentationFramework.dll"
#r "System.Xaml.dll"
#r "WindowsBase.dll"
//#r @"../packages/FsXaml.Wpf.2.1.0\lib\net45\FsXaml.Wpf.dll"
//#r @"../packages/FsXaml.Wpf.2.1.0\lib\net45\FsXaml.Wpf.TypeProvider.dll"
#endif

open System
open System.Windows
open System.Windows.Input
open System.Windows.Controls

type InheritTheApp() =
    inherit Application()

    override t.OnStartup(args) =
        base.OnStartup(args)

        let win = Window()
        win.Title <- "Inherit The App"
        win.Show()

    override t.OnExit(args) =
        base.OnExit(args)

        //let res = 
        MessageBox.Show("Do you want to save your data?","Question",MessageBoxButton.YesNoCancel,MessageBoxImage.Question,MessageBoxResult.Yes)
        |> ignore
        //MessageBox.Show("Hello") |> ignore

        //args.Cancel <- (res = MessageBoxResult.Yes)

[<STAThreadAttribute>]
do
    let app = new InheritTheApp()
    app.Run() |> ignore

