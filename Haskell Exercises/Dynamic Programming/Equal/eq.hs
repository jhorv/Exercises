-- I magined this would be enough, but it times out unfortunately.
-- I guess there is a way to reduce this n^2 algorithm to n. Let me give it a try.

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
          let mini = 5
          in
              (VB.minimum ar - mini)
              |> (\m -> VB.map (\x -> x - m) ar)
              |> VB.map (\x ->
                let

                    (ar :: Vector Int) = VB.generate (x+1) f
                    f i
                      | i == x = 0
                      | i < x = min (l (i+1)) (l (i+2)) |> min (l (i+5)) |> (+1)
                    l i = if i >= 0 && i <= x then ar VB.! i else 666
                in
                    traceShow ar $ VB.slice 0 (mini+1) ar)
              |> VB.foldl1' (VB.zipWith (+))
              |> VB.minimum

    in do
        n <- (VB.head . parse) <$> B.getLine
        (ars :: Vector (Vector Int)) <- VB.replicateM n (parse <$> (B.getLine >> B.getLine))
        ars |> VB.map (show . solve)
            |> VB.toList
            |> unlines
            |> putStr
