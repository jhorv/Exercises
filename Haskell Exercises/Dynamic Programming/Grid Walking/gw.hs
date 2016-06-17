module Main where
  import Data.List (unfoldr,foldl1')
  import Control.Applicative ((<$>))
  import Control.Monad
  import Data.ByteString (ByteString)
  import qualified Data.ByteString as B
  import qualified Data.ByteString.Char8 as C
  import Data.Vector.Unboxed (Vector)
  import qualified Data.Vector.Unboxed as V
  import qualified Data.Vector as VB
  import qualified Data.List as L


  parse :: ByteString -> [Int]
  parse = unfoldr $ C.readInt . C.dropWhile (== ' ')

  modi x = x `mod` 1000000007

  singleMoveDims :: Int -> Int -> Int -> V.Vector Int
  singleMoveDims x d m =
    let
      ar =
        let isI i = (i-1 == x && i-1 >= 0) || (i+1 == x && i+1 < d)
          in V.generate d (\i -> if isI i then 1 else 0)
      f = V.foldl' (\a b -> modi $ a+b) 0
      ars =
        V.unfoldrN (m-1)
          (\ar ->
            let
              ar' =
                V.generate d (\i ->
                  let
                    l = if i-1 >= 0 then ar `V.unsafeIndex` (i-1) else 0
                    r = if i+1 < d then ar `V.unsafeIndex` (i+1) else 0
                    in modi $ l+r
                  )
              in Just (f ar', ar')
          ) ar
      in V.cons (f ar) ars

  foldDims :: Vector Int -> Vector Int -> Vector Int
  foldDims x y = V.postscanl' (\s (l,r) -> modi $ s+l+r) 0 $ V.zip x y

  main = do
    [num_test_cases] <- parse <$> B.getLine
    s <-
      forM [1..num_test_cases] $ \_ -> do
        [_,m] <- parse <$> B.getLine
        x <- map (\x -> x-1) . parse <$> B.getLine
        d <- parse <$> B.getLine
        let
          dims = zipWith (\x d -> singleMoveDims x d m) x d
          folded_dims = L.foldl' (V.zipWith (+)) (V.replicate m 0) dims
          --folded_dims = foldl1' foldDims dims
          in return dims-- $ V.foldl1' (*) folded_dims
    print s
