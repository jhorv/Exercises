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

type TypeTreeViewItem(typ: Type) as t =
    inherit TreeViewItem()

    let mutable typ = typ

    do if typ <> null then t.Type <- typ
    
    new () = TypeTreeViewItem(null)
    member t.Type
        with get() = typ
        and set v = 
            typ <- v

            if typ.IsAbstract then
                t.Header <- sprintf "%s (abstract)" typ.Name
            else
                t.Header <- typ.Name

type ClassHierarchyTreeView(typeRoot: Type) as t =
    inherit TreeView()

    do
        // Make sure PresentationCore is loaded.
        let dummy = UIElement()

        // Get all referenced assemblies.
        let anames = Assembly.GetExecutingAssembly().GetReferencedAssemblies()

        // Put all the referenced assemblies in a List.
        let assemblies = anames |> Array.map (Assembly.Load)

        let classes = SortedList()

        for assembly in assemblies do
            for typ in assembly.GetTypes() do
                if typ.IsPublic && typ.IsSubclassOf typeRoot then
                    classes.Add(typ.Name,typ)

        let item = TypeTreeViewItem(typeRoot) |> addItems t

        t.CreateLinkedItems(item,classes)

    member t.CreateLinkedItems(itemBase: TypeTreeViewItem, list: SortedList<string,Type>) =
        for kvp in list do
            let k,v = kvp.Key,kvp.Value
            if v.BaseType = itemBase.Type then
                let item = TypeTreeViewItem(v)
                itemBase.Items.Add item |> ignore
                t.CreateLinkedItems(item,list)

[<STAThreadAttribute>]
try 
    let treevue = ClassHierarchyTreeView(typeof<DispatcherObject>)
    let win = Window(Title = "Show Class Hierarchy", Content = treevue)

    win.Show()
    //Application().Run(win) |> ignore
with e -> MessageBox.Show(e.Message,"Exception Error") |> ignore

