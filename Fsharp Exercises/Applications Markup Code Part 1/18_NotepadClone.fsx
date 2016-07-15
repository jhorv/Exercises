// As chapter 17 is on printing I decided to skip it. Likewise, I won't bother testing those features here.
// This is since I do not have a printer.

// Edit: Also, the notepad clone is really quite a bit larger than I thought it would be.
// I think the full version would come out to over 1k LOC. You know, as I do not want to spend two days on
// something I would only use for 5m I think I'll skip this as well. I do not like the design either.
// At this point I'd be better off studying Phil Trelford's examples.

// With that I am done with part 1. Good job.

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
open System.Xml.Serialization

type NotepadCloneSettings() =
    
    member val WindowState = WindowState.Normal with get,set
    member val RestoreBounds  = Rect.Empty with get,set
    member val TextWrapping  = TextWrapping.NoWrap with get,set
    member val FontFamily = "" with get,set
    member val FontStyle = FontStyleConverter().ConvertToString(FontStyles.Normal) with get,set
    member val FontWeight = FontWeightConverter().ConvertToString(FontWeights.Normal) with get,set
    member val FontStretch = FontStretchConverter().ConvertToString(FontStretches.Normal) with get,set
    member val FontSize = 11.0 with get,set

    abstract member Save: string -> bool
    default t.Save strAppData =
        try
            Directory.CreateDirectory(Path.GetDirectoryName(strAppData)) |> ignore
            use write = new StreamWriter(strAppData)
            let xml = XmlSerializer(t.GetType())
            xml.Serialize(write,t)
            true
        with e -> false

    static member Load(typ: Type,strAppData: string): obj =
        let xml = XmlSerializer(typ)
        try
            use reader = new StreamReader(strAppData)
            xml.Deserialize reader
        with e ->
            typ.GetConstructor(Type.EmptyTypes).Invoke null

type NotepadClone() as t =
    inherit Window()

    let mutable isFileDirty = false
    let mutable strLoadedFile = null

    let dock = DockPanel()
    do t.Content <- dock
    
    let asmbly = Assembly.GetExecutingAssembly()
    let title = asmbly.GetCustomAttributes(typeof<AssemblyTitleAttribute>,false).[0] :?> AssemblyTitleAttribute
    let strAppTitle = title.Title

    let product = asmbly.GetCustomAttributes(typeof<AssemblyProductAttribute>,false).[0] :?> AssemblyProductAttribute
    let strAppData =
        Path.Combine(
            Environment.GetFolderPath Environment.SpecialFolder.LocalApplicationData,
            sprintf "Petzold/%s/%s.Settings.xml" product.Product product.Product
            )

    let menu = Menu() |> addChildren dock |> DockPanel.DockTop
    let status = StatusBar() |> addChildren dock |> DockPanel.DockBottom
    let statLineCol = 
        StatusBarItem(HorizontalAlignment=HorizontalAlignment.Right)
        |> addItems status
        |> DockPanel.DockRight

    let txtbox = 
        TextBox(AcceptsReturn=true,AcceptsTab=true,VerticalScrollBarVisibility=ScrollBarVisibility.Auto,
                HorizontalScrollBarVisibility=ScrollBarVisibility.Auto)
        |> addChildren dock

    do txtbox.SelectionChanged.Add <| fun args ->
        let iChar = txtbox.SelectionStart
        let iLine = txtbox.GetLineIndexFromCharacterIndex iChar
        if iLine = -1 then
            statLineCol.Content <- ""
        else
            let iCol = iChar - txtbox.GetCharacterIndexFromLineIndex(iLine)
            let str = 
                sprintf "Line %i Col %i%s" (iLine+1) (iCol+1) (
                    if txtbox.SelectionLength > 0 then
                        let iChar = iChar + txtbox.SelectionLength
                        let iLine = txtbox.GetLineIndexFromCharacterIndex(iChar)
                        let iCol = iChar - txtbox.GetCharacterIndexFromLineIndex(iLine)
                        sprintf " - Line %i Col %i" (iLine+1) (iCol+1)
                    else ""
                    )

            statLineCol.Content <- str

    do txtbox.TextChanged.Add <| fun args ->
        isFileDirty <- true

            // Create all the top-level menu items.
//            AddFileMenu(menu);          // in NotepadClone.File.cs // TODOS
//            AddEditMenu(menu);          // in NotepadClone.Edit.cs
//            AddFormatMenu(menu);        // in NotepadClone.Format.cs
//            AddViewMenu(menu);          // in NotepadClone.View.cs
//            AddHelpMenu(menu);          // in NotepadClone.Help.cs

    let settings = t.LoadSettings() :?> NotepadCloneSettings

    do t.Loaded.Add <| fun args ->
        ApplicationCommands.New.Execute(null,t)

        let strArgs = Environment.GetCommandLineArgs()

        if strArgs.Length > 1 then
            if File.Exists strArgs.[1] then
                //LoadFile // TODO
                ()
            else
                let result =
                    let x = Path.GetFileName(strArgs.[1])
                    MessageBox.Show(sprintf "Cannot find the %s file.\r\n\r\nDo you want to create a new file?" x,
                                    strAppTitle,MessageBoxButton.YesNoCancel,MessageBoxImage.Question)
                if result = MessageBoxResult.Cancel then t.Close()
                elif result = MessageBoxResult.Yes then
                    try
                        strLoadedFile <- strArgs.[1]
                        File.Create(strLoadedFile).Close()
                    with e ->
                        MessageBox.Show(sprintf "Error on File Creation: %s" e.Message,
                                        strAppTitle,MessageBoxButton.OK,MessageBoxImage.Asterisk) |> ignore
                    t.UpdateTitle()

    member t.UpdateTitle() =
        if strLoadedFile = null then
            t.Title <- sprintf "Untitled - %s" strAppTitle
        else
            t.Title <- sprintf "%s - %s" (Path.GetFileName(strLoadedFile)) strAppTitle

    abstract member LoadSettings: unit -> obj
    default t.LoadSettings() =
        NotepadCloneSettings.Load(typeof<NotepadCloneSettings>,strAppData)

    abstract member SaveSettings: unit -> unit
    default t.SaveSettings() =
        settings.Save(strAppData) |> ignore

    override t.OnClosing args =
        base.OnClosing args
        // args.Cancel <- not OkToTrash() TODO
        settings.RestoreBounds <- t.RestoreBounds

    override t.OnClosed args =
        base.OnClosed args
        settings.WindowState <- t.WindowState
        settings.TextWrapping <- txtbox.TextWrapping

        settings.FontFamily <- txtbox.FontFamily.ToString()
        settings.FontStyle <- 
            FontStyleConverter().ConvertToString(txtbox.FontStyle)
        settings.FontWeight <- 
            FontWeightConverter().ConvertToString(txtbox.FontWeight)
        settings.FontStretch <- 
            FontStretchConverter().ConvertToString(txtbox.FontStretch)
        settings.FontSize <- txtbox.FontSize

        t.SaveSettings()



[<STAThreadAttribute>]
try 



    win.Show()
    //Application(ShutdownMode=ShutdownMode.OnMainWindowClose).Run(win) |> ignore
with e -> MessageBox.Show(e.Message,"Exception Error") |> ignore
