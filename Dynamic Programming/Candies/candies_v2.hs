-- The solution from this SO thead.
-- http://stackoverflow.com/questions/37501967/how-to-make-fromlist-lazy-in-this-dynamic-programming-example/37502736#37502736
-- The idea of doing two passes is quite brilliant.
-- Ironically, I thought of something similar to the TBS problem
-- but did not extrapolate it to this problem.

-- The go function is quite intricate. It seems to be written in continuation passing
-- style.

module Main where
  import Control.Monad (replicateM)
  import Control.Applicative ((<$>))
  import Debug.Trace


  solve :: [Int] -> Int
  solve = sum . map fst . loop . reverse . loop . zip  (repeat 0)
      where
      loop :: [(Int, Int)] -> [(Int, Int)]
      loop cs = foldr go (\_ _ -> []) cs 0 0
      go (candy, score) f c s = (candy', score): f candy' score
          where
            -- s is the score and c is the candy of the guy before
            -- if s < score then this guy should get at least c + 1 candies
            candy' = max candy $ if s < score then c + 1 else 1

  main = do
      n <- read <$> getLine
      solve . fmap read <$> replicateM n getLine >>= print

-- Here is the whole post on SO that might explain what those continuation are doing
-- From: http://stackoverflow.com/questions/37501967/how-to-make-fromlist-lazy-in-this-dynamic-programming-example
{-
    module Main where
      import System.Random
      import Data.Foldable
      import Control.Monad
      import qualified Data.Map as M
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

The above code is for the [HackerRank Candies](https://www.hackerrank.com/challenges/candies) challenge. I think the code is correct in essence even though it gives me runtime errors on submission. HackerRank does not say what those errors are, but most likely it is because I ran out allotted memory.

To make the above work, I need to rewrite the above so the fromList gets lazily evaluated or something to that effect. I like the above form and rewriting the functions so they pass along the map as a parameter is something I would very much like to avoid.

I know Haskell has various memoization libraries on Hackage, but the online judge does not allow their use.

I might have coded myself into a hole due to Haskell's purity.

Edit:

I did some experimenting in order to figure out how those folds and lambda's work. I think this is definitely linked to continuation passing after all, as the continuations are being built up along the fold. To show what I mean, I'll demonstrate it with a simple program.

    module Main where
      trans :: [Int] -> [Int]
      trans m =
        foldr go (\_ -> []) m 0 where
          go x f y = (x + y) : f x

      main = do
        s <- return $ trans [1,2,3]
        print s

One thing that surprised me was that when I inserted a print, it got executed in a reverse manner, from left to right, which made me think at first that I misunderstood how foldr works. That turned out to not be the case.

What the above does is print out `[1,3,5]`.

Here is the explanation how it executes. Trying to print out `f x` in the above will not be informative and will cause it to just all around the place.

It starts with something like this. The fold obviously executes 3 `go` functions.

    go x f y = (x + y) : f x
    go x f y = (x + y) : f x
    go x f y = (x + y) : f x

The above is not quite true. One has to keep in mind that all `f`s are separate.

    go x f'' y = (x + y) : f'' x
    go x f' y = (x + y) : f' x
    go x f y = (x + y) : f x

Also for clarity one it should also be instructive to separate out the lambdas.

    go x f'' = \y -> (x + y) : f'' x
    go x f' = \y -> (x + y) : f' x
    go x f = \y -> (x + y) : f x

Now the fold starts from the top. The topmost statement gets evaluated as...

    go 3 (\_ -> []) = \y -> (3 + y) : (\_ -> []) 3

This reduces to:

    go 3 (\_ -> []) = (\y -> (3 + y) : [])

The result is the unfinished lambda above. Now the fold evaluates the second statement.

    go 2 (\y -> (3 + y) : []) = \y -> (2 + y) : (\y -> (3 + y) : []) 2

This reduces to:

    go 2 (\y -> (3 + y) : []) = (\y -> (2 + y) : 5 : [])

The the fold goes to the last statement.

    go 1 (\y -> (2 + y) : 5 : []) = \y -> (1 + y) : (\y -> (2 + y) : 5 : []) 1

This reduces to:

    go 1 (\y -> (2 + y) : 5 : []) = \y -> (1 + y) : 3 : 5 : []

The the 0 outside the fold gets applied and the final lambda gets reduced to

    1 : 3 : 5 : []

This is just the start of it. The case gets more interesting when `f x` is replaced with `f y`.

Here is a similar program to the previous.

    module Main where
      trans :: [Int] -> [Int]
      trans m =
        foldr go (\_ -> []) m 1 where
          go x f y = (x + y) : f (2*y+1)

      main = do
        s <- return $ trans [1,2,3]
        print s

Let me once again go from top to bottom.

    go x f'' = \y -> (x + y) : f'' (2*y+1)
    go x f' = \y -> (x + y) : f' (2*y+1)
    go x f = \y -> (x + y) : f (2*y+1)

The top statement.

    go 3 (\_ -> []) = \y -> (3 + y) : (\_ -> []) (2*y+1)

The middle statement:

    go 2 (\y -> (3 + y) : (\_ -> []) (2*y+1)) = \y -> (2 + y) : (\y -> (3 + y) : (\_ -> []) (2*y+1)) (2*y+1)

The last statement:

    go 1 (\y -> (2 + y) : (\y -> (3 + y) : (\_ -> []) (2*y+1)) (2*y+1)) = \y -> (1 + y) : (\y -> (2 + y) : (\y -> (3 + y) : (\_ -> []) (2*y+1)) (2*y+1)) 2*y+1

Notice how the expressions build up because `y`s cannot be applied. Only after the 0 gets inserted can the whole expression be evaluated.

    (\y -> (1 + y) : (\y -> (2 + y) : (\y -> (3 + y) : (\_ -> []) (2*y+1)) (2*y+1)) 2*y+1) 1

    2 : (\y -> (2 + y) : (\y -> (3 + y) : (\_ -> []) (2*y+1)) (2*y+1)) 3

    2 : 5 : (\y -> (3 + y) : (\_ -> []) (2*y+1)) 7

    2 : 5 : 10 : (\_ -> []) 15

    2 : 5 : 10 : []

There is a buildup due to the order of evaluation.

Edit: So...

    go (candy, score) f c s = (candy', score): f candy' score
        where candy' = max candy $ if s < score then c + 1 else 1

The above in fact does 3 passes across the list in each iteration.

First foldr has to travel to back of the list before it can begin. Then as `candi'` depends on `s` and `c` variables which cannot be applied immediately this necessitates building up the continuations as in that last example.

Then when the two `0` `0` are fed into at the end of the fold, the whole thing only then gets evaluated.

It is a bit hard to reason about.
-}
