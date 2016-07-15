// Fourth exercise from Petzold's book.

#if INTERACTIVE
#r "PresentationCore.dll"
#r "PresentationFramework.dll"
#r "System.Xaml.dll"
#r "WindowsBase.dll"
#endif

open System
open System.Windows
open System.Windows.Input
open System.Windows.Controls

type WindowParty() =
    inherit Application()

    override t.OnStartup(args) =
        base.OnStartup(args)
        
        t.ShutdownMode <- ShutdownMode.OnMainWindowClose

        let winMain = Window()
        winMain.Title <- "Main Window"
        winMain.Show()

        winMain.MouseDown.Add (fun x ->
            let win = Window()
            //win.Owner <- winMain
            win.Title <- "Modal Dialog"
            win.ShowInTaskbar <- false
            win.ShowDialog() |> ignore
            )
        for i=1 to 2 do
            let win = Window()
            win.Owner <- winMain
            win.Title <- sprintf "Extra Window No.%i" i
            win.ShowInTaskbar <- false
            win.Show() |> ignore

[<STAThreadAttribute>]
do
    let app = new WindowParty()
    app.Run() |> ignore


