module Main where
  import Data.Foldable
  import Control.Monad

  x |> f = f x

  maxSum :: [Int] -> (Int, Int)
  maxSum ar =
    foldl' (\(mc,mnc,c,nc) e ->
      let
        c' = max (c + e) e
        nc' = max (nc + max e 0) e
        in (max mc c', max mnc nc', c', nc')
    ) (head ar,head ar,head ar,head ar) (tail ar)
    |> \(mc,mnc,_,_) -> (mc,mnc)


  main :: IO ()
  main = do
    n <- fmap read getLine
    r <-
      forM [1..n] $ \i -> do
        _ <- getLine
        ar <- fmap (map read . words) getLine
        return $ maxSum ar
    putStr $ unlines $ map (\(mc,mnc) -> show mc ++ " " ++ show mnc) r
