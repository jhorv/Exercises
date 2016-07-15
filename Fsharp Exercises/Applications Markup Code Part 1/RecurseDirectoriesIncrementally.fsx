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

let loadImage (file: string) =
    #if INTERACTIVE
    Path.Combine(__SOURCE_DIRECTORY__,"Images",file)
    |> Uri |> BitmapImage
    #else
    Path.Combine("pack://application:,,/Images/",file)
    |> Uri |> BitmapImage
    #endif

let loadImageDir (file: string) =
    #if INTERACTIVE
    Path.Combine(__SOURCE_DIRECTORY__,"ImagesDir",file)
    |> Uri |> BitmapImage
    #else
    Path.Combine("pack://application:,,/ImagesDir/",file)
    |> Uri |> BitmapImage
    #endif

type ImagedTreeViewItem() as t =
    inherit TreeViewItem()

    let stack = StackPanel(Orientation=Orientation.Horizontal)
    do  t.Header <- stack
    let img =
        Image(VerticalAlignment=VerticalAlignment.Center, Margin=Thickness(0.0,0.0,2.0,0.0)) 
        |> addChildren stack

    let text = TextBlock(VerticalAlignment=VerticalAlignment.Center) |> addChildren stack
    let mutable srcSelected, srcUnselected = null, null

    member t.Text
        with get() = text.Text
        and set v = text.Text <- v

    member t.SelectedImage
        with get() = srcSelected
        and set(v: ImageSource) =
            srcSelected <- v
            if t.IsSelected then
                img.Source <- v

    member t.UnselectedImage
        with get() = srcUnselected
        and set(v: ImageSource) =
            srcUnselected <- v
            if t.IsSelected = false then
                img.Source <- v

    override t.OnSelected args =
        base.OnSelected args
        img.Source <- srcSelected

    override t.OnUnselected args =
        base.OnUnselected args
        img.Source <- srcUnselected

type DirectoryTreeViewItem(dir: DirectoryInfo) as t =
    inherit ImagedTreeViewItem(Text=dir.Name)
    do
        t.SelectedImage <- loadImageDir "OPENFOLD.BMP"
        t.UnselectedImage <- loadImageDir "CLSDFOLD.BMP"

    member t.Populate() =
        try
            let dirs = dir.GetDirectories()
            for dir in dirs do
                DirectoryTreeViewItem dir |> addItems t |> ignore
        with e -> ()

    member t.DirectoryInfo = dir

    override t.OnExpanded args =
        base.OnExpanded args
        for x in t.Items do
            let item = x :?> DirectoryTreeViewItem
            item.Populate()

type DirectoryTreeView() as t =
    inherit TreeView()

    do t.RefreshTree()

    member t.RefreshTree() =
        t.BeginInit()
        t.Items.Clear()
        let drives = DriveInfo.GetDrives()
        for drive in drives do
            let chDrive = drive.Name.ToUpper().[0]
            let item = DirectoryTreeViewItem(drive.RootDirectory)

            // Display VolumeLabel if drive ready; otherwise just DriveType.
            if chDrive <> 'A' && drive.IsReady && drive.VolumeLabel.Length > 0 then
                item.Text <- sprintf "%s(%s)" (drive.VolumeLabel) (drive.Name)
            else
                item.Text <- sprintf "%s(%s)" (drive.DriveType.ToString()) (drive.Name)

            if chDrive = 'A' || chDrive = 'B' then
                let x = loadImageDir "35FLOPPY.bmp"
                item.SelectedImage <- x; item.UnselectedImage <- x
            elif drive.DriveType = DriveType.CDRom then
                let x = loadImageDir "CDDRIVE.bmp"
                item.SelectedImage <- x; item.UnselectedImage <- x
            else
                let x = loadImageDir "DRIVE.bmp"
                item.SelectedImage <- x; item.UnselectedImage <- x

            t.Items.Add item |> ignore

            // Populate the drive with directories.
            if chDrive <> 'A' && chDrive <> 'B' && drive.IsReady then
                item.Populate()
        t.EndInit()
        

[<STAThreadAttribute>]
try 
    let grid = Grid()
    let win = Window(Title = "Recurse Directories Incrementally", Content = grid)

    [|
    GridLength(50.0,GridUnitType.Star)
    GridLength.Auto
    GridLength(50.0,GridUnitType.Star)
    |] |> Array.iter(fun x -> ColumnDefinition(Width=x) |> grid.ColumnDefinitions.Add)

    let tree = DirectoryTreeView() |> addGrid grid 0 0
    let split = GridSplitter(Width=6.0,ResizeBehavior=GridResizeBehavior.PreviousAndNext) |> addGrid grid 0 1
    let stack = StackPanel()
    let scroll = ScrollViewer(Content=stack) |> addGrid grid 0 2

    tree.SelectedItemChanged.Add <| fun args ->
        let item = args.NewValue :?> DirectoryTreeViewItem
        stack.Children.Clear()
        try
            let infos = item.DirectoryInfo.GetFiles()
            for info in infos do
                TextBlock(Text=info.Name) |> addChildren stack |> ignore
        with e -> ()

    win.Show()
    //Application().Run(win) |> ignore
with e -> MessageBox.Show(e.Message,"Exception Error") |> ignore
