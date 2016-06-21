// I had intended to translate OCaml modules here using abstract classes, but I realized that modules are really abstract 
// classes combined with unions so I cannot really do that. Instead I am going to use vanilla member functions.

// Edit: Done with the most difficult remove function. It was not particularly hard to grasp thanks to this:
// http://www.algolist.net/Data_structures/Binary_search_tree/Removal

type BinaryTree<'a when 'a: comparison> =
    | Empty
    | Node of 'a BinaryTree * 'a * 'a BinaryTree

    member t.insert (x: 'a) =
        match t with
        | Empty -> Node(Empty,x,Empty)
        | Node(l,k,r) as v when x = k -> v
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
        | Empty -> failwith "Key not found!"
        | Node(Empty,k,n) | Node(n,k,Empty) when x = k -> n
        | Node(l,k,r) when x = k -> 
            let rec split_leftmost = function
                | Empty -> failwith "Empty should not be called on."
                | Node(Empty,k,r) -> 
                    k, r
                | Node(l,k,r) ->
                    let k',n = split_leftmost l
                    k', Node(n,k,r)
            let k,n = split_leftmost r
            Node(l,k,n)
        | Node(l,k,r) when x < k -> Node(l.remove x,k,r)
        | Node(l,k,r) -> Node(l,k,r.remove x)

let a = Node(Empty,2,Empty).insert(1).insert(3).insert(0).insert(10).insert(8).insert(5).insert(11).insert(-1)
let b = a.remove(-1).remove(2).remove(8).remove(5).remove(10).remove(0).remove(1).remove(3).remove(11)
a.depth
b.depth

let c = Node(Empty,0,Empty).insert(2).insert(-2).insert(-3).insert(-1).insert(1).insert(3)
c.depth