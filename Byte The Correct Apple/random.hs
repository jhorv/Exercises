-- Makes random outputs.

module Main where
  import System.Random

  main :: IO ()
  main = do
    conts <- getContents
    r <- newStdGen
    let
      num_lines = read $ head $ lines conts :: Int
      -- messages = ["fruit","computer-company"]
      random_numbers :: [Bool]
      random_numbers = randoms r
      boolToComp False = "fruit"
      boolToComp True = "computer-company"
      in
        putStr $ unlines $ map boolToComp $ take num_lines random_numbers
