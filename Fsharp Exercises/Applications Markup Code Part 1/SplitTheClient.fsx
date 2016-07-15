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
    
    win.Title <- "Split The Client"

    let grid1 = Grid()
    grid1.ColumnDefinitions.Add <| ColumnDefinition()
    grid1.ColumnDefinitions.Add <| ColumnDefinition()
    grid1.ColumnDefinitions.Add <| ColumnDefinition()
    grid1.ColumnDefinitions.[1].Width <- GridLength.Auto
    win.Content <- grid1

    Button()
    |> fun btn ->
        btn.Content <- "Button No. 1"
        grid1.Children.Add btn |> ignore
        Grid.SetRow(btn,0)
        Grid.SetColumn(btn,0)

    GridSplitter()
    |> fun split ->
        split.ShowsPreview <- true
        split.HorizontalAlignment <- HorizontalAlignment.Center
        split.VerticalAlignment <- VerticalAlignment.Stretch
        split.Width <- 6.0
        grid1.Children.Add split |> ignore
        Grid.SetRow(split,0)
        Grid.SetColumn(split,1)

    let grid2 = Grid()
    grid2.RowDefinitions.Add <| RowDefinition()
    grid2.RowDefinitions.Add <| RowDefinition()
    grid2.RowDefinitions.Add <| RowDefinition()
    grid2.RowDefinitions.[1].Height <- GridLength.Auto
    grid1.Children.Add grid2 |> ignore
    Grid.SetRow(grid2,0)
    Grid.SetColumn(grid2,2)

    Button()
    |> fun btn ->
        btn.Content <- "Button No. 2"
        grid2.Children.Add btn |> ignore
        Grid.SetRow(btn,0)
        Grid.SetColumn(btn,0)


    GridSplitter()
    |> fun split ->
        split.ShowsPreview <- true
        split.HorizontalAlignment <- HorizontalAlignment.Stretch
        split.VerticalAlignment <- VerticalAlignment.Center
        split.Height <- 6.0
        grid2.Children.Add split |> ignore
        Grid.SetRow(split,1)
        Grid.SetColumn(split,0)

    Button()
    |> fun btn ->
        btn.Content <- "Button No. 3"
        grid2.Children.Add btn |> ignore
        Grid.SetRow(btn,2)
        Grid.SetColumn(btn,0)

    win.Show()
    //Application().Run(win) |> ignore
with e -> MessageBox.Show(e.Message,"Exception Error") |> ignore
        

