// This example from the book is particularly badly made. I did what I could, but if I was doing it again, I'd pick a different design.

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

type TyleTag = NumberedTile of int | Empty

type Tile() as t = 
    inherit Canvas()

    let SIZE = 64.0
    let BORD = 8.0
    let txtblk = TextBlock()
    let mutable tag = Empty

    do
        t.Width <- SIZE
        t.Height <- SIZE

        Polygon()
        |> fun poly ->
            poly.Points <-
                [|
                Point(0.0,0.0)
                Point(SIZE,0.0)
                Point(SIZE-BORD,BORD)
                Point(BORD,BORD)
                Point(BORD,SIZE-BORD)
                Point(0.0,SIZE)
                |] |> PointCollection

            poly.Fill <- SystemColors.ControlLightLightBrush
            t.Children.Add poly |> ignore

        Polygon()
        |> fun poly ->
            poly.Points <-
                [|
                Point(SIZE,SIZE)
                Point(SIZE,0.0)
                Point(SIZE-BORD,BORD)
                Point(SIZE-BORD,SIZE-BORD)
                Point(BORD,SIZE-BORD)
                Point(0.0,SIZE)
                |] |> PointCollection

            poly.Fill <- SystemColors.ControlDarkDarkBrush
            t.Children.Add poly |> ignore

        let bord = Border()
        bord.Width <- SIZE - 2.0 * BORD
        bord.Height <- SIZE - 2.0 * BORD
        bord.Background <- Brushes.Black
        t.Children.Add bord |> ignore
        Canvas.SetLeft(bord,BORD)
        Canvas.SetTop(bord,BORD)

        txtblk.FontSize <- 32.0
        txtblk.Foreground <- SystemColors.ControlDarkBrush
        txtblk.HorizontalAlignment <- HorizontalAlignment.Center
        txtblk.VerticalAlignment <- VerticalAlignment.Center
        bord.Child <- txtblk

    member t.Text
        with get() = txtblk.Text
        and set v = txtblk.Text <- v

type Empty() =
    inherit System.Windows.FrameworkElement()

[<STAThreadAttribute>]
try 
    let NumberRows = 4
    let NumberColumns = 4

    let win = Window()
    win.Title <- "Jeu De Tacquin"
    win.SizeToContent <- SizeToContent.WidthAndHeight
    win.ResizeMode <- ResizeMode.CanMinimize
    win.Background <- SystemColors.ControlBrush

    let stack = StackPanel()
    win.Content <- stack

    let btn = Button()
    btn.Content <- "_Scramble"
    btn.Margin <- Thickness 10.0
    btn.HorizontalAlignment <- HorizontalAlignment.Center
    stack.Children.Add btn |> ignore

    let bord = Border()
    bord.BorderBrush <- SystemColors.ControlDarkBrush
    bord.BorderThickness <- Thickness 1.0
    stack.Children.Add bord |> ignore

    let unigrid = UniformGrid()
    unigrid.Rows <- NumberRows
    unigrid.Columns <- NumberColumns
    bord.Child <- unigrid

    let mutable xEmpty = NumberColumns-1
    let mutable yEmpty = NumberRows-1
    let elStore = ResizeArray<UIElement>(NumberColumns*NumberRows)

    let MoveTile(xTile,yTile) =
        if ((xTile = xEmpty && yTile = yEmpty) ||
             xTile < 0 || yTile < 0 || 
             xTile >= NumberColumns || yTile >= NumberRows) |> not then
            
            let iTile = NumberColumns * yTile + xTile
            let iEmpty = NumberColumns * yEmpty + xEmpty

            let t = elStore.[iTile]
            elStore.[iTile] <- elStore.[iEmpty]
            elStore.[iEmpty] <- t

            unigrid.Children.Clear()
            elStore |> Seq.iter (ignore << unigrid.Children.Add)
            
            xEmpty <- xTile
            yEmpty <- yTile

    for i=0 to NumberRows*NumberColumns-2 do
        let tile = Tile()
        tile.Text <- i+1 |> string
        elStore.Add tile
        
        tile.MouseDown.Add(fun args ->
            let iMove = unigrid.Children.IndexOf tile
            let xMove = iMove % NumberColumns
            let yMove = iMove / NumberRows
            if xMove = xEmpty then
                while yMove <> yEmpty do
                    let s = yMove-yEmpty |> sign
                    MoveTile(xMove,yEmpty+s)
            if yMove = yEmpty then
                while xMove <> xEmpty do
                    let s = xMove-xEmpty |> sign
                    MoveTile(xEmpty+s,yMove)
            )
    
    elStore.Add ( Empty())
    elStore |> Seq.iter (ignore << unigrid.Children.Add)

    btn.Click.Add(fun args -> 
        let rand = Random()
        let mutable iCounter = 16 * NumberColumns * NumberRows
        let tmr = DispatcherTimer()
        tmr.Interval <- TimeSpan.FromMilliseconds(10.0)
        tmr.Tick.Add(fun args ->
            for i=0 to 4 do
                MoveTile(xEmpty,yEmpty+rand.Next(3)-1)
                MoveTile(xEmpty+rand.Next(3)-1,yEmpty)
            if iCounter = 0 then tmr.Stop() else iCounter <- iCounter-1
            )
        tmr.Start()
        )

    win.KeyDown.Add(fun args -> 
        match args.Key with
        | Key.Right -> MoveTile(xEmpty+1,yEmpty)
        | Key.Left -> MoveTile(xEmpty-1,yEmpty)
        | Key.Up -> MoveTile(xEmpty,yEmpty-1)
        | Key.Down -> MoveTile(xEmpty,yEmpty+1)
        | _ -> ()
        )

    win.Show()
    //Application().Run(win) |> ignore
with e -> MessageBox.Show(e.Message,"Exception Error") |> ignore
        



