type QuadTree =
    | Empty
    | Leaf of x: float32 * y: float32
    | Fork of x1: float32 * y1: float32 * x2: float32 * y2: float32 * nw: QuadTree * ne: QuadTree * sw: QuadTree * se: QuadTree

    
    member t.insert(x,y as q) =
        let insert (x,y as q) (t: QuadTree) = t.insert q

        match t with
        | Empty -> failwith "Cannot insert into empty directly, the root must be a fork."
        | Leaf _ -> failwith "Cannot insert into a leaf node directly as it does not have boundary information."
        | Fork(x1,y1,x2,y2,nw,ne,sw,se) ->
            if x >= x1 && x < x2 && y >= y1 && y < y2 then
                let mid_x, mid_y = (x1+x2)/2.0f, (y1+y2)/2.0f
                let left, up = x < mid_x, y < mid_y

                let split x1 x2 y1 y2 (Leaf(x,y) as l) =
                    let mid_x, mid_y = (x1+x2)/2.0f, (y1+y2)/2.0f
                    let left, up = x < mid_x, y < mid_y

                    match left, up with
                    | true, true -> Fork(x1,x2,y1,y2,l,ne,sw,se)
                    | false, true -> Fork(x1,x2,y1,y2,nw,l,sw,se)
                    | true, false -> Fork(x1,x2,y1,y2,nw,ne,l,se)
                    | false, false -> Fork(x1,x2,y1,y2,nw,ne,sw,l)

                match left, up with
                | true, true -> 
                    match nw with
                    | Empty -> Fork(x1,y1,x2,y2,Leaf q,ne,sw,se)
                    | Leaf _ -> Fork(x1,y1,x2,y2,nw |> split x1 mid_x y1 mid_y |> insert q,ne,sw,se)
                | false, true -> 
                    match nw with
                    | Empty -> Fork(x1,y1,x2,y2,nw,Leaf q,sw,se)
                    | Leaf _ -> Fork(x1,y1,x2,y2,nw,ne |> split mid_x x2 y1 mid_y |> insert q,sw,se)
                | true, false -> 
                    match nw with
                    | Empty -> Fork(x1,y1,x2,y2,nw,ne,Leaf q,se)
                    | Leaf _ -> Fork(x1,y1,x2,y2,nw |> split x mid_x y mid_y |> insert q,ne,sw,se)
                | false, false -> 
                    match nw with
                    | Empty -> Fork(x1,y1,x2,y2,nw,ne,sw,Leaf q)
                    | Leaf _ -> Fork(x1,y1,x2,y2,nw |> split x mid_x y mid_y |> insert q,ne,sw,se)
            else failwith "Cannot insert outside the boundary."
                
