module Main where
  import Data.IORef
  import Control.Monad
  import Debug.Trace

  main :: IO ()
  main = do
    _ <- liftM (read :: String -> Int) getLine
    numbers <- liftM (map (read :: String -> Int) . words) getLine
    let
      plus_minus :: [Int] -> IO String
      plus_minus ar = do
        pos <- newIORef 0 :: IO (IORef Int)
        neg <- newIORef 0
        zer <- newIORef 0
        forM_ ar $ \x ->
          case () of _
                      | x > 0 -> modifyIORef' pos (+1)
                      | x < 0 -> modifyIORef' neg (+1)
                      | x == 0 -> modifyIORef' zer (+1)
        r1 <- readIORef pos
        r2 <- readIORef neg
        r3 <- readIORef zer
        let
          tot :: Double
          tot = fromIntegral (r1+r2+r3)
          r1' = show $ fromIntegral r1 / tot
          r2' = show $ fromIntegral r2 / tot
          r3' = show $ fromIntegral r3 / tot
          in return $ unlines [r1', r2', r3']


      in do
        s <- plus_minus numbers
        putStr s
