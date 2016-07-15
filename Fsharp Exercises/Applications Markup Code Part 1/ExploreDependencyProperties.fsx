// There is an error in TypeToString class in the book. It got fixed in the online source.

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

let TypeToString =
    {new IValueConverter with
        member t.Convert(o,typ,param,culture) =
            (o :?> Type).Name |> box
        member t.ConvertBack(o,typ,p,c) = null}

let MetadataToFlags =
    {new IValueConverter with
        member t.Convert(o,typ,param,culture) =
            let flags = FrameworkPropertyMetadataOptions.None
            match o with
            | :? FrameworkPropertyMetadata as metadata ->
                [|
                FrameworkPropertyMetadataOptions.AffectsArrange,metadata.AffectsArrange
                FrameworkPropertyMetadataOptions.AffectsMeasure,metadata.AffectsMeasure
                FrameworkPropertyMetadataOptions.AffectsParentArrange,metadata.AffectsParentArrange
                FrameworkPropertyMetadataOptions.AffectsParentMeasure,metadata.AffectsParentMeasure
                FrameworkPropertyMetadataOptions.AffectsRender,metadata.AffectsRender
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,metadata.BindsTwoWayByDefault
                FrameworkPropertyMetadataOptions.Inherits,metadata.Inherits
                FrameworkPropertyMetadataOptions.Journal,metadata.Journal
                FrameworkPropertyMetadataOptions.NotDataBindable,metadata.IsNotDataBindable
                FrameworkPropertyMetadataOptions.OverridesInheritanceBehavior,metadata.OverridesInheritanceBehavior
                FrameworkPropertyMetadataOptions.SubPropertiesDoNotAffectRender,metadata.SubPropertiesDoNotAffectRender
                |] 
                |> Array.fold (fun s (x,b) -> s ||| if b then x |> int else 0) 0
                |> box
            | _ -> null
                

        member t.ConvertBack(o,typ,p,c) =
            FrameworkPropertyMetadata(null,o :?> FrameworkPropertyMetadataOptions)
            |> box
            }

type DependencyPropertyListView() as t =
    inherit ListView()

    static let typeProperty = 
        let onPropChanged =
            PropertyChangedCallback(fun obj args ->
                let lstvue = obj :?> DependencyPropertyListView
                let typ = args.NewValue :?> Type
                
                lstvue.ItemsSource <- null                   
                if typ <> null then
                    lstvue.ItemsSource <-
                        typ.GetFields()
                        |> Array.choose (fun info ->
                            if info.FieldType = typeof<DependencyProperty> then
                                (info.Name,info.GetValue(null) :?> DependencyProperty) |> Some
                            else None)
                        |> fun x -> Array.sortInPlaceBy fst x; x
                        |> Array.map snd
                )
        
        DependencyProperty.Register(
            "Type",typeof<Type>,typeof<DependencyPropertyListView>,
            PropertyMetadata(null,onPropChanged))

    do
        let grvue = GridView()
        t.View <- grvue

        let templateOwner = 
            let elTextBlock = 
                FrameworkElementFactory(typeof<TextBlock>)
                |> setBind' TextBlock.TextProperty (Binding("OwnerType",Converter=TypeToString))
            DataTemplate(VisualTree=elTextBlock)

        let templateProperty =
            let elTextBlock = 
                FrameworkElementFactory(typeof<TextBlock>)
                |> setBind' TextBlock.TextProperty (Binding("PropertyType",Converter=TypeToString))
            DataTemplate(VisualTree=elTextBlock)

        let templateMetadata =
            let elTextBlock = 
                FrameworkElementFactory(typeof<TextBlock>)
                |> setBind' TextBlock.TextProperty (Binding("DefaultMetadata",Converter=MetadataToFlags))
            DataTemplate(VisualTree=elTextBlock)

        [|
        GridViewColumn(Header="Name",Width=200.0,DisplayMemberBinding=Binding "Name")
        GridViewColumn(Header="Owner",Width=100.0,CellTemplate=templateOwner)
        GridViewColumn(Header="Type",Width=100.0,CellTemplate=templateProperty)
        GridViewColumn(Header="Default",Width=75.0,DisplayMemberBinding=Binding "DefaultMetadata.DefaultValue")
        GridViewColumn(Header="Read-Only",Width=75.0,DisplayMemberBinding=Binding "DefaultMetadata.ReadOnly")
        GridViewColumn(Header="Usage",Width=75.0,DisplayMemberBinding=Binding "DefaultMetadata.AttachedPropertyUsage")
        GridViewColumn(Header="Flags",Width=250.0,CellTemplate=templateMetadata)
        |] |> Array.iter (grvue.Columns.Add)
        
    static member TypeProperty = typeProperty
    member t.Type
        with get() = t.GetValue(typeProperty) :?> Type
        and set(v: Type) = t.SetValue(typeProperty,v)

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
    let grid = Grid()
    let win = Window(Title = "Explore Dependency Properties", Content = grid)
    [|
    ColumnDefinition(Width=GridLength(1.0,GridUnitType.Star))
    ColumnDefinition(Width=GridLength.Auto)
    ColumnDefinition(Width=GridLength(3.0,GridUnitType.Star))
    |] |> Array.iter grid.ColumnDefinitions.Add

    let treevue = 
        ClassHierarchyTreeView(typeof<DependencyObject>)
        |> addGrid grid 0 0

    let split = 
        GridSplitter(HorizontalAlignment=HorizontalAlignment.Center,VerticalAlignment=VerticalAlignment.Stretch,
                 Width=6.0)
        |> addGrid grid 0 1
    
    let lstvue =
        DependencyPropertyListView(DataContext=treevue)
        |> addGrid grid 0 2
        |> setBind DependencyPropertyListView.TypeProperty "SelectedItem.Type"

    win.Show()
    //Application().Run(win) |> ignore
with e -> MessageBox.Show(e.Message,"Exception Error") |> ignore
