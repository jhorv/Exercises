#if INTERACTIVE
#r "PresentationCore.dll"
#r "PresentationFramework.dll"
#r "System.Xaml.dll"
#r "WindowsBase.dll"
#r "UIAutomationTypes.dll"
#endif

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

[<STAThreadAttribute>]
try 
    let win = Window()
    win.Title <- "Build Button Factory"

    // Create a ControlTemplate intended for a Button object.
    let template = ControlTemplate(typeof<Button>)

    // Create a FrameworkElementFactory for the Border class.
    let factoryBorder = FrameworkElementFactory(typeof<Border>)
    factoryBorder.Name <- "border"

    // Set certain default properties.
    factoryBorder.SetValue(Border.BorderBrushProperty, Brushes.Red)
    factoryBorder.SetValue(Border.BorderThicknessProperty, Thickness 3.0)
    factoryBorder.SetValue(Border.BackgroundProperty, SystemColors.ControlLightBrush)

    // Create a FrameworkElementFactory for the ContentPresenter class.
    let factoryContent = FrameworkElementFactory(typeof<ContentPresenter>)
    factoryContent.Name <- "content"

    // Bind some ContentPresenter properties to Button properties.
    factoryContent.SetValue(ContentPresenter.ContentProperty, TemplateBindingExtension(Button.ContentProperty))
    factoryContent.SetValue(ContentPresenter.MarginProperty, TemplateBindingExtension(Button.PaddingProperty))

    // Make the ContentPresenter a child of the Border.
    factoryBorder.AppendChild(factoryContent)

    // Make the Border the root element of the visual tree.
    template.VisualTree <- factoryBorder

    // Define a new Trigger when IsMouseOver is true.
    Trigger()
    |> fun trig ->
        trig.Property <- UIElement.IsMouseOverProperty
        trig.Value <- true

        // Associate a Setter with that Trigger to change the CornerRadius property of the "border" element.
        Setter()
        |> fun set ->
            set.Property <- Border.CornerRadiusProperty
            set.Value <- CornerRadius 24.0
            set.TargetName <- "border"
            trig.Setters.Add set

        // Similarly, define a Setter to change the FontStyle.
        // (No TargetName is needed because it's the button's property.)
        Setter()
        |> fun set ->
            set.Property <- Control.FontStyleProperty
            set.Value <- FontStyles.Italic
            trig.Setters.Add set
        
        template.Triggers.Add trig

    Trigger()
    |> fun trig ->
        trig.Property <- Button.IsPressedProperty
        trig.Value <- true

        let set = Setter()
        set.Property <- Border.BackgroundProperty
        set.Value <- SystemColors.ControlDarkBrush
        set.TargetName <- "border"
        trig.Setters.Add set

        template.Triggers.Add trig

    let btn = Button()
    btn.Template <- template

    btn.Content <- "Button with custom template"
    btn.Padding <- Thickness 20.0
    btn.FontSize <- 48.0
    btn.HorizontalAlignment <- HorizontalAlignment.Center
    btn.VerticalAlignment <- VerticalAlignment.Center
    win.Content <- btn

    btn.Click.Add(fun _ -> MessageBox.Show("You clicked the button.", win.Title) |> ignore)

    win.Show()
    //Application().Run(win) |> ignore
with e -> MessageBox.Show(e.Message,"Exception Error") |> ignore




