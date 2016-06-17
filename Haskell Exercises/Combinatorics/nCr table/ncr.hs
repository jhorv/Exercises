module Main where
  import qualified Data.Map as M
  import Data.Maybe
  import Control.Monad
  import Debug.Trace

  ncr :: Int -> M.Map (Int, Int) Integer
  ncr n = ncrs
    where go 0 0 = 1
          go n k = (n-1) `c` (k-1) + (n-1) `c` k
          ncrs = M.fromList [((x,y), go x y) | x <- [0..n], y <- [0..x]]
          c n k = fromMaybe 0 (M.lookup (n,k) ncrs)

  main :: IO ()
  main =
    let
      table = ncr 1000
      c n k = fromMaybe 0 (M.lookup (n,k) table)
      in do
      _ <- getLine
      cases <- fmap (map read . lines) getContents
      s <-
        forM cases $ \case_ ->
            forM [0..case_] $ \i ->
              return $ case_ `c` i
      putStr $ unlines $ map (unwords . map (show . (`mod` 1000000000))) s
