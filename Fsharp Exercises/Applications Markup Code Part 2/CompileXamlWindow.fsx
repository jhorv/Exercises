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

type CompileXamlWindow() as t =
    inherit Window()

    // Required method call to hook up event handlers and
    // initialize fields.
    // do 
        InitializeComponent()




    

[<STAThread>]
try
    let dock = DockPanel()
    let win = Window(Title="Load XAML File", Content=dock)
    let btn = 
        Button(Content="Open File...",Margin=Thickness 12.0,HorizontalAlignment=HorizontalAlignment.Left)
        |> addChildren dock
        |> DockPanel.DockTop

    let frame = Frame() |> addChildren dock
    btn.Click.Add <| fun args ->
        let filt = "XAML Files (*.xaml;*.xml)|*.xaml;*.xml|All files (*.*)|*.*"
        let dlg = OpenFileDialog(Filter=filt)

        if dlg.ShowDialog().GetValueOrDefault false then
            try
                match XmlTextReader(dlg.FileName) |> XamlReader.Load with
                | :? Window as x ->
                    x.Owner <- win
                    x.Show()
                | _ as x -> frame.Content <- x
            with e ->
                MessageBox.Show(e.Message,win.Title) |> ignore

    win.Show()

with e -> MessageBox.Show(e.Message,"Exception Error") |> ignore