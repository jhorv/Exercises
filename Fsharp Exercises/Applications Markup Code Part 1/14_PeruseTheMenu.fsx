// There is nothing wrong with the example, but I will make a note to learn those
// WPF Fsharp helpers that I saw in Phil Trelford's talk. The style of coding shown
// in this example has led me to a significant amount of grief in the past due to copy 
// paste errors.

// If I could figure out how those helpers work, I could even apply the technique in
// my autodiff library! This would be a definite gain for me.

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
open System.Windows.Data

[<STAThreadAttribute>]
try 
    let win = Window()
    win.Title <- "Peruse The Menu"

    let dock = DockPanel()
    win.Content <- dock

    let menu = Menu()
    dock.Children.Add menu |> ignore
    DockPanel.SetDock(menu,Dock.Top)

    let text = TextBlock()
    text.Text <- win.Title
    text.FontSize <- 32.0
    text.TextAlignment <- TextAlignment.Center
    dock.Children.Add text |> ignore

    let itemFile = MenuItem()
    itemFile.Header <- "_File"
    menu.Items.Add itemFile |> ignore

    let itemNew = MenuItem()
    itemNew.Header <- "_New"
    itemFile.Items.Add itemNew |> ignore

    let itemOpen = MenuItem()
    itemOpen.Header <- "_Open"
    itemFile.Items.Add itemOpen |> ignore

    let itemSave = MenuItem()
    itemSave.Header <- "_Save"
    itemFile.Items.Add itemSave |> ignore

    [|itemNew;itemOpen;itemSave|]
    |> Array.iter(fun item ->
        let str = item.Header.ToString().Replace("_","")
        item.Click.Add <| fun _ -> MessageBox.Show(sprintf "The %s option has not yet been implemented" str, win.Title) |> ignore)

    let itemExit = MenuItem()
    itemExit.Header <- "E_xit"
    itemExit.Click.Add(fun _ -> win.Close())
    itemFile.Items.Add itemExit |> ignore

    let itemWindow = MenuItem()
    itemWindow.Header <- "_Window"
    menu.Items.Add itemWindow |> ignore

    let itemTaskbar = MenuItem()
    itemTaskbar.Header <- "_Show In Taskbar"
    itemTaskbar.IsCheckable <- true
    itemTaskbar.IsChecked <- win.ShowInTaskbar
    itemTaskbar.Click.Add(fun _ -> win.ShowInTaskbar <- itemTaskbar.IsChecked)
    itemWindow.Items.Add itemTaskbar |> ignore

    let itemSize = MenuItem()
    itemSize.Header <- "Size to _Content"
    itemSize.IsCheckable <- true
    itemSize.IsChecked <- win.SizeToContent = SizeToContent.WidthAndHeight
    let itemSizeCheckedFun _ = win.SizeToContent <- if itemSize.IsChecked then SizeToContent.WidthAndHeight else SizeToContent.Manual
    itemSize.Checked.Add itemSizeCheckedFun
    itemSize.Unchecked.Add itemSizeCheckedFun
    itemWindow.Items.Add itemSize |> ignore

    let itemResize = MenuItem()
    itemResize.Header <- "_Resizable"
    itemResize.IsCheckable <- true
    itemResize.IsChecked <- win.ResizeMode = ResizeMode.CanResize
    itemResize.Click.Add(fun _ -> win.ResizeMode <- if itemResize.IsChecked then ResizeMode.CanResize else ResizeMode.CanMinimize)
    itemWindow.Items.Add itemResize |> ignore

    let itemTopmost = MenuItem()
    itemTopmost.Header <- "_Topmost"
    itemTopmost.IsCheckable <- true
    itemTopmost.IsChecked <- win.Topmost
    let itemTopmostCheckedFun _ = win.Topmost <- itemTopmost.IsChecked
    itemTopmost.Checked.Add itemTopmostCheckedFun
    itemTopmost.Unchecked.Add itemTopmostCheckedFun
    itemWindow.Items.Add itemTopmost |> ignore

    win.Show()
    //Application().Run(win) |> ignore
with e -> MessageBox.Show(e.Message,"Exception Error") |> ignore