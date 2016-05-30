module Main where
  import qualified Data.Text as T
  import qualified Data.Text.IO as TIO
  import Data.Text.Read
  import Control.Monad
  import Control.Applicative ((<$>))
  import Data.Vector.Unboxed (Vector,(!))
  import qualified Data.Vector.Unboxed as V
  import Data.Either

  solve :: Vector Int -> Int
  solve ar =
    V.foldl' go 0 ar' where
      ar' = V.zip ar (V.postscanr' max 0 ar)
      go sr (p,m) = sr + m - p

  main = do
    t <- fmap (read . T.unpack) TIO.getLine -- With Data.Text, the example finishes 15% faster.
    s <-
      forM [1..t] $ \i -> do
          TIO.getLine
          s <- TIO.getLine
          return $ (T.pack . show . solve . V.fromList . map (read . T.unpack) . T.words) s
    TIO.putStr $ T.unlines s
    -- T.unlines . map (T.pack . show . solve . V.fromList . map (read . T.unpack) . T.words)
    --   <$> replicateM t (TIO.getLine >> TIO.getLine) >>= TIO.putStr
