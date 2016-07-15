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
    let menu = ContextMenu()
    let text = TextBlock(FontSize=32.0,HorizontalAlignment=HorizontalAlignment.Center,
                         VerticalAlignment = VerticalAlignment.Center)
    let win = Window(Title = "Popup Context Menu", Content = text)

    let mutable inlClicked : Inline option = None

    let itemBold = MenuItem(Header="Bold") |> addItems menu
    itemBold.Click.Add <| fun args ->
        match inlClicked with
        | Some v -> 
            v.FontWeight <- if itemBold.IsChecked = false then FontWeights.Bold else FontWeights.Normal
        | None -> ()
    let itemItalic = MenuItem(Header="Italic") |> addItems menu
    itemItalic.Click.Add <| fun args ->
        match inlClicked with
        | Some v -> v.FontStyle <- if itemItalic.IsChecked = false then FontStyles.Italic else FontStyles.Normal
        | None -> ()

    let itemDecor =
        Enum.GetValues(typeof<TextDecorationLocation>) :?> TextDecorationLocation []
        |> Array.map (fun loc ->
            let decor = TextDecoration(Location = loc)
            MenuItem(Header=loc.ToString(),Tag=decor) |> addItems menu
            |> fun item ->
                item.Click.Add <| fun args ->
                    match inlClicked with
                    | Some v ->
                        if item.IsChecked = false then v.TextDecorations.Add(decor) 
                        else v.TextDecorations.Remove(decor) |> ignore
                    | None -> ()
                item
            )

    "To be, or not to be, that is the question".Split()
    |> Array.iter (fun word ->
        Run(word, TextDecorations=TextDecorationCollection()) 
        |> fun inl ->
            inl.MouseRightButtonUp.Add <| fun args ->
                itemBold.IsChecked <- inl.FontWeight = FontWeights.Bold
                itemItalic.IsChecked <- inl.FontStyle = FontStyles.Italic
                for item in itemDecor do
                    item.IsChecked <- inl.TextDecorations.Contains(item.Tag :?> TextDecoration)
                menu.IsOpen <- true
                args.Handled <- true
                inlClicked <- inl :> Inline |> Some
            text.Inlines.Add inl
        text.Inlines.Add " "
        )

    win.Show()
    //Application().Run(win) |> ignore
with e -> MessageBox.Show(e.Message,"Exception Error") |> ignore
