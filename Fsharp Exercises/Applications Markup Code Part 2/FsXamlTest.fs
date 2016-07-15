// I can't really transcribe the rest of the examples from the chapter straightforwardly as C# can generate code
// by using XAML files unlike F#. Instead what I will do now is figure out the FsXaml type provider.

// Edit: It is no good. I'll put this project on freeze until the F# UI library situation gets better.

#if INTERACTIVE
#r "PresentationCore.dll"
#r "PresentationFramework.dll"
#r "System.Xaml.dll"
#r "WindowsBase.dll"
#r "UIAutomationTypes.dll"
#I "../packages/FsXaml.Wpf.2.1.0/lib/net45"
#r "FsXaml.Wpf.dll"
#r "FsXaml.Wpf.TypeProvider.dll"
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
open FsXaml

[<Literal>]
let xamlPath = @"C:\Users\Marko\Documents\Visual Studio 2015\Projects\GUI Experiments\Applications Markup Code Part 2\LoadXamlResource.xml"

type MainResource = XAML<xamlPath>

[<STAThread>]
try
    let res = MainResource()
    //let win = Window(Title="FsXamlTest", Content=res)
    //printfn "%A" (res.GetType())
    //win.Show()
    ()
with e -> MessageBox.Show(e.Message,"Exception Error") |> ignore