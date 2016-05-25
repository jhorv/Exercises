module Main where
  import Data.IORef
  import Control.Monad
  import Debug.Trace

  main :: IO ()
  main = do
    n <- liftM (read :: String -> Int) getLine
    numbers <- liftM (map (map (read :: String -> Int) . words) . lines) getContents
    let
      diagonal_sum :: [[Int]] -> IO Int
      diagonal_sum ar = do
        s1 <- newIORef 0
        s2 <- newIORef 0
        forM_ [0..n-1] $ \i -> do
          modifyIORef' s1 (+ (ar !! i !! i))
          modifyIORef' s2 (+ (ar !! i !! (n-i-1)))
        r1 <- readIORef s1
        r2 <- readIORef s2
        return $ abs $ r1-r2
      in do
        s <- diagonal_sum numbers
        print s
