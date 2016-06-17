-- This version is only 3x better than the previous one that used immutable updates.
-- N^2 / 2 is not cutting it for this problem.

-- I'll have to do a double pass again.

module Main where
  import Control.Monad
  import Control.Monad.ST
  import Control.Applicative ((<$>))
  import Data.Vector.Unboxed (Vector,(!))
  import qualified Data.Vector.Unboxed as V
  import qualified Data.Vector.Unboxed.Mutable as VM
  import Debug.Trace

  x |> f = f x

  solve :: Vector Int -> Vector Int
  solve ar =
    (\(x,_,_) -> x) $
    V.foldl' go (V.replicate (V.length ar) 0, V.head ar, 1) (V.tail ar) where
      go :: (Vector Int, Int, Int) -> Int -> (Vector Int, Int, Int)
      go (s,previous_price,i) price = runST $ do
        let diff = price-previous_price
        s' <- VM.new (V.length ar)
        VM.write s' 0 (V.maximum s)
        forM_ [1..i-1] $ \x ->
          VM.write s' x (max ((s ! (x-1)) + x*diff) ((s ! x) + x*diff))
        VM.write s' i ((s ! (i-1)) + i*diff)
        s' <- V.unsafeFreeze s'
        return (s',price,i+1)

  main = do
    t <- fmap read getLine
    unlines . map (show . V.maximum . solve . V.fromList . map read . words)
      <$> replicateM t (getLine >> getLine) >>= putStr
    -- map (solve . V.fromList . map read . words)
    --   <$> replicateM t (getLine >> getLine) >>= print
