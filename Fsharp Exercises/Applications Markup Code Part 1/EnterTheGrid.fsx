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
    
    win.Title <- "Enter The Grid"
    win.MinWidth <- 300.0
    win.SizeToContent <- SizeToContent.WidthAndHeight

    let stack = StackPanel()
    win.Content <- stack

    let grid1 = Grid()
    grid1.Margin <- Thickness 5.0
    stack.Children.Add grid1 |> ignore

    for i=0 to 4 do
        let rowdef = RowDefinition()
        rowdef.Height <- GridLength.Auto
        grid1.RowDefinitions.Add rowdef

    ColumnDefinition()
    |> fun coldef ->
        coldef.Width <- GridLength.Auto
        grid1.ColumnDefinitions.Add coldef

    ColumnDefinition()
    |> fun coldef ->
        coldef.Width <- GridLength(100.0,GridUnitType.Star)
        grid1.ColumnDefinitions.Add coldef 

    [|
    "_First Name:"; "_Last Name:"; "_Social Security Number:"; 
    "_Credit Card Number"; "_Other personal stuff" |]
    |> Array.iteri (fun i x -> 
        let lbl = Label()
        lbl.Content <- x
        lbl.VerticalContentAlignment <- VerticalAlignment.Center
        grid1.Children.Add(lbl) |> ignore
        Grid.SetRow(lbl,i)
        Grid.SetColumn(lbl,0)

        let txtbox = TextBox()
        txtbox.Margin <- Thickness 5.0
        grid1.Children.Add txtbox |> ignore
        Grid.SetRow(txtbox,i)
        Grid.SetColumn(txtbox,1)
        )

    let grid2 = Grid()
    grid2.Margin <- Thickness 10.0
    stack.Children.Add grid2 |> ignore
    grid2.ColumnDefinitions.Add <| ColumnDefinition() // The default is star
    grid2.ColumnDefinitions.Add <| ColumnDefinition()

    Button()
    |> fun btn ->
        btn.Content <- "Submit"
        btn.HorizontalAlignment <- HorizontalAlignment.Center
        btn.IsDefault <- true
        btn.Click.Add(fun _ -> win.Close())
        grid2.Children.Add btn |> ignore

    Button()
    |> fun btn ->
        btn.Content <- "Cancel"
        btn.HorizontalAlignment <- HorizontalAlignment.Center
        btn.IsCancel <- true
        btn.Click.Add(fun _ -> win.Close())
        grid2.Children.Add btn |> ignore
        Grid.SetColumn(btn,1)

    (stack.Children.[0] :?> Panel).Children.[0].Focus() |> ignore
    win.Show()
    //Application().Run(win) |> ignore
with e -> MessageBox.Show(e.Message,"Exception Error") |> ignore
        
