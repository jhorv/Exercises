// Combinatorics never fails to impress. I forgot to account for all the combinations of the paths. Damn.

let x1,x2 = 0,0
let d1,d2 = 7,10
let m = 6

let inline modi x = x % 1000000007u

let dp2D (ar : uint32[,]) (m : int) =
    let inline read (ar : uint32[,]) i j =
        let inline r i j =
            if i >= 0 && j >= 0 && i < d1 && j < d2 then ar.[i,j] else 0u
        r (i-1) j + r (i+1) j + r i (j-1) + r i (j+1)
        |> modi

    Seq.fold (fun ar _ -> Array2D.init d1 d2 (read ar)) ar {1..m}

let dp1D (ar : uint32[]) (m : int) =
    let inline read (ar : uint32[]) i =
        let inline r i =
            if i >= 0 && i < ar.Length then ar.[i] else 0u
        r (i-1) + r (i+1) 
        |> modi

    Seq.fold (fun ar _ -> Array.init ar.Length (read ar)) ar {1..m}

let dp1Dacc (ar : uint32[]) (m : int) =
    let inline read (ar : uint32[]) i =
        let inline r i =
            if i >= 0 && i < ar.Length then ar.[i] else 0u
        r (i-1) + r (i+1) 
        |> modi

    Seq.scan (fun ar _ -> Array.init ar.Length (read ar)) ar {1..m}
    |> Seq.map Array.sum
    |> Seq.toArray

let sumrow (ar : uint32[,]) =
    let d1,d2 = Array2D.length1 ar, Array2D.length2 ar
    let f i = 
        let mutable s = 0u
        for j=0 to d2-1 do
            s <- s+ar.[i,j]
        s

    Array.init d1 f

let sumcol (ar : uint32[,]) =
    let d1,d2 = Array2D.length1 ar, Array2D.length2 ar
    let f j = 
        let mutable s = 0u
        for i=0 to d1-1 do
            s <- s+ar.[i,j]
        s

    Array.init d2 f

let sumall (ar : uint32[,]) =
    let mutable s = 0u
    for i=0 to d1-1 do
        for j=0 to d2-1 do
            s <- s+ar.[i,j]
    s

let ar = Array2D.create d1 d2 0u
ar.[x1,x2] <- 1u

let ar1 = Array.create d1 0u
ar1.[x1] <- 1u

let ar2 = Array.create d2 0u
ar2.[x2] <- 1u

let posm1 = dp1Dacc ar1 m
let posm2 = dp1Dacc ar2 m

let C =
    let ar = ResizeArray()
    ar.Add(Array.create 1 1u)

    for up=1 to m do
        let f i =
            let r i =
                if i >= 0 && i < up then ar.[up-1].[i] else 0u
            r (i-1) + r i |> modi
        ar.Add(Array.init (up+1) f)
    ar.ToArray()

let r =
    let s = m
    let mutable sum = 0u
    for i=0 to m do
        let j = s-i
        sum <- sum+C.[s].[i]*posm1.[i]*posm2.[j]
    sum
            
let r' = dp2D ar m |> sumall