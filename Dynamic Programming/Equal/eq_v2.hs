-- This solution just indexes into a precalculated array. O(N) right there.

{-# LANGUAGE ScopedTypeVariables #-}

module Main where
  import Control.Applicative ((<$>))
  import Data.ByteString (ByteString)
  import Data.Vector(Vector)
  import qualified Data.ByteString as B
  import qualified Data.ByteString.Char8 as C
  import qualified Data.Vector as VB
  import qualified Data.List as L
  import Data.Char
  import Debug.Trace

  (|>) :: a -> (a -> b) -> b
  x |> f = f x

  main :: IO ()
  main =
    let
        parse (s :: ByteString) = s |> VB.unfoldr (C.readInt . C.dropWhile (== ' '))
        solve (ar :: Vector Int) =
          let mini = 5 :: Int
          in
              VB.minimum ar
              |> (\minimum_ar ->
                VB.map (\x -> x - minimum_ar + mini) ar)
              |> (\ar -> (VB.maximum ar, ar))
              |> (\(maxi, m) ->
                let
                    (ar :: Vector Int) = VB.generate (maxi+1) f
                    f i
                      | i == maxi = 0
                      | i < maxi =
                        min (l (i+1)) (l (i+2))
                        |> min (l (i+5))
                        |> (+1)
                    l i = if i >= 0 && i <= maxi then ar VB.! i else 666
                in
                    [ VB.map (\x -> ar VB.! (maxi - (x - mini + i))) m
                      |> VB.sum | i <- [0..mini]]
                    |> minimum)

    in do
        n <- (VB.head . parse) <$> B.getLine
        (ars :: Vector (Vector Int)) <- VB.replicateM n (parse <$> (B.getLine >> B.getLine))
        ars |> VB.map (show . solve)
            |> VB.toList
            |> unlines
            |> putStr
