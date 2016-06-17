module Main where
  import Data.IORef
  import Control.Monad

  main :: IO ()
  main = do
    n <- liftM (read :: String -> Int) getLine
    --numbers <- liftM (map (read :: String -> Int) . words) getLine
    let
      staircase :: IO String
      staircase = do
        s <- newIORef ""
        forM_ (reverse [0..n-1]) $ \x -> do
          modifyIORef' s (++ replicate x ' ')
          modifyIORef' s (++ replicate (n-x) '#')
          modifyIORef' s (++ "\n")
        readIORef s
      in do
        s <- staircase
        putStr s
