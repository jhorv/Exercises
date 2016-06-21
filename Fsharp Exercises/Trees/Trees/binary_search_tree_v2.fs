//

type Color = R | B

type RBTree<'a when 'a: comparison> =
    | Empty
    | Node of Color * 'a RBTree * 'a * 'a RBTree

    member t.is_member (x: 'a) =
        match t with
        | Empty -> false
        | Node(_,l,v,r) when x = v -> true
        | Node(_,l,v,r) when x < v -> l.is_member x
        | Node(_,l,v,r) -> r.is_member x
        

    member t.insert (x: 'a) =
        let rec ins t =
            let balance = function
                | Node(B,Node(R,Node(R,a,x,b),y,c),z,d)
                | Node(B,Node(R,a,x,Node(R,b,y,c)),z,d)
                | Node(B,a,x,Node(R,Node(R,b,y,c),z,d))
                | Node(B,a,x,Node(R,b,y,Node(R,c,z,d))) ->
                    Node(R,Node(B,a,x,b),y,Node(B,c,z,d))
                | _ as x -> x
            match t with
            | Empty -> Node(R,Empty,x,Empty)
            | Node(c,l,k,r) as v when x = k -> v
            | Node(c,l,k,r) when x < k -> 
                Node(c,ins l,k,r) |> balance
            | Node(c,l,k,r) -> 
                Node(c,l,k,ins r) |> balance

        ins t |> function
            | Node(_,l,x,r) as n -> Node(B,l,x,r)
            | _ -> failwith "impossible to reach this point after insertion"

    member t.depth =
        let rec go t (i: int) =
            match t with
            | Empty -> i
            | Node(_,l,_,r) -> max (go l (i+1)) (go r (i+1))
        go t 0

let size = 1000000

let stopwatch = System.Diagnostics.Stopwatch.StartNew()

let a = (Node(B,Empty,0,Empty), {1..size}) ||> Seq.fold (fun s x -> s.insert x) 
let a_depth = a.depth

let b = 
    let t = 
        let rng = System.Random()
        [|1..size|] |> Array.map (fun x -> rng.NextDouble(),x) |> Array.sortBy fst |> Array.map snd // Sort shuffle
    (Node(B,Empty,0,Empty), t) ||> Array.fold (fun s x -> s.insert x) // This might be the first time I actually used the ||> operator. It helps type inference.

let b_depth = b.depth

let test (a: RBTree<_>) =
    for i=0 to size do
        if a.is_member i = false then failwith "element not found!"

test a
test b

printfn "Time elapsed: %A" stopwatch.Elapsed
printfn "Depth of a = %i" a.depth
printfn "Depth of b = %i" b.depth

// Compare it with F#'s builtin set.

stopwatch.Restart()
let s = 
    let t = 
        let rng = System.Random()
        [|1..size|] |> Array.map (fun x -> rng.NextDouble(),x) |> Array.sortBy fst |> Array.map snd // Sort shuffle
    let s = (Set.singleton 0, t) ||> Array.fold (fun s x -> Set.add x s) // This might be the first time I actually used the ||> operator. It helps type inference.
    for i=0 to size do
        if Set.contains i s = false then failwithf "element %i not found!" i
    s

printfn "Time elapsed for the F# set: %A" stopwatch.Elapsed