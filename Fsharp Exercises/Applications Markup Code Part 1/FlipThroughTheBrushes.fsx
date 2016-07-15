#if INTERACTIVE
#r "PresentationCore.dll"
#r "PresentationFramework.dll"
#r "System.Xaml.dll"
#r "WindowsBase.dll"
#r "UIAutomationTypes.dll"
#endif

open System
open System.Reflection
open System.Windows
open System.Windows.Input
open System.Windows.Controls
open System.Windows.Media

type FlipThroughTheBrushes() as t =
    inherit Window()

    let num_brushes, brushes = 
        let props = typeof<Brushes>.GetProperties(BindingFlags.Static ||| BindingFlags.Public)
        props.Length,
        fun index ->
            props.[max 0 (min (props.Length-1) index)].GetValue(null,null) :?> Brush

    do 
        t.Height <- 400.0
        t.Width <- 400.0
        t.WindowStartupLocation <- WindowStartupLocation.CenterScreen
        t.Background <- brushes 0

    let mutable index = 0

    override t.OnKeyDown args =
        base.OnKeyDown args

        match args.Key with
        | Key.A -> 
            index <- index+1 |> min (num_brushes-1)
            t.Background <- brushes index
        | Key.S -> 
            index <- index-1 |> max 0
            t.Background <- brushes index
        | _ -> ()
            

FlipThroughTheBrushes().Show()


