-- As check.fsx indicates, it seems there is a problem in the cost function currently.
-- I'll copy the program here and pare it down so I could test it.

module Main where
  import System.Random
  import Control.Monad

  import qualified Debug.Trace as Tr

  import qualified Data.Vector.Unboxed as V
  import qualified Data.Vector.Unboxed.Mutable as VM
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
                in profit -- + decay_factor' * price_of_blimp_at_city city
                  - distance_travelled * (1 + (fromIntegral blimps_in_possession) * blimp_cost_per_miles)

              total_cities_visited' = if city /= 0 then total_cities_visited - 1 else total_cities_visited
              blimps_in_possession' = if city /= 0 then blimps_in_possession + 1 else 0
              prev_city' = city
              in (decay_factor',total_cities_visited',blimps_in_possession',profit',prev_city')
          number_of_cities_visited = V.length path - number_of_visits_to_headquarters
          number_of_declines = number_of_cities_visited `div` tenth_of_cities
          decay_factor = factor_of_decline_fun V.! number_of_declines
          in
            V.scanr' costOfReversedPath' (decay_factor,number_of_cities_visited,0,0,V.last path) path
            -- |> costOfReversedPath' 0 -- Adds an extra trip to HQ
            -- |> (\(_,_,_,profit,_) -> profit)
      in do
        print $ costOfReversedPath (V.fromList [0,2,4,5,6,7,0,3,10,9,0]) 3
