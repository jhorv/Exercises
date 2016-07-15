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

type DiskDirectory(dirinfo: DirectoryInfo) as t =
    
    member t.Name = dirinfo.Name
    
    member t.Subdirectories
        with get() = 
            let dirs = ResizeArray()
            try
                let subdirs = dirinfo.GetDirectories()
                for subdir in subdirs do
                    dirs.Add(DiskDirectory(subdir))
                dirs
            with e -> dirs

[<STAThreadAttribute>]
try 
    let treevue = TreeView()
    let win = Window(Title = "Template the Tree", Content = treevue)

    let factoryTextBlock = FrameworkElementFactory(typeof<TextBlock>) |> setBind' TextBlock.TextProperty (Binding "Name")
    let template = 
        HierarchicalDataTemplate(
                typeof<DiskDirectory>,ItemsSource=Binding "Subdirectories",
                VisualTree=factoryTextBlock)
    
    let dir = DiskDirectory(DirectoryInfo(Path.GetPathRoot(Environment.SystemDirectory)))

    TreeViewItem(Header=dir.Name,ItemsSource=dir.Subdirectories,ItemTemplate=template,IsExpanded=true)
    |> addItems treevue |> ignore


    win.Show()
    //Application().Run(win) |> ignore
with e -> MessageBox.Show(e.Message,"Exception Error") |> ignore

