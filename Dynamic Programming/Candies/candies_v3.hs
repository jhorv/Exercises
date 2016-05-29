module Main where
  import Control.Monad (replicateM)
  import Control.Applicative ((<$>))
  import Debug.Trace


  solve :: [Int] -> Int
  solve ar = 1 -- loop . zip  (repeat 0)
      where
      --loop :: [(Int, Int)] -> [(Int, Int)]
      --loop cs = (\(_,_,x) -> x 0 0) . foldr go (0, 0, \_ _ -> []) cs
      --t = (\(_,_,x) -> x 0 0) $ go (5,5) (1,1,\_ _ -> [(3,4)])
      go :: (Int, Int) -> (Int, Int, Int -> Int -> [Int]) -> (Int, Int, Int -> Int -> [Int])
      go (candy, score) (candyP,scoreP,f) =
        let
          candyP' = if scoreP < score then candyP + 1 else 1
          in
            (candyP', score,
              \candyN scoreN ->
                let
                  candy' = max candyP' $ if scoreN < score then candyN + 1 else 1
                  in candy' : f candy' score)
              -- s is the score and c is the candy of the guy before
              -- if s < score then this guy should get at least c + 1 candies


  main = do
      n <- read <$> getLine
      solve . fmap read <$> replicateM n getLine >>= print
