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
open System.Windows.Threading

[<STAThreadAttribute>]
try 
    let win = Window()
    win.Title <- "Set Font Size Property"
    win.SizeToContent <- SizeToContent.WidthAndHeight
    win.ResizeMode <- ResizeMode.CanMinimize
    win.FontSize <- 16.0
    

    let grid = Grid()
    win.Content <- grid

    for i=0 to 1 do
        let rowdef = RowDefinition()
        rowdef.Height <- GridLength.Auto
        grid.RowDefinitions.Add rowdef

    let fntsizes = [|8.0;16.0;32.0|]
    fntsizes
    |> Array.iter (fun _ ->
        let coldef = ColumnDefinition()
        coldef.Width <- GridLength.Auto
        grid.ColumnDefinitions.Add coldef
        )

    fntsizes
    |> Array.iteri (fun i fntsize ->
        Button()
        |> fun btn ->
            btn.Content <- TextBlock(Run(sprintf "Set window FontSize to %.0f" fntsize))
            btn.Tag <- fntsize
            btn.HorizontalAlignment <- HorizontalAlignment.Center
            btn.VerticalAlignment <- VerticalAlignment.Center
            btn.Click.Add (fun args -> win.FontSize <- btn.Tag :?> float)
            grid.Children.Add btn |> ignore
            Grid.SetRow(btn,0)
            Grid.SetColumn(btn,i)

        Button()
        |> fun btn ->
            btn.Content <- TextBlock(Run(sprintf "Set button FontSize to %.0f" fntsize))
            btn.Tag <- fntsize
            btn.HorizontalAlignment <- HorizontalAlignment.Center
            btn.VerticalAlignment <- VerticalAlignment.Center
            btn.Click.Add (fun args -> btn.FontSize <- btn.Tag :?> float)
            grid.Children.Add btn |> ignore
            Grid.SetRow(btn,1)
            Grid.SetColumn(btn,i)
        )

    win.Show()
    //Application().Run(win) |> ignore
with e -> MessageBox.Show(e.Message,"Exception Error") |> ignore
        



