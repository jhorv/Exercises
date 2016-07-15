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

try 
    let win = Window()

    win.Title <- "Meet The Dockers"
    let dock = DockPanel()
    win.Content <- dock

    let menu = Menu()
    let item = MenuItem()
    item.Header <- "Menu"
    menu.Items.Add(item) |> ignore

    DockPanel.SetDock(menu,Dock.Top)
    dock.Children.Add(menu) |> ignore

    let toolbar = ToolBar()
    toolbar.Header <- "Toolbar"
    DockPanel.SetDock(toolbar,Dock.Top)
    dock.Children.Add(toolbar) |> ignore

    let statbar = StatusBar()
    let statitem = StatusBarItem()
    statitem.Content <- "Status"
    statbar.Items.Add statitem |> ignore

    DockPanel.SetDock(statbar,Dock.Bottom)
    dock.Children.Add statbar |> ignore

    let lstbox = ListBox()
    lstbox.Margin <- Thickness(0.0,0.0,5.0,0.0) // Splitter
    lstbox.Items.Add "List Box Item" |> ignore

    DockPanel.SetDock(lstbox,Dock.Left)
    dock.Children.Add lstbox |> ignore

    let txtbox = TextBox()
    txtbox.AcceptsReturn <- true

    dock.Children.Add txtbox |> ignore
    txtbox.Focus() |> ignore
    
    win.Show()
with e -> MessageBox.Show(e.Message,"Exception Error") |> ignore

//[<STAThreadAttribute>]
//do
//    try Application().Run(TuneTheRadio()) |> ignore
//    with e -> MessageBox.Show(e.Message,"Exception Error") |> ignore


