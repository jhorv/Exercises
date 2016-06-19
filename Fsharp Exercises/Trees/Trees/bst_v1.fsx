// I had intended to translate OCaml modules here using abstract classes, but I realized that modules are really abstract 
// classes combined with unions so I cannot really do that. Instead I am going to use vanilla member functions.

type BinaryTree<'a when 'a: comparison> =
    | Empty
    | Node of 'a BinaryTree * 'a * 'a BinaryTree

    member t.insert (x: 'a) =
        match t with
        | Empty -> Node(Empty,x,Empty)
        | Node(l,k,r) when x = k -> failwith "key is already in the tree"
        | Node(l,k,r) when x < k -> Node(l.insert x,k,r)
        | Node(l,k,r) -> Node(l,k,r.insert x)

    member t.depth =
        let rec go t (i: int) =
            match t with
            | Empty -> i
            | Node(l,_,r) -> max (go l (i+1)) (go r (i+1))
        go t 0

    member t.remove (x: 'a) =
        match t with
        | Empty -> Node(Empty,x,Empty)
        | Node(l,k,r) when x = k -> 
            if l.depth > r.depth then 
                match l with
                | Empty -> r
                | Node(l',k',r') -> Node(l'.insert r,k',r')
            else
                match r with
                | Empty -> l
                | Node(l',k',r') -> Node(l'.insert l,k',r')
        | Node(l,k,r) when x < k -> Node(l.remove x,k,r)
        | Node(l,k,r) -> Node(l,k,r.remove x)

