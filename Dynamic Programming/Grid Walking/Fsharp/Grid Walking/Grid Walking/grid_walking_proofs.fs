// A bunch of experiments that I did to get a grip on the HackerRank Grid Walking problem.
// I never tried proving things in this fashion, but there is a first time for everything.

let x1,x2 = 0,0
let d1,d2 = 4,3

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

let t = dp2D ar 3 

// Proof that sum (dp1D (sumrow t) 1) + sum (dp1D (sumcol t) 1) = sumall (dp2D t 1)
let t2 = 
    let sum = Array.sum
    let r1 = sum (dp1D (sumrow t) 1) + sum (dp1D (sumcol t) 1)
    let r2 = sumall (dp2D t 1)
    r1 = r2

let tx x = 
    let sum = Array.sum
    let r1 = sum (dp1D (sumrow t) x) + sum (dp1D (sumcol t) x)
    let r2 = sumall (dp2D t x)
    r1 = r2

// Unfortunately the above is false for more than one step.
tx 2 = false

// The above is a good first start. But what I need to beat this problem is to linearly separate the dimensions and possibly stitch them back again.
// So for that, I need to prove that ar.[i,j] = ar1.[i] + ar2.[j] where ar1 and ar2 are ???

// ...

// How about this? (sumrow ar).[i] = ar1.[i] + sum ar2 and (sumrow ar).[j] = sum ar1 + ar2.[j] ?
// ...

let s1 =
    let sum = Array.sum
    let r = sumrow t
    let c = sumcol t
    let r' = dp1D r 1
    let c' = dp1D c 1

    let z = dp2D t 1
    let zr = sumrow z
    let zc = sumcol z
    let s = sum r' + sum c'
    let s' = (s, sum zr, sum zc)
    (r', c', zr, zc), s', ((r, r', sum r'), (c, c', sum c')), z
