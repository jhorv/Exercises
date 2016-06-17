// Combinatorics never fails to impress. I forgot to account for all the combinations of the paths. Damn.

open System

let num_test_cases = Console.In.ReadLine() |> int
    
for i = 1 to num_test_cases do
    let readIntLine() = 
        Console.In.ReadLine().Split [|' '|]
        |> Array.choose (fun x -> if x <> "" then Some (int x) else None)
    let [|_; m|] = readIntLine()
    let x = readIntLine()
    let d = readIntLine()
    if x.Length <> d.Length then failwith "x and d have to have the same lengths!"

    let prime = 1000000007UL
    let inline modi x = x % prime

    let dp1Dacc (ar : uint64[]) (m : int) =
        let inline read (ar : uint64[]) i =
            let inline r i =
                if i >= 0 && i < ar.Length then ar.[i] else 0UL
            r (i-1) + r (i+1) |> modi
        
        let f (ar : uint64[], i) =
            if i <= m 
            then 
                let next_state = Array.init ar.Length (read ar), i+1
                let sum = Array.fold (fun s x -> s+x |> modi) 0UL
                (sum ar, next_state) |> Some 
            else None
        Array.unfold f (ar, 0)

    let ar = 
        [|
        for i=0 to x.Length-1 do
            let t = Array.create d.[i] 0UL
            t.[x.[i]-1] <- 1UL
            yield dp1Dacc t m
        |]

    let C =
        let ar = ResizeArray()
        ar.Add(Array.create 1 1UL)

        for up=1 to m do
            let f i =
                let r i =
                    if i >= 0 && i < up then ar.[up-1].[i] else 0UL
                r (i-1) + r i |> modi
            ar.Add(Array.init (up+1) f)
        ar.ToArray()

    let solution =
        let make_next_possible_move_ar (posm1 : uint64[]) (posm2 : uint64[]) =
            let number_of_possible_moves_at m = 
                let f s i =
                    let j = m - i
                    let ( *** ) x y = x * y |> modi 
                    let ( +++ ) x y = x + y |> modi
                    s +++ C.[m].[i]***posm1.[i]***posm2.[j]
                Seq.fold f 0UL {0..m}
            Array.init (m+1) number_of_possible_moves_at
        Array.reduce make_next_possible_move_ar ar
        |> Array.last

    printfn "%i" solution

