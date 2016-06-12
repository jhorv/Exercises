module Main where
  import Control.Applicative ((<$>))
  import Control.Monad
  import Control.Monad.ST
  import Data.ByteString (ByteString)
  import qualified Data.ByteString as B
  import qualified Data.ByteString.Char8 as C
  import qualified Data.Vector.Unboxed as V
  import qualified Data.Vector.Unboxed.Mutable as VM
  import qualified Data.Vector as VB
  import qualified Data.List as L
  import Debug.Trace
  import Data.Int
  import Data.Word
  import Data.Char

  solve :: Int -> VB.Vector ByteString -> Int32
  solve l =
    let getTuple :: Int -> Int -> (Int, Int)
        getTuple n i =
          let (x0, y0) = (i `div` (n + 1), i `mod` (n + 1)) in
          if x0 < y0 then (x0, y0) else (n - 1 - x0, n - y0)
        w2c8 :: Word8 -> Char
        w2c8 = chr . fromEnum
        combinations :: Int -> ByteString -> V.Vector (Int32, Int32)
        combinations i ar =
          let l' = ((l-1)*l) `div` 2 in
          V.generate l' (\ j ->
            let (p1,p2) = getTuple (l-1) j
                e1 = ar `B.index` p1
                e2 = ar `B.index` p2
                c '.' '.' = (fromIntegral i,fromIntegral i)
                c _ _ = (maxBound, minBound) in
            c (w2c8 e1) (w2c8 e2)) in
    V.foldl1' max
    . V.imap (\i (min',max') ->
      let (p1',p2') = getTuple (l-1) i
          (p1,p2) = (fromIntegral p1', fromIntegral p2') in
      if min' /= maxBound && max' /= minBound && min' /= max'
      then 2*(p2-p1) + 2*(max'-min') else -1)
    . VB.foldl1' (V.zipWith (\(min1,max1) (min2,max2) -> (min min1 min2, max max1 max2)))
    . VB.imap combinations

  main :: IO ()
  main =
    let parse :: ByteString -> [Int]
        parse = L.unfoldr $ C.readInt . C.dropWhile (== ' ')
        pr :: Int32 -> String
        pr i = if i <= 0 then "impossible" else show i in do
    [n,l] <- parse <$> B.getLine
    VB.replicateM n B.getLine >>= putStr . pr . solve l
