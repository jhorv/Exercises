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

[<STAThreadAttribute>]
try 
    let dock = DockPanel()
    let win = Window(Title = "Check the Color", Content = dock)

    let menu = Menu() |> DockPanel.DockTop |> addChildren dock
    let text = TextBlock(Text=win.Title, FontSize=32.0, TextAlignment=TextAlignment.Center, 
                         Background=SystemColors.WindowBrush, Foreground=SystemColors.WindowTextBrush) |> addChildren dock
    let itemStyle = MenuItem(Header = "_Color") |> addItems menu

    let fillWithColors itemParent handler =
        typeof<Colors>.GetProperties()
        |> Array.iter(fun prop ->
            let clr = prop.GetValue(null) :?> Color
        
            let mutable i = 0
            let check x = if x = 0uy || x = 255uy then i <- i+1
            check clr.R; check clr.G; check clr.B
        
            if clr.A = 255uy && i > 1 then
                let rect = Rectangle(Fill=SolidColorBrush clr,Width=24.0,Height=12.0)
                MenuItem(Header="_"+prop.Name, Tag=clr, Icon=rect) 
                |> fun x -> x.Click.Add (handler clr); x
                |> addItems itemParent
                |> ignore
            )

    let itemForeground = MenuItem(Header = "_Foreground") |> addItems itemStyle
    let itemBackground = MenuItem(Header = "_Background") |> addItems itemStyle

    fillWithColors itemForeground (fun clr _ -> text.Foreground <- clr |> SolidColorBrush)
    fillWithColors itemBackground (fun clr _ -> text.Background <- clr |> SolidColorBrush)

    // This could be done more elegantly by setting the Tag to a closure that compares
    // colors.
    itemForeground.SubmenuOpened.Add <| fun _ ->
        for item in itemForeground.Items do
            let item = item :?> MenuItem
            let br = text.Foreground :?> SolidColorBrush
            item.IsChecked <- br.Color = (item.Tag :?> Color)

    itemBackground.SubmenuOpened.Add <| fun _ ->
        for item in itemBackground.Items do
            let item = item :?> MenuItem
            let br = text.Background :?> SolidColorBrush
            item.IsChecked <- br.Color = (item.Tag :?> Color)

    win.Show()
    //Application().Run(win) |> ignore
with e -> MessageBox.Show(e.Message,"Exception Error") |> ignore

