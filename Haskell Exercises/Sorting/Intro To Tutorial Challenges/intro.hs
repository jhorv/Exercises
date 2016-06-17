module Main where
  import Control.Monad
  import Debug.Trace
  import Data.List

  (|>) :: a -> (a -> b) -> b
  x |> f = f x

  main :: IO ()
  main = do
    n' <- getLine
    _ <- getLine
    ar' <- getLine
    let
      n = read n' :: Int
      ar :: [Int]
      ar = words ar' |> map read
      in do
        print $ (\(Just x) -> x) $ findIndex (==n) ar
