// This quadtree is quite nicer that the snippet.
// The question is what do I do with it?

type QuadTree =
    | Empty
    | Leaf of x: float32 * y: float32
    | Fork of x1: float32 * x2: float32 * y1: float32 * y2: float32 * nw: QuadTree * ne: QuadTree * sw: QuadTree * se: QuadTree

    // Unlike standard trees, quadtrees should start from a Fork root
    static member create_empty(x1,x2,y1,y2) =
        if x1 >= x2 && y1 >= y2 then failwithf "x1(%A) >= x2(%A) && y1(%A) >= y2(%A)" x1 x2 y1 y2
        Fork(x1,x2,y1,y2,Empty,Empty,Empty,Empty)

    member t.insert(x,y as q) =
        let insert (x,y as q) (t: QuadTree) = t.insert q

        // The arguments for insert' are boundaries.
        let rec insert' (xy: (float32*float32*float32*float32) option) (t: QuadTree) =
            let insert'' xy = Some xy |> insert'
            match t with
            | Empty -> Leaf(x, y)
            | Leaf(x',y') -> 
                if x' = x && y' = y then failwith "Cannot insert an item with the same coordinates into a quadtree."

                let x1, x2, y1, y2 = xy.Value
                Fork(x1,x2,y1,y2,Empty,Empty,Empty,Empty) |> insert (x', y') |> insert (x, y)
            | Fork(x1,x2,y1,y2,nw,ne,sw,se) ->
                if x >= x1 && x < x2 && y >= y1 && y < y2 then
                    let mid_x, mid_y = (x1+x2)/2.0f, (y1+y2)/2.0f
                    let left, up = x < mid_x, y < mid_y

                    match left, up with
                    | true, true -> Fork(x1,x2,y1,y2,nw |> insert'' (x1,mid_x,y1,mid_y),ne,sw,se)
                    | false, true -> Fork(x1,x2,y1,y2,nw,ne |> insert'' (mid_x,x2,y1,mid_y),sw,se)
                    | true, false -> Fork(x1,x2,y1,y2,nw,ne,sw |> insert'' (x1,mid_x,mid_y,y2),se)
                    | false, false -> Fork(x1,x2,y1,y2,nw,ne,sw,se |> insert'' (mid_x,x2,mid_y,y2))
                else failwithf "Cannot insert(%A,%A) outside the boundary(%A,%A,%A,%A)." x y x1 x2 y1 y2

        insert' None t    

let a = 
    QuadTree
        .create_empty(0.0f,100.0f,0.0f,100.0f).insert(25.0f,25.0f)
        .insert(75.0f,25.0f).insert(25.0f,75.0f).insert(75.0f,75.0f)
        .insert(25.0f,45.0f)