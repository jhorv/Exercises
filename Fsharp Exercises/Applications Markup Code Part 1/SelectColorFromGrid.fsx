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
open System.Text
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

type ColorGridBox() as t =
    inherit ListBox()

    static let strColorsNames =
        [|
            [|"Black"; "Brown"; "DarkGreen"; "MidnightBlue"; "Navy"; "DarkBlue"; "Indigo"; "DimGray"|]
            [|"DarkRed"; "OrangeRed"; "Olive"; "Green"; "Teal"; "Blue"; "SlateGray"; "Gray"|]
            [|"Red"; "Orange"; "YellowGreen"; "SeaGreen"; "Aqua"; "LightBlue"; "Violet"; "DarkGray"|]
            [|"Pink"; "Gold"; "Yellow"; "Lime"; "Turquoise"; "SkyBlue"; "Plum"; "LightGray"|]
            [|"LightPink"; "Tan"; "LightYellow"; "LightGreen"; "LightCyan"; "LightSkyBlue"; "Lavender"; "White"|]
        |]
        |> Array.concat

    do
        let factoryUnigrid = FrameworkElementFactory(typeof<UniformGrid>)
        factoryUnigrid.SetValue(UniformGrid.ColumnsProperty,8)
        t.ItemsPanel <- ItemsPanelTemplate(factoryUnigrid)

        for strColor in strColorsNames do
            // Create Rectangle and add to ListBox.
            let rect = Rectangle()
            rect.Width <- 12.0
            rect.Height <- 12.0
            rect.Margin <- Thickness 4.0
            rect.Fill <- typeof<Brushes>.GetProperty(strColor).GetValue(null) :?> Brush
            t.Items.Add rect |> ignore

            // Create ToolTip for Rectangle.
            let tip = ToolTip()
            tip.Content <- strColor
            rect.ToolTip <- tip

        // Indicate that SelectedValue is Fill property of Rectangle item.
        t.SelectedValuePath <- "Fill"


[<STAThreadAttribute>]
try 
    let win = Window()
    win.Title <- "Select Color from Grid"
    win.SizeToContent <- SizeToContent.WidthAndHeight

    let stack = StackPanel()
    stack.Orientation <- Orientation.Horizontal
    win.Content <- stack

    Button()
    |> fun btn ->
        btn.Content <- "Do-nothing button\nto test tabbing"
        btn.Margin <- Thickness 24.0
        btn.HorizontalAlignment <- HorizontalAlignment.Center
        btn.VerticalAlignment <- VerticalAlignment.Center
        stack.Children.Add btn |> ignore

    let clrGrid = ColorGridBox()
    clrGrid.Margin <- Thickness 24.0
    clrGrid.HorizontalAlignment <- HorizontalAlignment.Center
    clrGrid.VerticalAlignment <- VerticalAlignment.Center
    stack.Children.Add clrGrid |> ignore

    clrGrid.SetBinding(ColorGridBox.SelectedValueProperty,"Background") |> ignore
    clrGrid.DataContext <- win

    Button()
    |> fun btn ->
        btn.Content <- "Do-nothing button\nto test tabbing"
        btn.Margin <- Thickness 24.0
        btn.HorizontalAlignment <- HorizontalAlignment.Center
        btn.VerticalAlignment <- VerticalAlignment.Center
        stack.Children.Add btn |> ignore

    win.Show()
    Application().Run(win) |> ignore
with e -> MessageBox.Show(e.Message,"Exception Error") |> ignore



