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
open System.ComponentModel
open System.Windows
open System.Windows.Input
open System.Windows.Controls
open System.Windows.Controls.Primitives
open System.Windows.Media
open System.Windows.Media.Imaging
open System.Windows.Shapes
open System.Windows.Documents

type EditSomeRichText() as t =
    inherit Window()

    let filePath = Path.Combine(Environment.GetFolderPath Environment.SpecialFolder.LocalApplicationData,"Temp/EditSomeRichText.txt")
    

    let txtbox = RichTextBox()

    do 
        t.MinHeight <- 300.0
        t.MinWidth <- 300.0
        t.WindowStartupLocation <- WindowStartupLocation.CenterScreen
        t.SizeToContent <- SizeToContent.WidthAndHeight

        t.Title <- "Edit Some Rich Text"

        txtbox.AcceptsReturn <- true
        txtbox.VerticalScrollBarVisibility <- ScrollBarVisibility.Auto

        t.Content <- txtbox

    override t.OnPreviewTextInput args =
        let strFilter = "Document Files(*.xaml)|*.xaml|All files (*.*)|*.*"
        if args.ControlText.Length > 0 && args.ControlText.[0] = '\x0F' then
            let dlg = OpenFileDialog()
            dlg.CheckFileExists <- true
            dlg.Filter <- strFilter
            if dlg.ShowDialog(t).GetValueOrDefault false then
                let flow = txtbox.Document
                let range = (flow.ContentStart, flow.ContentEnd) |> TextRange
                try
                    use strm = new FileStream(dlg.FileName,FileMode.Open)
                    range.Load(strm,DataFormats.Xaml)
                with e -> MessageBox.Show(e.Message,t.Title) |> ignore
            args.Handled <- true
        elif args.ControlText.Length > 0 && args.ControlText.[0] = '\x13' then
            let dlg = SaveFileDialog()
            dlg.Filter <- strFilter
            if dlg.ShowDialog(t).GetValueOrDefault false then
                let flow = txtbox.Document
                let range = (flow.ContentStart, flow.ContentEnd) |> TextRange
                try
                    use strm = new FileStream(dlg.FileName,FileMode.Create)
                    range.Save(strm,DataFormats.Xaml)
                with e -> MessageBox.Show(e.Message,t.Title) |> ignore
            args.Handled <- true
        base.OnPreviewTextInput args
try EditSomeRichText().Show()
with e -> MessageBox.Show(e.Message,"Exception Error") |> ignore

//[<STAThreadAttribute>]
//do
//    try Application().Run(EditSomeRichText()) |> ignore
//    with e -> MessageBox.Show(e.Message,"Exception Error") |> ignore



