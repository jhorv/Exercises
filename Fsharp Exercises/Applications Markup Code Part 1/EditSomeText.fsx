#if INTERACTIVE
#r "PresentationCore.dll"
#r "PresentationFramework.dll"
#r "System.Xaml.dll"
#r "WindowsBase.dll"
#r "UIAutomationTypes.dll"
#endif

open System
open System.IO
open System.ComponentModel
open System.Windows
open System.Windows.Input
open System.Windows.Controls
open System.Windows.Controls.Primitives
open System.Windows.Media
open System.Windows.Media.Imaging
open System.Windows.Shapes
open System.Windows.Documents

type EditSomeText() as t =
    inherit Window()

    let filePath = Path.Combine(Environment.GetFolderPath Environment.SpecialFolder.LocalApplicationData,"Temp/EditSomeText.txt")
    let txtbox = 
        { new TextBox() with
            override t.OnKeyDown(args) =
                base.OnKeyDown args
                match args.Key with
                | Key.F5 ->
                    t.SelectedText <- DateTime.Now.ToString()
                    t.CaretIndex <- t.SelectionStart + t.SelectionLength
                | _ -> ()}

    do 
        t.MinHeight <- 300.0
        t.MinWidth <- 300.0
        t.WindowStartupLocation <- WindowStartupLocation.CenterScreen
        t.SizeToContent <- SizeToContent.WidthAndHeight

        t.Title <- "Edit Some Text"

        txtbox.AcceptsReturn <- true
        txtbox.TextWrapping <- TextWrapping.Wrap
        txtbox.VerticalScrollBarVisibility <- ScrollBarVisibility.Auto

        t.Content <- txtbox

        txtbox.Text <- 
            try File.ReadAllText filePath
            with _ -> ""
        txtbox.CaretIndex <- txtbox.Text.Length
        txtbox.Focus() |> ignore

    override t.OnClosing(args) =
        try
            Directory.CreateDirectory(Path.GetDirectoryName filePath) |> ignore
            File.WriteAllText(filePath, txtbox.Text)
        with e ->
            MessageBox.Show("File could not be saved: " + e.Message + "\nClose the program anyway?",t.Title,MessageBoxButton.YesNo,MessageBoxImage.Exclamation)
            |> fun result -> args.Cancel <- (result = MessageBoxResult.No)


            
        
//try EditSomeText().Show()
//with e -> MessageBox.Show(e.Message,"Exception Error") |> ignore

[<STAThreadAttribute>]
do
    try Application().Run(EditSomeText()) |> ignore
    with e -> MessageBox.Show(e.Message,"Exception Error") |> ignore


