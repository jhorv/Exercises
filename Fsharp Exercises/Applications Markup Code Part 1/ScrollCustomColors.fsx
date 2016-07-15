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
    
    win.Title <- "Scroll Custom Colors"
    win.Width <- 500.0
    win.Height <- 300.0
    let gridMain = Grid()
    win.Content <- gridMain

    ColumnDefinition()
    |> fun coldef ->
        coldef.Width <- GridLength(200.0,GridUnitType.Pixel)
        gridMain.ColumnDefinitions.Add coldef

    ColumnDefinition()
    |> fun coldef ->
        coldef.Width <- GridLength.Auto
        gridMain.ColumnDefinitions.Add coldef
        GridSplitter()
        |> fun split ->
            split.HorizontalAlignment <- HorizontalAlignment.Center
            split.VerticalAlignment <- VerticalAlignment.Stretch
            split.Width <- 6.0
            gridMain.Children.Add split |> ignore
            Grid.SetRow(split,0)
            Grid.SetColumn(split,1)

    ColumnDefinition()
    |> fun coldef ->
        coldef.Width <- GridLength(100.0,GridUnitType.Star)
        gridMain.ColumnDefinitions.Add coldef


    let pnl = StackPanel()
    pnl.Background <- SystemColors.WindowColor |> SolidColorBrush
    gridMain.Children.Add pnl |> ignore
    Grid.SetRow(pnl,0)
    Grid.SetColumn(pnl,2)

    let grid = Grid()
    gridMain.Children.Add grid |> ignore

    RowDefinition()
    |> fun rowdef ->
        rowdef.Height <- GridLength.Auto
        grid.RowDefinitions.Add rowdef

    RowDefinition()
    |> fun rowdef ->
        rowdef.Height <- GridLength(100.0,GridUnitType.Star)
        grid.RowDefinitions.Add rowdef

    RowDefinition()
    |> fun rowdef ->
        rowdef.Height <- GridLength.Auto
        grid.RowDefinitions.Add rowdef

    for i=0 to 2 do
        ColumnDefinition()
        |> fun coldef ->
            coldef.Width <- GridLength(33.0,GridUnitType.Star)
            grid.ColumnDefinitions.Add coldef

    
    let args = 
        let clr = pnl.Background :?> SolidColorBrush |> fun x -> x.Color
        [|"Red",clr.R;"Green",clr.G;"Blue",clr.B|]
        |> Array.map (fun (colorStr,colorByte) -> colorStr,float colorByte,ScrollBar(),TextBlock()) 
    let scrollbars = args |> Array.map (fun (_,_,s,_) -> s)
    args
    |> Array.iteri(fun i (colorStr,color,scrollbar,txtblk) ->
        let lbl = Label()
        lbl.Content <- colorStr
        lbl.HorizontalAlignment <- HorizontalAlignment.Center
        grid.Children.Add lbl |> ignore
        Grid.SetRow(lbl,0)
        Grid.SetColumn(lbl,i)

        scrollbar.Focusable <- true
        scrollbar.Orientation <- Orientation.Vertical
        scrollbar.Minimum <- 0.0
        scrollbar.Maximum <- 255.0
        scrollbar.SmallChange <- 1.0
        scrollbar.LargeChange <- 16.0
        scrollbar.Value <- color

        scrollbar.ValueChanged.Add (fun _ ->
            txtblk.Text <- sprintf "%i" (scrollbar.Value |> round |> int)
            pnl.Background <- 
                let [|r;g;b|] = scrollbars |> Array.map (fun x -> x.Value |> byte)
                SolidColorBrush(Color.FromRgb(r,g,b))
            )
        grid.Children.Add scrollbar |> ignore
        Grid.SetRow(scrollbar,1)
        Grid.SetColumn(scrollbar,i)

        txtblk.TextAlignment <- TextAlignment.Center
        txtblk.HorizontalAlignment <- HorizontalAlignment.Center
        txtblk.Margin <- Thickness 5.0
        grid.Children.Add txtblk |> ignore
        Grid.SetRow(txtblk,2)
        Grid.SetColumn(txtblk,i)
        )

    
    win.Show()
    //Application().Run(win) |> ignore
with e -> MessageBox.Show(e.Message,"Exception Error") |> ignore
        


