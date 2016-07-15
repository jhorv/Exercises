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

type UniformGridAlmost () =
    inherit Panel()

    static let columnsProperty = DependencyProperty.Register("Columns",typeof<int>,typeof<UniformGridAlmost>,FrameworkPropertyMetadata(0,FrameworkPropertyMetadataOptions.AffectsMeasure))

    member t.ColumnsProperty = columnsProperty
    member t.Columns
        with get() = t.GetValue(columnsProperty) :?> int
        and set(v: int) = t.SetValue(columnsProperty,v)

    member t.Rows
        with get() = (t.InternalChildren.Count + t.Columns - 1) / t.Columns

    override t.MeasureOverride sizeAvailable = 
        let sizeChild = Size(sizeAvailable.Width / float t.Columns, sizeAvailable.Height / float t.Rows)

        let mutable maxWidth = 0.0
        let mutable maxHeight = 0.0

        for child in t.InternalChildren do
            child.Measure sizeChild
            maxWidth <- max maxWidth child.DesiredSize.Width
            maxHeight <- max maxHeight child.DesiredSize.Height
            
        Size(float t.Columns * maxWidth, float t.Rows * maxHeight)

    override t.ArrangeOverride sizeFinal =
        let sizeChild = Size(sizeFinal.Width / float t.Columns, sizeFinal.Height / float t.Rows)

        for i=0 to t.InternalChildren.Count-1 do
            let row = i / t.Columns |> float
            let col = i % t.Columns |> float

            let rectChild = Rect(Point(col * sizeChild.Width, row * sizeChild.Height),sizeChild)
            t.InternalChildren.[i].Arrange(rectChild)
        sizeFinal


[<STAThreadAttribute>]
try 
    let win = Window()
    win.Title <- "Duplicate Uniform Grid"
    //win.SizeToContent <- SizeToContent.WidthAndHeight

    let unigrid = UniformGridAlmost()
    unigrid.Columns <- 5
//    unigrid.HorizontalAlignment <- HorizontalAlignment.Center
//    unigrid.VerticalAlignment <- VerticalAlignment.Center
    win.Content <- unigrid

//    unigrid.Width <- 200.0
//    unigrid.Height <- 200.0

    let rand = Random()

    for i=0 to 47 do
        let btn = Button()
        btn.Name <- sprintf "Button%i" i
        btn.Content <- btn.Name
        btn.FontSize <- btn.FontSize + float (rand.Next(10))
//        btn.HorizontalAlignment <- HorizontalAlignment.Center
//        btn.VerticalAlignment <- VerticalAlignment.Center
        unigrid.Children.Add btn |> ignore
        btn.Click.Add(fun args -> MessageBox.Show(sprintf "%s has been clicked." btn.Name, win.Title) |> ignore)

    win.Show()
    //Application().Run(win) |> ignore
with e -> MessageBox.Show(e.Message,"Exception Error") |> ignore



