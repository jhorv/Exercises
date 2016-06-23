﻿// I found this on the net. I do not understand it.

open System.Drawing

type IQuadable = 
    abstract member Bounds : Rectangle

type SubNodes = { NW: QuadNode; NE: QuadNode; SE: QuadNode; SW: QuadNode }
and QuadNode = { Bounds: Rectangle; mutable Contents: List<IQuadable>; SubNodes: Option<SubNodes> }

module QuadTree =
    let contains (tree: QuadNode) (bounds: Rectangle) = tree.Bounds.Contains bounds

    let getConstrainingNode (tree: QuadNode) (elementBounds: Rectangle) : QuadNode = 
        let rec contrainingRec currentNode = 
            match currentNode.SubNodes with
            | None -> currentNode
            | Some (subTree) -> 
                [ subTree.NE; subTree.NW; subTree.SE; subTree.SW ]
                |> List.filter (fun n -> n.Bounds.Contains elementBounds)
                |> function 
                   | [] -> currentNode
                   | head :: [] -> head
                   | _ -> currentNode
        contrainingRec tree

    let add (tree: QuadNode) (element: IQuadable) =
        let target = getConstrainingNode tree element.Bounds
        target.Contents <- element :: target.Contents
        tree
             
    let init (elements: IQuadable list) (depth: int) =
        let maxBounds = elements
                        |> List.map (fun x -> x.Bounds) 
                        |> List.fold (fun a r -> Rectangle.Union(a, r)) Rectangle.Empty
        let rec buildTree nodeBounds = 
            function
            | 0 -> { Bounds = nodeBounds; Contents = List.empty; SubNodes = None }
            | curDepth -> let midPoint = Point(nodeBounds.Width - nodeBounds.X, nodeBounds.Height - nodeBounds.Y) in
                            { Bounds = nodeBounds; 
                              Contents = List.empty;
                              SubNodes = Some { NW = buildTree <| Rectangle.FromLTRB(nodeBounds.X, nodeBounds.Y, midPoint.X, midPoint.Y) <| curDepth - 1;
                                                SW = buildTree <| Rectangle.FromLTRB(nodeBounds.X, midPoint.Y, midPoint.X, nodeBounds.Y) <| curDepth - 1;
                                                NE = buildTree <| Rectangle.FromLTRB(midPoint.X, nodeBounds.Y, nodeBounds.X, midPoint.Y) <| curDepth - 1;
                                                SE = buildTree <| Rectangle.FromLTRB(midPoint.X, midPoint.Y, nodeBounds.X, nodeBounds.Y) <| curDepth - 1; } }
 
        let emptyTree = buildTree maxBounds depth
        List.fold (add) emptyTree elements
