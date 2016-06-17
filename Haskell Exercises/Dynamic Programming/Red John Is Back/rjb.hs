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
  import Control.Monad.Primitive
  import Data.STRef
  import Debug.Trace
  import Data.Int

  parse :: ByteString -> [Int]
  parse = L.unfoldr $ C.readInt . C.dropWhile (== ' ')

  config :: Int -> Int
  config 0 = 0
  config m = f m where
    f x
      | x > 0 = l (x-1) + l (x-4)
      | x == 0 = 1
      | otherwise = 0
    l x
      | x >= 0 && x <= m = ar VB.! x
      | otherwise = 0
    ar = VB.generate (m+1) f

  sieve :: Int -> Int
  sieve c
    | c > 0 = runST $ do
      s <- VM.replicate (c+1) (1 :: Int8)
      tot <- newSTRef 0
      forM_ [2..c] $ \i -> do
        r <- VM.read s i
        when (r == 1) $ do
          modifySTRef' tot (+1)
          forM_ [i,i+i..c] $ \j ->
            VM.write s j 0
      readSTRef tot
    | otherwise = 0

  main :: IO ()
  main = do
    [n] <- parse <$> B.getLine
    replicateM_ n $ B.getLine >>= print . sieve . config . head . parse
