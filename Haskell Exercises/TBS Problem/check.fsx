open System.Collections.Generic

let blimp_cost, decay_factor = 3.0f, 0.95f
let blimp_price = Dictionary(HashIdentity.Structural)
blimp_price.Add((0,0),0)
let cities =
    [|
    1; 1; 30;
    2; 2; 35;
    0; 8; 50;
    7; 2; 35;
    7; 3; 40;
    10; 7; 90;
    9; 8; 35;
    5; 15; 10;
    8; 18; 15;
    1; 9; 60;
    |]
    |> Array.chunkBySize 3
    |> Array.iter (fun [|x;y;c|] -> blimp_price.Add((x,y),c))

let path =
    [|
    1; 1; 6;
    2; 2; 5;
    7; 2; 4;
    7; 3; 3;
    10; 7; 2;
    9; 8; 1;
    0; 0; 0;
    0; 8; 4;
    1; 9; 3;
    5; 15; 2;
    8; 18; 1;
    0; 0; 0;
    |]
    |> Array.chunkBySize 3

let cost (path : int [][]) =
    Array.scan (fun (x:int,y:int,c,f) [|x';y';p|] ->
        let distance_cost = (x-x')*(x-x') + (y-y')*(y-y') |> float32 |> sqrt |> fun x -> x+x*(float32 p)*blimp_cost
        let blimp_sale_profit = (blimp_price.[(x',y')] |> float32)*f
        (x',y',c-distance_cost+blimp_sale_profit,if x' <> 0 && y' <> 0 then f*decay_factor else f)
        ) (0,0,0.0f,1.0f) path

let r = cost path
