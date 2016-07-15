// Time for the fun stuff.

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
open System.Collections.Generic
open System.Diagnostics
open System.Reflection
open System.ComponentModel
open System.Globalization
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
open System.Windows.Markup
open System.Xml

[<STAThread>]
try
    let win =
        #if COMPILED
        "pack://application:,,/LoadXamlWindow.xml"
        |> Uri
        |> fun uri -> Application.GetResourceStream(uri).Stream
        #else
        Path.Combine(__SOURCE_DIRECTORY__,"LoadXamlWindow.xml")
        |> XmlTextReader
        #endif
        |> XamlReader.Load
        |> fun x -> x :?> Window

    let onClick =
        RoutedEventHandler(fun _ args ->
            MessageBox.Show(sprintf "The button labeled %A has been clicked" (args.Source :?> Button).Content, win.Title)
            |> ignore
            )

    win.AddHandler(Button.ClickEvent,onClick)

    win.Show()

with e -> MessageBox.Show(e.Message,"Exception Error") |> ignore