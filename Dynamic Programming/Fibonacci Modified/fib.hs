module Main where
  import Data.Vector hiding (map)

  fib' :: Integer -> Integer -> Int -> Integer
  fib' a b max = go max
    where go 0 = a
          go 1 = b
          go n = (fibs ! (n - 1))*(fibs ! (n - 1)) + fibs ! (n - 2)
          fibs = fromList [go x | x <- [0..max]]

  main :: IO ()
  main = do
    [a',b',x'] <- fmap words getLine
    let
      a = read a'
      b = read b'
      x = read x' - 1

      in print $ fib' a b x
