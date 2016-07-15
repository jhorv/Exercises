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

type SystemParam() =
    let mutable strName, objValue = null,null
    
    member t.Name
        with get() = strName
        and set(v:string) = strName <- v

    member t.Value
        with get() = objValue
        and set(v: obj) = objValue <- v

    override t.ToString() =
        sprintf "%s=%A" t.Name t.Value


[<STAThreadAttribute>]
try 
    let grvue = GridView()
    let lsvue = ListView(View=grvue)
    let win = Window(Title = "List System Parameters", Content = lsvue)

    [|
    "Property Name",200.0,Binding "Name"
    "Value",200.0,Binding "Value"
    |] |> Array.iter (fun (n,w,b) -> GridViewColumn(Header=n,Width=w,DisplayMemberBinding=b) |> grvue.Columns.Add)

    // Get all the system parameters in one handy array.
    typeof<SystemParameters>.GetProperties()
    |> Array.sortBy(fun x -> x.ToString())
    |> Array.iter(fun prop ->
        if prop.PropertyType <> typeof<ResourceKey> then
            SystemParam(Name=prop.Name,Value=prop.GetValue(null))
            |> addItems lsvue |> ignore
        )

    win.Show()
    //Application().Run(win) |> ignore
with e -> MessageBox.Show(e.Message,"Exception Error") |> ignore