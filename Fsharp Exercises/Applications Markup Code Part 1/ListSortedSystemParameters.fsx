// A bug that I can't fix in this example is to align the text in the second column to the right.
// I actually got the original example from Petzold from his server and tried compiling it.
// It turns out that it has the same error. Setting the horizontal alignment property for
// TextBlock does not affect layout in any way. 

// Well, it is not that important.

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

    let systemParams =
        // Get all the system parameters in one handy array.
        typeof<SystemParameters>.GetProperties()
        |> Array.sortBy(fun x -> x.ToString())
        |> Array.choose(fun prop ->
            if prop.PropertyType <> typeof<ResourceKey> then
                SystemParam(Name=prop.Name,Value=prop.GetValue(null)) |> Some
            else None
            )

    let grvue = GridView()
    let lsvue = ListView(View=grvue,ItemsSource=systemParams)
    let win = Window(Title = "List Sorted System Parameters", Content = lsvue)

    let template = 
        let factoryTextBlock = 
            FrameworkElementFactory(typeof<TextBlock>)
            |> setDP TextBlock.HorizontalAlignmentProperty HorizontalAlignment.Right
            |> setBind' TextBlock.TextProperty (Binding "Value")
        DataTemplate(typeof<string>, VisualTree=factoryTextBlock)

    [|
    GridViewColumn(Header="Property Name",Width=200.0,DisplayMemberBinding=Binding "Name")
    GridViewColumn(Header="Value",Width=200.0,CellTemplate=template)
    |] |> Array.iter (grvue.Columns.Add)

    win.Show()
    //Application().Run(win) |> ignore
with e -> MessageBox.Show(e.Message,"Exception Error") |> ignore