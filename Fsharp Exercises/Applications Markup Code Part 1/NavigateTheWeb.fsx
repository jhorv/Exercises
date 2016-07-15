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
open System.Windows.Controls.Primitives
open System.Windows.Media
open System.Windows.Media.Imaging
open System.Windows.Shapes
open System.Windows.Documents

type UriDialog() as t =
    inherit Window()

    let txtbox = TextBox()

    do 
        t.MinHeight <- 300.0
        t.MinWidth <- 300.0
        t.WindowStartupLocation <- WindowStartupLocation.CenterScreen
        t.SizeToContent <- SizeToContent.WidthAndHeight

        t.Title <- "Enter the URI"
        
        t.WindowStyle <- WindowStyle.ToolWindow

        txtbox.Margin <- Thickness(48.0)
        t.Content <- txtbox
        txtbox.Focus() |> ignore

    member t.Text 
        with get() = txtbox.Text
        and set v = txtbox.Text <- v; txtbox.SelectionStart <- txtbox.Text.Length

    override t.OnKeyDown args =
        match args.Key with
        | Key.Enter -> t.Close()
        | _ -> ()

type NavigateTheWeb() as t =
    inherit Window()

    let frm = Frame()

    do 
        t.MinHeight <- 400.0
        t.MinWidth <- 400.0
        t.WindowStartupLocation <- WindowStartupLocation.CenterScreen
        t.SizeToContent <- SizeToContent.WidthAndHeight

        t.Title <- "Navigate The Web"
        
        t.Content <- frm
        t.Loaded.Add(fun args ->
            let dlg = UriDialog()
            dlg.Owner <- t
            dlg.Text <- "http://"
            dlg.ShowDialog() |> ignore
            try frm.Source <- Uri dlg.Text
            with e -> MessageBox.Show(e.Message,t.Title) |> ignore
            )


try NavigateTheWeb().Show()
with e -> MessageBox.Show(e.Message,"Exception Error") |> ignore

//[<STAThreadAttribute>]
//do
//    try Application().Run(NavigateTheWeb()) |> ignore
//    with e -> MessageBox.Show(e.Message,"Exception Error") |> ignore



