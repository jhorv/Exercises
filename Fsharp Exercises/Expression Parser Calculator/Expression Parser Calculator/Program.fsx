// A slight calculator example using an expression parser that I had meant to do for a while. Obeys
// precedence rules faithfully. It was much easier than I'd thought it would be to make this.

// I do not know why, but compared to 4 months ago, I can understand the documentation much more
// clearly that before. Maybe learning WPF strenghtened my OO skills similarly to how Haskell
// did my FP skills? Or maybe it is the focus received from wanting to do this one specific task?

// At any rate, I thought it might be good to use bindings at first, but it turns out that bindings
// really do lack flexibility. Event handlers are a must. For what I wanted to do Petzold's book was 
// a bit of an overkill for me at any rate, but it will serve me well in the time to come.

#if INTERACTIVE
#r "PresentationCore.dll"
#r "PresentationFramework.dll"
#r "WindowsBase.dll"
#load "Bindings.fs"
#I @"..\packages\FParsec.1.0.2\lib\net40-client\"
#r "FParsecCS.dll"
#r "FParsec.dll"
#endif

open System
open System.Windows
open System.Windows.Controls
open System.Windows.Media

open Bindings

open FParsec

type Assoc = Associativity

let runCalculatorParser =
    let ws = spaces
    let str_ws s = pstring s >>. ws

    let opp = new OperatorPrecedenceParser<float,unit,unit>()
    let expr = opp.ExpressionParser
    let term = (pfloat .>> ws) <|> between (str_ws "(") (str_ws ")") expr
    opp.TermParser <- term

    opp.AddOperator(InfixOperator("+", ws, 10, Assoc.Left, fun x y -> x + y))
    opp.AddOperator(InfixOperator("-", ws, 10, Assoc.Left, fun x y -> x - y))
    opp.AddOperator(InfixOperator("*", ws, 20, Assoc.Left, fun x y -> x * y))
    opp.AddOperator(InfixOperator("/", ws, 20, Assoc.Left, fun x y -> x / y))

    fun str -> run expr str
        
[<STAThreadAttribute>]
try 
    let dock = DockPanel()
    let scroll = ScrollViewer(Content=dock,VerticalScrollBarVisibility=ScrollBarVisibility.Hidden,
                              HorizontalScrollBarVisibility=ScrollBarVisibility.Auto)
    let win = Window(Title="Expression Parser Calculator", Content=scroll, MinWidth=300.0, MaxWidth=600.0,
                     SizeToContent=SizeToContent.WidthAndHeight,FontFamily=FontFamily("Consolas"))
    let txtbox = TextBox() |> addDock dock Dock.Top
    let txtblk = 
        TextBlock(HorizontalAlignment=HorizontalAlignment.Right) 
        |> addDock dock Dock.Top 

    txtbox.TextChanged.Add <| fun _ ->
        match runCalculatorParser txtbox.Text with
        | Success(v,_,_) ->
            txtblk.Foreground <- SystemColors.WindowTextBrush
            txtblk.Text <- string v
        | Failure(v,_,_) ->
            txtblk.Foreground <- Brushes.Red
            txtblk.Text <- v

    win.Show()
    Application().Run(win) |> ignore
with e -> MessageBox.Show(e.Message,"Exception Error") |> ignore
