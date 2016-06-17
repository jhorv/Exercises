module Main where
  import System.Random
  import Data.Foldable
  import Control.Monad
  import qualified Data.Map as M
  import qualified Data.IntMap as W
  import qualified Data.Vector as V
  import Debug.Trace
  import Data.Maybe
  import Data.Ord

  -- Represents the maximal integer. maxBound is no good because it overflows.
  -- Ideally should be something like a billion.
  maxi = 1000

  candies :: V.Vector Int -> Int --M.Map (Int, Int) Int
  candies ar = ff [l (V.length ar - 1) x | x <- [0..maxi]]
    where
      go :: Int -> Int -> Int
      go _ 0 = maxi
      go 0 j = j
      go i j =
        case compare (ar V.! (i-1)) (ar V.! i) of
          LT -> ff [l (i-1) x + j | x <- [0..j-1]]
          GT -> ff [l (i-1) x + j | x <- [j+1..maxi]]
          EQ -> ff [l (i-1) x + j | x <- [0..maxi]]
      l :: Int -> Int -> Int
      l i j = fromMaybe maxi (M.lookup (i,j) cs)
      ff l = --minimum l
        case l of
          l:ls -> if l < maxi then l else ff ls
          [] -> maxi

      -- I need to make this lazy somehow.
      cs :: M.Map (Int, Int) Int
      cs = M.fromList [((i,j), go i j) | i <- [0..V.length ar - 1], j <- [0..maxi]]


  main :: IO ()
  main = do
    --ar <- fmap (V.fromList . map read . tail . words) getContents
    g <- fmap (V.fromList . take 5 . randomRs (1,50)) getStdGen
    print $ candies g
