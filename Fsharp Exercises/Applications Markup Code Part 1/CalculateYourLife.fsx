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
    
    win.Title <- "Calculate Your Life"
    win.SizeToContent <- SizeToContent.WidthAndHeight
    win.ResizeMode <- ResizeMode.CanMinimize

    let grid = Grid()
    win.Content <- grid

    for i=0 to 2 do
        let rowdef = RowDefinition()
        rowdef.Height <- GridLength.Auto
        grid.RowDefinitions.Add rowdef |> ignore

    for i=0 to 1 do
        let coldef = ColumnDefinition()
        coldef.Width <- GridLength.Auto
        grid.ColumnDefinitions.Add coldef |> ignore

    let lbl = Label()
    lbl.Content <- "Begin date: "
    grid.Children.Add(lbl) |> ignore
    Grid.SetRow(lbl,0)
    Grid.SetColumn(lbl,0)

    let txtboxBegin = TextBox()
    txtboxBegin.Text <- DateTime(1980,1,1).ToShortDateString()
    grid.Children.Add(txtboxBegin) |> ignore
    Grid.SetRow(txtboxBegin,0)
    Grid.SetColumn(txtboxBegin,1)
    
    let lbl = Label()
    lbl.Content <- "End date: "
    grid.Children.Add(lbl) |> ignore
    Grid.SetRow(lbl,1)
    Grid.SetColumn(lbl,0)

    let txtboxEnd = TextBox()
    grid.Children.Add(txtboxEnd) |> ignore
    Grid.SetRow(txtboxEnd,1)
    Grid.SetColumn(txtboxEnd,1)    

    let lbl = Label()
    lbl.Content <- "Life years: "
    grid.Children.Add(lbl) |> ignore
    Grid.SetRow(lbl,2)
    Grid.SetColumn(lbl,0)

    let lblLifeYears = Label()
    grid.Children.Add(lblLifeYears) |> ignore
    Grid.SetRow(lblLifeYears,2)
    Grid.SetColumn(lblLifeYears,1)

    let thick = Thickness 5.0
    grid.Margin <- thick

    for x in grid.Children do
        (x :?> Control).Margin <- thick

    txtboxBegin.Focus() |> ignore
    txtboxEnd.Text <- DateTime.Now.ToShortDateString()

    let onChanged args =
        let dtBegin, dtEnd = ref <| DateTime(), ref <| DateTime()
        if DateTime.TryParse(txtboxBegin.Text,dtBegin) && DateTime.TryParse(txtboxEnd.Text,dtEnd) then
            let dtBegin, dtEnd = !dtBegin, !dtEnd
            let mutable iYears = dtEnd.Year - dtBegin.Year
            let mutable iMonths = dtEnd.Month - dtBegin.Month
            let mutable iDays = dtEnd.Day - dtBegin.Day

            if iDays < 0 then
                iDays <- iDays + DateTime.DaysInMonth(dtEnd.Year,1 + (dtEnd.Month+10) % 12)
                iMonths <- iMonths-1
            if iMonths < 0 then
                iMonths <- iMonths+12
                iYears <- iYears-1

            lblLifeYears.Content <- 
                let adds x = if x > 1 then "s" else ""
                sprintf "%i year%s, %i month%s, %i day%s" iYears (adds iYears) iMonths (adds iMonths) iDays (adds iDays)
        else
            lblLifeYears.Content <- ""

    txtboxBegin.TextChanged.Add onChanged
    txtboxEnd.TextChanged.Add onChanged

    win.Show()
    Application().Run(win) |> ignore
with e -> MessageBox.Show(e.Message,"Exception Error") |> ignore
        





