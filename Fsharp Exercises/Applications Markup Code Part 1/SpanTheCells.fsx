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
    
    win.Title <- "Span The Cells"
    win.SizeToContent <- SizeToContent.WidthAndHeight

    let grid = Grid()
    grid.Margin <- Thickness 5.0

    win.Content <- grid

    for i=0 to 5 do
        let rowdef = RowDefinition()
        rowdef.Height <- GridLength.Auto
        grid.RowDefinitions.Add rowdef

    for i=0 to 3 do
        let coldef = ColumnDefinition()
        coldef.Width <- if i = 0 then GridLength.Auto else GridLength(100.0,GridUnitType.Star)
        grid.ColumnDefinitions.Add coldef

    [|
    "_First Name:"; "_Last Name:"; "_Social Security Number:"; 
    "_Credit Card Number"; "_Other personal stuff" |]
    |> Array.iteri (fun i x -> 
        let lbl = Label()
        lbl.Content <- x
        lbl.VerticalContentAlignment <- VerticalAlignment.Center
        lbl.HorizontalAlignment <- HorizontalAlignment.Left
        grid.Children.Add lbl |> ignore
        Grid.SetRow(lbl,i)
        Grid.SetColumn(lbl,0)

        let txtbox = TextBox()
        txtbox.Margin <- Thickness 5.0
        //txtbox.HorizontalAlignment <- HorizontalAlignment.Stretch
        grid.Children.Add txtbox |> ignore
        Grid.SetRow(txtbox,i)
        Grid.SetColumn(txtbox,1)
        Grid.SetColumnSpan(txtbox,3)
        )

    Button()
    |> fun btn ->
        btn.Content <- "Submit"
        btn.HorizontalAlignment <- HorizontalAlignment.Center
        btn.IsDefault <- true
        grid.Children.Add btn |> ignore
        Grid.SetRow(btn,5)
        Grid.SetColumn(btn,2)

    Button()
    |> fun btn ->
        btn.Content <- "Cancel"
        btn.HorizontalAlignment <- HorizontalAlignment.Center
        btn.IsCancel <- true
        grid.Children.Add btn |> ignore
        Grid.SetRow(btn,5)
        Grid.SetColumn(btn,3)

    grid.Children.[1].Focus() |> ignore
    win.Show()
    //Application().Run(win) |> ignore
with e -> MessageBox.Show(e.Message,"Exception Error") |> ignore
        

