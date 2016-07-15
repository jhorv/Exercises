// Added the Bindings module by Phil Threlford. 
// http://trelford.com/blog/post/F-operator-overloads-for-WPF-dependency-properties.aspx

// Also being stupid, I did not realize at all that in F# one can set the properties during
// class creation. Goddamn it. ...Well, it is a really good feature.

// Also Phil Threlford's calculator example is pretty much inspirational compared to the
// rough C# translation that I did in CalculateInHex.fsx.

// Edit: I ended up removing the stuff by Trelford with my own to make it more F# idiomatic.

// http://stackoverflow.com/questions/38329028/how-do-i-overload-operators-for-wpf-containers/
// Got some interesting responses from the question above.

// Edit2: This code is finally at a level I would not be embarassed to show to somebody else.
// Too bad Intellisense does not work when setting the properties inside the contructor.

// Actually, before I started learning WPF I had no idea what properties even were.
// Like learning Haskell strengtened my FP skills, learning WPF did the same to my OO skills.

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
    let win = Window(Title = "Check the Window Style", Content = dock)

    let menu = Menu() |> DockPanel.DockTop |> Panel.Add dock
    let text = TextBlock(Text=win.Title, FontSize=32.0, TextAlignment=TextAlignment.Center) |> Panel.Add dock
    let itemStyle = MenuItem(Header = "_Style") |> ItemsControl.Add menu

    let mutable itemChecked: MenuItem = null

    let createMenuItem str style =
        let item = MenuItem(Header = str, Tag = style, IsChecked = (style = win.WindowStyle))
        item.Click.Add(fun _ ->
            itemChecked.IsChecked <- false
            itemChecked <- item
            itemChecked.IsChecked <- true
            win.WindowStyle <- style)

        if item.IsChecked then itemChecked <- item
        item

    [|
    createMenuItem "_No border or caption" WindowStyle.None
    createMenuItem "_Single border window" WindowStyle.SingleBorderWindow
    createMenuItem "3_D border window" WindowStyle.ThreeDBorderWindow
    createMenuItem "_Tool window" WindowStyle.ToolWindow
    |] |> Array.iter (ignore << ItemsControl.Add itemStyle)

    win.Show()
    //Application().Run(win) |> ignore
with e -> MessageBox.Show(e.Message,"Exception Error") |> ignore
