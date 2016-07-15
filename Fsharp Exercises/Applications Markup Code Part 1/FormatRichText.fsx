// Although I transcribed this faithfully, there are some races between txtbox and toolbar handlers.
// I can't figure out how to fix this as one event triggers the other. Probably some flags or something.

// I need a different way of thinking about this. It just goes to show that concurrency is difficult
// if Petzold could not do it. I'll leave this for later.

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
open System.Windows.Data

type ColorGridBox() as t =
    inherit ListBox(SelectedValuePath = "Fill")

    static let strColorsNames =
        [|
            [|"Black"; "Brown"; "DarkGreen"; "MidnightBlue"; "Navy"; "DarkBlue"; "Indigo"; "DimGray"|]
            [|"DarkRed"; "OrangeRed"; "Olive"; "Green"; "Teal"; "Blue"; "SlateGray"; "Gray"|]
            [|"Red"; "Orange"; "YellowGreen"; "SeaGreen"; "Aqua"; "LightBlue"; "Violet"; "DarkGray"|]
            [|"Pink"; "Gold"; "Yellow"; "Lime"; "Turquoise"; "SkyBlue"; "Plum"; "LightGray"|]
            [|"LightPink"; "Tan"; "LightYellow"; "LightGreen"; "LightCyan"; "LightSkyBlue"; "Lavender"; "White"|]
        |]
        |> Array.concat

    do
        let factoryUnigrid = 
            FrameworkElementFactory(typeof<UniformGrid>)
            |> setDP UniformGrid.ColumnsProperty 8
        t.ItemsPanel <- ItemsPanelTemplate(factoryUnigrid)

        for strColor in strColorsNames do
            // Create ToolTip for Rectangle.
            let tip = ToolTip(Content = strColor)

            // Create Rectangle and add to ListBox.
            let col = typeof<Brushes>.GetProperty(strColor).GetValue(null) :?> Brush
            Rectangle(Width = 12.0, Height = 12.0, Margin = Thickness 4.0, Fill = col, ToolTip=tip) 
            |> addItems t
            |> ignore

[<STAThreadAttribute>]
try 
    let dock = DockPanel()
    let win = Window(Title = "Format Rich Text", Content = dock)
    let tray = ToolBarTray() |> addChildren dock |> DockPanel.DockTop
    
    let txtbox = RichTextBox()

    let addFileToolBar =
        let formatsFilter =
            [|
            DataFormats.Xaml,"XAML Document Files (*.xaml)|*.xaml"
            DataFormats.XamlPackage,"XAML Package Files (*.zip)|*.zip"
            DataFormats.Rtf,"Rich Text Format Files (*.rtf)|*.rtf"
            DataFormats.Text,"Text Files (*.txt)|*.txt"
            DataFormats.Text,"All files (*.*)|*.*"
            |]

        let strFilter =
            formatsFilter
            |> Array.map snd
            |> String.concat "|"

        fun (tray: ToolBarTray) band index ->
            // New: Set content to an empty string.
            let onNew _ _ =
                let flow = txtbox.Document
                let range = TextRange(flow.ContentStart,flow.ContentEnd)
                range.Text <- ""

            // Open: Display dialog box and load file.
            let onOpen _ _ = 
                let dlg = OpenFileDialog(CheckFileExists=true,Filter=strFilter)
                if (dlg.ShowDialog win).GetValueOrDefault false then
                    let flow = txtbox.Document
                    let range = TextRange(flow.ContentStart,flow.ContentEnd)
                    try
                        use strm = new FileStream(dlg.FileName,FileMode.Open)
                        range.Load(strm,formatsFilter.[dlg.FilterIndex-1] |> fst)
                    with e -> MessageBox.Show(e.Message,win.Title) |> ignore

            // Save: Display dialog box and save file.
            let onSave _ _ =
                let dlg = SaveFileDialog(Filter=strFilter)
                if (dlg.ShowDialog win).GetValueOrDefault false then
                    let flow = txtbox.Document
                    let range = TextRange(flow.ContentStart,flow.ContentEnd)
                    try
                        use strm = new FileStream(dlg.FileName,FileMode.Create)
                        range.Save(strm,formatsFilter.[dlg.FilterIndex-1] |> fst)
                    with e -> MessageBox.Show(e.Message,win.Title) |> ignore

            let toolbar = ToolBar(Band=band,BandIndex=index) |> addToolBars tray
            [|
            ApplicationCommands.New,"NewDocumentHS.png",(ApplicationCommands.New,onNew)
            ApplicationCommands.Open,"openHS.png",(ApplicationCommands.Open,onOpen)
            ApplicationCommands.Save,"saveHS.png",(ApplicationCommands.Save,onSave)
            |] |> Array.iter (fun (comm: RoutedUICommand,imgName: string,bind) ->
                // Saves me some typing.
                bind |> CommandBinding |> win.CommandBindings.Add |> ignore

                #if INTERACTIVE
                let bitmap = BitmapImage(Uri(Path.Combine(__SOURCE_DIRECTORY__,"Images",imgName)))
                #else
                let bitmap = BitmapImage(Uri(Path.Combine("pack://application:,,/Images/",imgName)))
                #endif
                let img = Image(Source=bitmap,Stretch=Stretch.None)
                let tip = ToolTip(Content=comm.Text)
                Button(Command=comm,Content=img,ToolTip=tip) |> addItems toolbar |> ignore
                CommandBinding(comm,(fun _ _ -> MessageBox.Show(sprintf "%s not yet implemented" comm.Name, win.Title) |> ignore))
                |> win.CommandBindings.Add |> ignore
                )
    
    let addEditToolBar (tray: ToolBarTray) band index =
        let toolbar = ToolBar(Band=band,BandIndex=index) |> addToolBars tray
        let addCommands (comm: RoutedUICommand,imgName: string,bind) =
            #if INTERACTIVE
            let bitmap = BitmapImage(Uri(Path.Combine(__SOURCE_DIRECTORY__,"Images",imgName)))
            #else
            let bitmap = BitmapImage(Uri(Path.Combine("pack://application:,,/Images/",imgName)))
            #endif
            let img = Image(Source=bitmap,Stretch=Stretch.None)
            let tip = ToolTip(Content=comm.Text)
            Button(Command=comm,Content=img,ToolTip=tip) |> addItems toolbar |> ignore

            match bind with
            | Some(executed,canExecute) -> 
                CommandBinding(
                    comm,
                    ExecutedRoutedEventHandler executed,
                    CanExecuteRoutedEventHandler canExecute) |> win.CommandBindings.Add
            | None -> CommandBinding(comm) |> win.CommandBindings.Add 
            |> ignore

        let onDelete _ _ = txtbox.Selection.Text <- ""
        let canDelete _ (args: CanExecuteRoutedEventArgs) =
            args.CanExecute <- true
        [|
        ApplicationCommands.Cut,"CutHS.png",None
        ApplicationCommands.Copy,"CopyHS.png",None
        ApplicationCommands.Paste,"PasteHS.png",None
        ApplicationCommands.Delete,"DeleteHS.png",Some(onDelete, canDelete)
        |] |> Array.iter addCommands
        toolbar.Items.Add (Separator()) |> ignore
        [|
        ApplicationCommands.Undo,"Edit_UndoHS.png",None
        ApplicationCommands.Redo,"Edit_RedoHS.png",None
        |] |> Array.iter addCommands

    let addCharToolBar (tray: ToolBarTray) band index =
        let toolbar = ToolBar(Band=band,BandIndex=index) |> addToolBars tray

        let comboFamily = 
            let tip = ToolTip(Content="Font Family")
            ComboBox(Width=144.0,ItemsSource=Fonts.SystemFontFamilies,
                     SelectedItem=txtbox.FontFamily,ToolTip=tip) |> addItems toolbar

        comboFamily.SelectionChanged.Add <| fun _ ->
            let family = comboFamily.SelectedItem :?> FontFamily
            if family <> null then
                txtbox.Selection.ApplyPropertyValue(FlowDocument.FontFamilyProperty,family)
                
            txtbox.Focus() |> ignore

        let comboSize = 
            let sizes = 
                [|8.0; 9.0; 10.0; 11.0; 12.0; 14.0; 16.0; 18.0;
                  20.0; 22.0; 24.0; 26.0; 28.0; 36.0; 48.0; 72.0|]
            ComboBox(Width=48.0,IsEditable=true,Text=(0.75 * txtbox.FontSize).ToString(),
                     ItemsSource=sizes) |> addItems toolbar

        let mutable strOriginal = ""

        comboSize.SelectionChanged.Add <| fun _ ->
            strOriginal <- comboSize.Text

        comboSize.LostFocus.Add <| fun _ ->
            match Double.TryParse(comboSize.Text) with
            | true, size -> txtbox.Selection.ApplyPropertyValue(FlowDocument.FontSizeProperty,size / 0.75)
            | false, _ -> comboSize.Text <- strOriginal

        comboSize.KeyDown.Add <| fun args ->
            match args.Key with
            | Key.Escape -> 
                comboSize.Text <- strOriginal
                args.Handled <- true
                txtbox.Focus() |> ignore
            | Key.Enter ->
                args.Handled <- true
                txtbox.Focus() |> ignore
            | _ -> ()
            
        comboSize.SelectionChanged.Add <| fun _ ->
            if comboSize.SelectedIndex <> -1 then
                let size = comboSize.SelectedValue :?> float
                txtbox.Selection.ApplyPropertyValue(FlowDocument.FontSizeProperty,size / 0.75)
                txtbox.Focus() |> ignore
        
        let btnBold = 
            #if COMPILED
            let uri = Uri(Path.Combine("pack://application:,,/Images/","boldhs.png"))
            #else
            let uri = Uri(Path.Combine(__SOURCE_DIRECTORY__,"Images","boldhs.png"))
            #endif
            let img = Image(Source=BitmapImage(uri),
                            Stretch=Stretch.None)
            let tip = ToolTip(Content="Bold")
            ToggleButton(Content=img, ToolTip=tip) |> addItems toolbar

        btnBold.Checked.Add <| fun _ -> 
            txtbox.Selection.ApplyPropertyValue(FlowDocument.FontWeightProperty,FontWeights.Bold)

        btnBold.Unchecked.Add <| fun _ -> 
            txtbox.Selection.ApplyPropertyValue(FlowDocument.FontWeightProperty,FontWeights.Normal)

        let btnItalic =
            #if COMPILED
            let uri = Uri(Path.Combine("pack://application:,,/Images/","ItalicHS.png"))
            #else
            let uri = Uri(Path.Combine(__SOURCE_DIRECTORY__,"Images","ItalicHS.png"))
            #endif
            let img = Image(Source=BitmapImage(uri),
                            Stretch=Stretch.None)
            let tip = ToolTip(Content="Italic")
            ToggleButton(Content=img, ToolTip=tip) |> addItems toolbar

        btnItalic.Checked.Add <| fun _ -> 
            txtbox.Selection.ApplyPropertyValue(FlowDocument.FontStyleProperty,FontStyles.Italic)

        btnItalic.Unchecked.Add <| fun _ -> 
            txtbox.Selection.ApplyPropertyValue(FlowDocument.FontStyleProperty,FontStyles.Normal)

        toolbar.Items.Add (Separator()) |> ignore

        let menu = 
            Menu() |> addItems toolbar

        let item =
            #if COMPILED
            let uri = Uri(Path.Combine("pack://application:,,/Images/","ColorHS.png"))
            #else
            let uri = Uri(Path.Combine(__SOURCE_DIRECTORY__,"Images","ColorHS.png"))
            #endif
            let img = Image(Source=BitmapImage(uri),
                            Stretch=Stretch.None)
            let tip = ToolTip(Content="Background Color")
            MenuItem(Header=img, ToolTip=tip) |> addItems menu

        let clrboxBackground = 
            ColorGridBox() |> addItems item

        clrboxBackground.SelectionChanged.Add <| fun _ ->
            if clrboxBackground.SelectedValue <> null then
                txtbox.Selection.ApplyPropertyValue(FlowDocument.BackgroundProperty,clrboxBackground.SelectedValue)

        let item =
            #if COMPILED
            let uri = Uri(Path.Combine("pack://application:,,/Images/","Color_fontHS.png"))
            #else
            let uri = Uri(Path.Combine(__SOURCE_DIRECTORY__,"Images","Color_fontHS.png"))
            #endif
            let img = Image(Source=BitmapImage(uri),
                            Stretch=Stretch.None)
            let tip = ToolTip(Content="Foreground Color")
            MenuItem(Header=img, ToolTip=tip) |> addItems menu

        let clrboxForeground = 
            ColorGridBox() |> addItems item

        clrboxForeground.SelectionChanged.Add <| fun _ ->
            if clrboxForeground.SelectedValue <> null then
                txtbox.Selection.ApplyPropertyValue(FlowDocument.ForegroundProperty,clrboxForeground.SelectedValue)

        txtbox.SelectionChanged.Add <| fun _ ->
            match txtbox.Selection.GetPropertyValue(FlowDocument.FontFamilyProperty) with
            | :? FontFamily as f -> comboFamily.SelectedItem <- f
            | _ -> comboFamily.SelectedIndex <- -1

            match txtbox.Selection.GetPropertyValue(FlowDocument.FontSizeProperty) with
            | :? float as f -> comboSize.Text <- (0.75 * f) |> string
            | _ -> comboSize.SelectedIndex <- -1

            match txtbox.Selection.GetPropertyValue(FlowDocument.FontWeightProperty) with
            | :? FontWeight as f -> btnBold.IsChecked <- f = FontWeights.Bold |> Nullable
            | _ -> ()

            match txtbox.Selection.GetPropertyValue(FlowDocument.FontStyleProperty) with
            | :? FontStyle as f -> btnItalic.IsChecked <- f = FontStyles.Italic |> Nullable
            | _ -> ()

            match txtbox.Selection.GetPropertyValue(FlowDocument.BackgroundProperty) with
            | :? Brush as f -> clrboxBackground.SelectedValue <- f
            | _ -> ()

            match txtbox.Selection.GetPropertyValue(FlowDocument.ForegroundProperty) with
            | :? Brush as f -> clrboxForeground.SelectedValue <- f
            | _ -> ()
                
    let addParaToolBar (tray: ToolBarTray) band index =
        let toolbar = ToolBar(Band=band,BandIndex=index) |> addToolBars tray

        let rec btnsAlign: ToggleButton[] =
            [|
            createButton TextAlignment.Left "Align Left" 0.0 4.0
            createButton TextAlignment.Center "Center" 2.0 2.0
            createButton TextAlignment.Right "Align Right" 4.0 0.0
            createButton TextAlignment.Justify "Justify" 0.0 0.0
            |] |> Array.map (addItems toolbar)
        and createButton align strToolTip offsetLeft offsetRight =
            let canv = Canvas(Width=16.0,Height=16.0)
            let btn = 
                let tip = ToolTip(Content=strToolTip)
                ToggleButton(Tag=align,Content=canv,ToolTip=tip)

            btn.Click.Add <| fun _ ->
                for btnAlign in btnsAlign do
                    btnAlign.IsChecked <- btn = btnAlign |> Nullable
                txtbox.Selection.ApplyPropertyValue(Paragraph.TextAlignmentProperty,align)

            for i=0 to 4 do
                let poly = Polyline(Stroke=SystemColors.WindowTextBrush,StrokeThickness=1.0)
                if i % 2 = 0 then
                    poly.Points <-
                        let i = float i
                        PointCollection([|Point(2.0,2.0+3.0*i)
                                          Point(14.0,2.0+3.0*i)|])
                else
                    poly.Points <-
                        let i = float i
                        PointCollection([|Point(2.0+offsetLeft,2.0+3.0*i)
                                          Point(14.0+offsetRight,2.0+3.0*i)|])

                canv.Children.Add poly |> ignore

            btn

        txtbox.SelectionChanged.Add <| fun _ ->
            match txtbox.Selection.GetPropertyValue(Paragraph.TextAlignmentProperty) with  
            | :? TextAlignment as align -> 
                for btn in btnsAlign do
                    btn.IsChecked <- align = (btn.Tag :?> TextAlignment) |> Nullable
            | _ -> 
                for btn in btnsAlign do
                    btn.IsChecked <- false |> Nullable

    let addStatusBar (dock: DockPanel) =
        let status = StatusBar() |> addChildren dock |> DockPanel.DockBottom

        let getDT()=
            let dt = DateTime.Now
            sprintf "%s %s" (dt.ToLongDateString()) (dt.ToLongTimeString())

        let itemDateTime = 
            StatusBarItem(Content=getDT(), HorizontalAlignment=HorizontalAlignment.Left)
            |> addItems status

        let tmr = DispatcherTimer(Interval=TimeSpan.FromSeconds 1.0)
        tmr.Tick.Add <| fun _ ->
            itemDateTime.Content <- getDT()
        tmr.Start()


    // Call methods in other files...
    addFileToolBar tray 0 0
    addEditToolBar tray 1 0
    addCharToolBar tray 2 0
    addParaToolBar tray 2 1
    addStatusBar dock

    dock.Children.Add txtbox |> ignore

    txtbox.Focus() |> ignore

    win.Show()
    Application().Run(win) |> ignore
with e -> MessageBox.Show(e.Message,"Exception Error") |> ignore
