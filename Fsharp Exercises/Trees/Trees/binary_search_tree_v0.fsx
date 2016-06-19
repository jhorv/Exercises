// Well, this was easy.

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

let a = Node(Empty,2,Empty).insert(1).insert(3).insert(0).insert(10).insert(8).insert(5).insert(11).insert(-1)

a.depth