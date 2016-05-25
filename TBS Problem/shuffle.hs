module Main where
  import System.Random
  import qualified Data.Vector.Unboxed as V
  import qualified Data.Vector.Unboxed.Mutable as VM
  import Control.Monad

  -- | Randomly shuffle a list
  --   /O(N)/
  shuffle :: V.Vector Int -> IO (V.Vector Int)
  shuffle xs =
    let
      n = V.length xs
      in do
        ar <- V.thaw xs
        forM_ [0..n-1] $ \i -> do
          j <- randomRIO (i,n-1)
          vi <- VM.read ar i
          vj <- VM.read ar j
          VM.write ar j vi
          VM.write ar i vj
        V.freeze ar

  main :: IO ()
  main = do
    s <- shuffle $ V.fromList [1,2,3,4,5]
    print s
