-- Too innefficient memorywise. Only passes 4/12 tets on HakcerRank.

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
  import Data.Word
  import Debug.Trace


  parse :: ByteString -> [Int]
  parse = unfoldr $ C.readInt . C.dropWhile (== ' ')

  modi x = x `mod` 1000000007

  solve :: Vector Word32 -> Vector Word32 -> Word32 -> Word32
  solve cords dims' m =
    let
      dims :: Vector Word32
      dims = V.scanl' (*) 1 dims'
      decodeDigit dig =
        V.generate (V.length dims') (\x -> (dig `mod `(dims V.! (x+1))) `div` (dims V.! x))
      decodeDim dig dim =
        (dig `mod` (dims V.! (dim+1))) `div` (dims V.! dim)
      encodeDigit :: Vector Word32 -> Word32
      encodeDigit =
        V.ifoldl' (\x i e -> x + e*(dims V.! i)) 0
      getAdjacentsOfDigit :: Word32 -> Vector Word32
      getAdjacentsOfDigit dig =
        let
          dec = decodeDigit dig
          --dec' = zip (V.toList dec) [0..]
          gen (y, i') =
            V.generate (V.length dec) (\i ->
              if i == i' then y else dec V.! i)
          mapF = V.map (encodeDigit . gen)
          mapUp = V.imap (\i x -> (x+1,i))
          filUp = V.filter (\(y,i) -> y < (dims' V.! i))
          mapDown = V.imap (\i x -> (x-1,i))
          filDown = V.filter (\(y,_) -> y /= maxBound)
          in mapF (filUp (mapUp dec) V.++ filDown (mapDown dec))
      startingAr :: Vector Word32
      startingAr =
        let
          start = encodeDigit cords
          in
            V.generate (fromIntegral $ V.last dims) (\i ->
              if i == fromIntegral start then 1 else 0)
      -- sums up the adjacenets for each element
      sumAdjacents :: Vector Word32 -> Vector Word32 -> Word32
      sumAdjacents ar = V.foldl' (\s x -> modi $ s + (ar V.! fromIntegral x)) 0
      newAr prevAr =
        V.generate (fromIntegral $ V.last dims) (sumAdjacents prevAr . getAdjacentsOfDigit . fromIntegral)
      --allDigits = [(getAdjacentsOfDigit x, decodeDigit x, x) | x <- [0..V.last dims-1]]
      r = L.foldl' (\prevAr _ -> newAr prevAr) startingAr [1..m]
      in V.foldl' (\s x -> modi $ s + x) 0 r

  main = do
    [num_test_cases] <- parse <$> B.getLine
    s <-
      forM [1..num_test_cases] $ \_ -> do
        [_,m] <- parse <$> B.getLine
        x <- V.fromList . map (\x -> fromIntegral $ x-1) . parse <$> B.getLine
        d <- V.map fromIntegral . V.fromList . parse <$> B.getLine
        return $ solve x d (fromIntegral m)
    putStr $ unlines $ map show s
