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
    let content =
        #if COMPILED
        "pack://application:,,/LoadXamlResource.xml"
        |> Uri
        |> fun uri -> Application.GetResourceStream(uri).Stream
        #else
        Path.Combine(__SOURCE_DIRECTORY__,"LoadXamlResource.xml")
        |> XmlTextReader
        #endif
        |> XamlReader.Load
        |> fun x -> x :?> FrameworkElement

    let win = Window(Title="Load Xaml Resource", Content=content)

    match content.FindName("MyButton") with
    | :? Button as btn ->
        btn.Click.Add <| fun args ->
            MessageBox.Show(sprintf "Button labeled %s has been clicked." btn.Name, win.Title) |> ignore
    | _ -> failwith "Not found"

    win.Show()

with e -> MessageBox.Show(e.Message,"Exception Error") |> ignore