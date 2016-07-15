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
    
    win.Title <- "Split Nine"

    let grid = Grid()
    win.Content <- grid

    for i=0 to 2 do
        grid.ColumnDefinitions.Add <| ColumnDefinition()
        grid.RowDefinitions.Add <| RowDefinition()

    for x=0 to 2 do
        for y=0 to 2 do
            let btn = Button()
            btn.Content <- sprintf "Row %i and Column %i" y x
            grid.Children.Add btn |> ignore
            Grid.SetColumn(btn,x)
            Grid.SetRow(btn,y)

    let split = GridSplitter()
    split.MinWidth <- 6.0
    split.MinHeight <- 6.0
    grid.Children.Add split |> ignore
    Grid.SetRow(split,0)
    Grid.SetColumn(split,1)

    split.HorizontalAlignment <- HorizontalAlignment.Stretch
    split.VerticalAlignment <- VerticalAlignment.Bottom

    win.Show()
    //Application().Run(win) |> ignore
with e -> MessageBox.Show(e.Message,"Exception Error") |> ignore
        
