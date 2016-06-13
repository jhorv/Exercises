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

  --solve :: Int -> VB.Vector ByteString -> Int32
  solve l =
    let l' = ((l-1)*l) `div` 2
        getTuple :: Int -> Int -> (Int, Int)
        getTuple n i =
          let (x0, y0) = (i `div` (n + 1), i `mod` (n + 1)) in
          if x0 < y0 then (x0, y0) else (n - 1 - x0, n - y0)
        w2c8 :: Word8 -> Char
        w2c8 = chr . fromEnum
        converter :: ByteString -> V.Vector Int
        converter ar =
          let tracerFun :: Int -> Int -> Int
              tracerFun l r = if r == 1 then l+r else 0
              f :: Int -> Int
              f i = case w2c8 $ ar `B.index` i of
                '.' -> 1
                'x' -> 0 in
          V.postscanl' tracerFun 0 $ V.generate l f
        decider :: V.Vector Int -> V.Vector (Bool, Bool)
        decider ar =
          let f i =
                let (p1,p2) = getTuple (l-1) i
                    e1 = ar V.! p1
                    e2 = ar V.! p2 in
                (p2-p1 < e2, e1 == 0 || e2 == 0) in
          V.generate l' f
        scanner :: Vector Int -> Vector (Bool, Bool) -> Vector Int
        scanner s r =
          let f :: Int -> (Bool, Bool) -> Int
              f a (b1,b2) = if a = 0 &&
          V.zipWith (\l r -> ())

        -- maxGenFun :: V.Vector (Int,Int,Int,Int,Int) -> V.Vector Int -> Int -> (Int,Int,Int,Int,Int)
        -- maxGenFun s r i =
        --   let (p1,p2) = getTuple (l-1) i
        --       e1 = r V.! p1
        --       e2 = r V.! p2
        --       (_,_,_,_,m) = s V.! i in
        --   traceShow s $
        --   -- If the horizontal fence is clear
        --   if m == 0 && p2-p1 < e2 then traceShow (i,p1,p2,e1,e2,m) (p1,p2,e1,e2,666)
        --   -- If the vertical part is clear, the horizontal does
        --   -- not matter except on the first step.
        --   else if e1 == 0 || e2 == 0 then (p1,p2,e1,e2,0)
        --   else (p1,p2,e1,e2,55) in


    -- V.foldl1' max
    -- . V.imap (\i m ->
    --   let (p1',p2') = getTuple (l-1) i
    --       v@(p1,p2) = (fromIntegral p1', fromIntegral p2') in
    --   if m > 1 then 2*(p2-p1) + 2*(m-1) else -1)
    -- .
    --VB.postscanl' (\l r -> V.generate l' (maxGenFun l r)) (V.replicate l' (0,0,0,0,0))
    VB.postscanl' (\l r -> V.generate l' ()) (V.replicate l' 0)
    .
    VB.map (decider . converter)

  main :: IO ()
  main =
    let parse :: ByteString -> [Int]
        parse = L.unfoldr $ C.readInt . C.dropWhile (== ' ')
        pr :: Int -> String
        pr i = if i <= 0 then "impossible" else show i in do
    [n,l] <- parse <$> B.getLine
    VB.replicateM n B.getLine >>= print . solve l
