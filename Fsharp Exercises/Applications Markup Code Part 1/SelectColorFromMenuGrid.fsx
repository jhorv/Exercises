#if INTERACTIVE
#r "PresentationCore.dll"
#r "PresentationFramework.dll"
#r "System.Xaml.dll"
#r "WindowsBase.dll"
#r "UIAutomationTypes.dll"
#load "Bindings.fs"
#endif

open Bindings
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
open System.Windows.Data

type ColorGridBox() as t =
    inherit ListBox(SelectedValuePath = "Fill")

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
        let factoryUnigrid = 
            FrameworkElementFactory(typeof<UniformGrid>)
            |> setDP UniformGrid.ColumnsProperty 8
        t.ItemsPanel <- ItemsPanelTemplate(factoryUnigrid)

        for strColor in strColorsNames do
            // Create ToolTip for Rectangle.
            let tip = ToolTip(Content = strColor)

            // Create Rectangle and add to ListBox.
            let col = typeof<Brushes>.GetProperty(strColor).GetValue(null) :?> Brush
            Rectangle(Width = 12.0, Height = 12.0, Margin = Thickness 4.0, Fill = col, ToolTip=tip) 
            |> addItems t
            |> ignore


[<STAThreadAttribute>]
try 
    let dock = DockPanel()
    let win = Window(Title = "Select Color from Menu Grid", Content = dock)

    let menu = Menu() |> DockPanel.DockTop |> addChildren dock
    let text = TextBlock(Text=win.Title, FontSize=32.0, TextAlignment=TextAlignment.Center) |> addChildren dock
    let itemColor = MenuItem(Header = "_Color") |> addItems menu

    let itemForeground = MenuItem(Header = "_Foreground") |> addItems itemColor
    
    ColorGridBox(DataContext = win) 
    |> setBind ColorGridBox.SelectedValueProperty "Foreground"
    |> addItems itemForeground
    |> ignore

    let itemBackground = MenuItem(Header = "_Background") |> addItems itemColor
    
    ColorGridBox(DataContext = win) 
    |> setBind ColorGridBox.SelectedValueProperty "Background"
    |> addItems itemBackground
    |> ignore

    win.Show()
    //Application().Run(win) |> ignore
with e -> MessageBox.Show(e.Message,"Exception Error") |> ignore

