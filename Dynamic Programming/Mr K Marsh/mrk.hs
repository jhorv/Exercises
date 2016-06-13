{-# LANGUAGE ScopedTypeVariables #-}

module Main where
  import Control.Applicative ((<$>))
  import Data.ByteString (ByteString)
  import qualified Data.ByteString as B
  import qualified Data.ByteString.Char8 as C
  import qualified Data.Vector.Unboxed as V
  import qualified Data.Vector as VB
  import qualified Data.List as L
  import Data.Char
  import Debug.Trace

  (|>) :: a -> (a -> b) -> b
  x |> f = f x

  main :: IO ()
  main =
    let
        parse (s :: ByteString) = s |> L.unfoldr (C.readInt . C.dropWhile (== ' '))
        pr (i :: Int) = if i <= 0 then "impossible" else show i
        solve (l :: Int) (ars :: VB.Vector ByteString) =
          let
              (bijective_loop :: V.Vector (Int, Int)) = V.fromList [(x,y) | x <- [0..l-1], y <- [x+1..l-1]]
          in
              ars
              |> VB.map (\x ->
                V.generate l (\i ->
                  let c '.' = 1
                      c 'x' = 0
                  in  (x `B.index` i) |> fromEnum |> chr |> c)
                |> V.postscanl' (\l r -> if r == 1 then l+r else 0) 0)
              |> (\x -> x :: VB.Vector (V.Vector Int))
              |> VB.foldl' (\(l :: V.Vector (Int, Int)) (r :: V.Vector Int) ->
                  let (r' :: V.Vector (Bool, Bool)) =
                        V.map (\(i1,i2) ->
                          let p1 = r V.! i1
                              p2 = r V.! i2
                          in (i2-i1<p2,p1 == 0 || p2 == 0)
                          ) bijective_loop
                  in
                      V.zipWith (\(m, s) (c1 ,c2) ->
                        -- This code acts as a state machine
                        -- The state is determined by the length of the field

                        -- Go to the empty state if one of the endpoints is a marsh
                        if c2 then (m, 0)
                          -- If in empty state and no marshes inbetween, start tracking if there are no marshes inbetween
                        else if s == 0 then let x = fromEnum c1 in (max m x, x)
                        -- Increase the state by one and modify the max as there are no marshes in between
                        else if c1 then let x = s+1 in (max m x, x)
                        -- Increments the state by one, but does not modify the max because there are marshes inbetween
                        else let x = s+1 in (m, x)
                      ) l r'
                ) (V.replicate (V.length bijective_loop) (0,0))
              |> V.zipWith (\(i1,i2) (m,_) ->
                if m >= 2 then 2*(i2-i1)+2*(m-1) else -1
                ) bijective_loop
              |> V.maximum

    in do
        [n,l :: Int] <- parse <$> B.getLine
        (ars :: VB.Vector ByteString) <- VB.replicateM n B.getLine
        ars |> solve l
            |> pr
            |> putStr
