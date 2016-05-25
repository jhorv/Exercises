-- This would be trivial to do using a fold operation, but I need to do it in an
-- imperative fashion for those exp points.

module Main where
  import Data.IORef
  import Control.Monad

  main :: IO ()
  main = do
    _ <- getLine
    numbers <- liftM (map (read :: String -> Integer) . words) getLine

    let
      imperative_sum :: [Integer] -> IO Integer
      imperative_sum ar = do
        acc <- newIORef 0
        forM_ ar $ \a ->
          modifyIORef' acc (+ a)
        readIORef acc
      in do
        s <- imperative_sum numbers
        print s
