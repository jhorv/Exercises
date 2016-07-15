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

[<STAThreadAttribute>]
try 
    let tree = TreeView()
    let win = Window(Title = "Manually Populate TreeView", Content = tree)

    let itemAnimal = TreeViewItem(Header="Animal") |> addItems tree

    let itemDog = TreeViewItem(Header="Animal") |> addItems itemAnimal
    [|"Poodle";"Irish Setter";"German Shepherd"|]
    |> Array.iter (ignore << itemDog.Items.Add)

    let itemCat = TreeViewItem(Header="Cat") |> addItems itemAnimal
    ([|"Chimpanzee";TreeViewItem(Header="Alley Cat");Button(Content="Noodles");"Siamese"|] : obj[])
    |> Array.iter (ignore << itemCat.Items.Add)

    let itemPrimate = TreeViewItem(Header="Primate") |> addItems itemAnimal
    ([|"Chimpanzee";"Bonobo";"Human"|] : obj[])
    |> Array.iter (ignore << itemPrimate.Items.Add)

    let itemMineral = TreeViewItem(Header="Mineral") |> addItems tree
    ([|"Calcium";"Zinc";"Iron"|] : obj[])
    |> Array.iter (ignore << itemMineral.Items.Add)

    let itemVegetable = TreeViewItem(Header="Vegetable") |> addItems tree
    ([|"Carrot";"Asparagus";"Broccoli"|] : obj[])
    |> Array.iter (ignore << itemVegetable.Items.Add)
    win.Show()
    //Application().Run(win) |> ignore
with e -> MessageBox.Show(e.Message,"Exception Error") |> ignore
