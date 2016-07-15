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


let xamlPanel =
    """
<!-- =================================================
      XamlStackPanel.xaml (c) 2006 by Charles Petzold
     =============================================
==== -->
<StackPanel xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation">

    <Button HorizontalAlignment="Center" Margin="24">
        Hello, XAML!
    </Button>

    <Ellipse Width="200" Height="100" Margin="24"
             Stroke="Red" StrokeThickness="10" />

    <ListBox Width="100" Height="100" Margin="24">
        <ListBoxItem>Sunday</ListBoxItem>
        <ListBoxItem>Monday</ListBoxItem>
        <ListBoxItem>Tuesday</ListBoxItem>
        <ListBoxItem>Wednesday</ListBoxItem>
        <ListBoxItem>Thursday</ListBoxItem>
        <ListBoxItem>Friday</ListBoxItem>
        <ListBoxItem>Saturday</ListBoxItem>
    </ListBox>

</StackPanel>
    """

[<STAThread>]
try
    let content =
        xamlPanel
        |> StringReader
        |> XmlTextReader
        |> XamlReader.Load

    let win = Window(Title="Load Embedded Xaml", Content=content)
    win.Show()

with e -> MessageBox.Show(e.Message,"Exception Error") |> ignore

    