module Main where
  import Control.Monad (replicateM)
  import Control.Applicative ((<$>))
  import Debug.Trace


  solve :: [Int] -> Int
  solve = sum . loop
      where
      loop :: [Int] -> [Int]
      loop = (\(_,_,x) -> x 0 0) . foldr go (0, 0, \_ _ -> [])
      go :: Int -> (Int, Int, Int -> Int -> [Int]) -> (Int, Int, Int -> Int -> [Int])
      go score (candyP,scoreP,f) =
        let
          candyP' = if scoreP < score then candyP + 1 else 1
          in
            (candyP', score,
              \candyN scoreN ->
                let
                  candy' = max candyP' $ if scoreN < score then candyN + 1 else 1
                  in candy' : f candy' score) -- This part could be replaced with a sum

  main = do
      n <- read <$> getLine
      (solve . fmap read) <$> replicateM n getLine >>= print
