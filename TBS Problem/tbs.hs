module Main where
  import GHC.Prim
  import Control.Monad
  import Control.Monad.ST
  import Control.Monad.Primitive

  import qualified Debug.Trace as Tr

  import qualified Data.Vector.Unboxed as V
  import Data.List

  (|>) :: a -> (a -> b) -> b
  x |> f = f x

  main :: IO ()
  main = do
    conts <- getContents
    let
      dat =
        conts
        |> lines
        |> map ((\[x1,x2,x3] -> (x1,x2,x3)) . map ((read :: String -> Float)) . words)
      (num_cities_f,blimp_cost_per_miles,factor_of_decline) = head dat

      factor_of_decline_fun :: V.Vector Float
      factor_of_decline_fun =
        V.scanl' (*) 1 (V.replicate 10 factor_of_decline)

      number_of_cities_including_hq = 1 + round num_cities_f :: Int
      tenth_of_cities = round $ num_cities_f / 10

      cities = V.fromList $ (0,0,0) : tail dat

      price_of_blimp_at_city x = (cities V.! x) |> (\(_,_,p) -> p)

      adjMat =
        let
          dist (x1,y1) (x2,y2) = sqrt ((x1-x2)*(x1-x2)+(y1-y2)*(y1-y2))
          in V.concatMap (\(x1,y1,_) -> V.map (\(x2,y2,_) -> dist (x1,y1) (x2,y2)) cities) cities

      adjMatAtInd x y =
        if x >= 0 && x < number_of_cities_including_hq && y >= 0 && y < number_of_cities_including_hq
        then adjMat `V.unsafeIndex` (x*number_of_cities_including_hq+y)
        else error "Out of bounds"

      -- By reversing the path, computing cost of travel for blimps becomes
      -- trivial as it would not require predetermination or lookahead.
      -- Apart from that, since the cost is being calculated from the end
      -- the cost decay factor has to work in reverse as well.

      -- I've complained about Haskell, but in the end, I feel like my ability
      -- to do function composition has really gone up on the past week and a
      -- half. I think I've finally internalized the fold as well.

      -- Of course, I already knew how to use fold, but I've never managed to
      -- put it into long term working memory up until now.
      -- I think I'll completely ditch the standard reduce in F# as it has led
      -- to bugs. Also, I've managed to internalize unfold as well.

      -- Figuring out this function has been agonizing for a while.
      -- In the end, ignorance often hides behind lethargy.

      -- For this function previously I would have used recursion.
      -- It is not even necessary to reverse the path, foldr' will do nicely.

      --costOfReversedPath :: V.Vector Int -> Int -> Float
      costOfReversedPath path number_of_visits_to_headquarters =
        let
          costOfReversedPath':: Int -> (Float,Int,Int,Float,Int) -> (Float,Int,Int,Float,Int)
          costOfReversedPath' city (decay_factor,total_cities_visited,blimps_in_possession,profit,prev_city) =
            let
              decay_factor' =
                if total_cities_visited `mod` tenth_of_cities == 0 && city /= 0
                then decay_factor / factor_of_decline
                else decay_factor

              profit' =
                let distance_travelled = adjMatAtInd prev_city city
                in profit + decay_factor' * price_of_blimp_at_city city
                  - distance_travelled * (1 + (fromIntegral blimps_in_possession) * blimp_cost_per_miles)

              total_cities_visited' = if city /= 0 then total_cities_visited - 1 else total_cities_visited
              blimps_in_possession' = if city /= 0 then blimps_in_possession + 1 else 0
              prev_city' = city
              in (decay_factor',total_cities_visited',blimps_in_possession',profit',prev_city')
          number_of_cities_visited = V.length path - number_of_visits_to_headquarters
          number_of_declines = number_of_cities_visited `div` tenth_of_cities
          decay_factor = factor_of_decline_fun V.! number_of_declines
          in
            V.foldr' costOfReversedPath' (decay_factor,number_of_cities_visited,0,0,V.last path) path
            |> costOfReversedPath' 0 -- Adds an extra trip to HQ
            |> (\(_,_,_,profit,_) -> profit)

      iterateTwoOpt :: (Float, V.Vector Int) -> (Float, V.Vector Int)
      iterateTwoOpt (cost,path) =
        let
          number_of_visits_to_headquarters =
            V.foldl' (\s x -> if x == 0 then s+1 else s) 0 path
          l = V.length path - 1

          -- Does not just swap the path, but reverses the middle as well.
          --
          twoOpt :: V.Vector Int -> Int -> Int -> V.Vector Int
          twoOpt path i j =
            let
              v1 = i
              v2 = j - i
              v3 = l - j + 1
              x1 = V.slice 0 v1 path
              x2 = V.slice i v2 path |> V.reverse
              x3 = V.slice j v3 path

              in --Tr.traceShow ([(0,v1),(i,v2),(j,v3),(l,v1+v2+v3),(l,V.length path)])
                V.concat [x1,x2,x3]
          in
            foldl' (\(cost,path) i ->
              foldl' (\(cost,path) j ->
                let
                  swapped_path = twoOpt path i j
                  swapped_cost = costOfReversedPath swapped_path number_of_visits_to_headquarters
                  in
                    if cost < swapped_cost
                    then (swapped_cost,swapped_path)
                    else (cost,path)
                ) (cost,path) [i+1..l]
              |> (\(swapped_cost,swapped_path) ->
                if cost < swapped_cost
                then (swapped_cost,swapped_path)
                else (cost,path)
                )
              ) (cost,path) [0..l-1]
      t :: V.Vector Int
      t = V.fromList [1,2,0,6,7,0,3,10]
      c = costOfReversedPath t 2
      in
        print $
          (c,t):
          unfoldr (\c@(cost,path) ->
            let n@(new_cost,new_path) = iterateTwoOpt c
              in
                if cost < new_cost
                then Just(n,n)
                else Nothing
            ) (c,t)
