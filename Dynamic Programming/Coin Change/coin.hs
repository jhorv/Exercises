module Main where
  import Data.Foldable hiding (concat)
  import Control.Monad
  import qualified Data.Map as M
  import Debug.Trace
  import Data.Maybe

  x |> f = f x

  coinChange :: Int -> [Int] -> Int
  coinChange n m =
    foldl' (\m c ->
      let -- doing this part with folds would have been a nightmare, so I used list comprehensions
        cs = [(c*i,1) | i <- [1..n `div` c]]
        ms = M.toList m
        t = [(c+m,mi) | (c,ci) <- cs, (m,mi) <- ms, c+m <= n]
        x = M.fromListWith (+) (concat [t,cs,ms])
        in x
    ) M.empty m
    |> \x -> fromMaybe 0 (M.lookup n x)

  main :: IO ()
  main = do
    [n,m] <- fmap (map read . words) getLine
    if n == 0 || m == 0
    then putStr "0"
    else do
      ar <- fmap (map read . words) getLine
      print $ coinChange n ar
