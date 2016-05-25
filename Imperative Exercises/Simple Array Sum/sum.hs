-- This would be trivial to do using a fold operation, but I need to do it in an
-- imperative fashion for those exp points.

module Main where

  import Control.Monad
  import Control.Monad.ST
  import Data.STRef

  main :: IO ()
  main = do
    _ <- getLine
    numbers <- liftM (map (read :: String -> Int) . words) getLine

    let
      imperative_sum :: [Int] -> Int
      imperative_sum ar = runST $ do
        acc <- newSTRef 0
        forM_ ar $ \a ->
          modifySTRef' acc (+ a)
        readSTRef acc
      in
        print $ imperative_sum numbers
