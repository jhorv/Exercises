import Control.Applicative ((<$>))
import Control.Monad (replicateM, mapM_)
import Data.Maybe (fromJust)
import Data.ByteString (ByteString)
import qualified Data.ByteString as B
import qualified Data.ByteString.Char8 as C
import qualified Data.List as L

parse :: ByteString -> [Int]
parse b = fst . fromJust . C.readInt <$> C.words b

solve :: IO Int
solve = B.getLine >> solve' . parse <$> B.getLine
    where
      solve' :: [Int] -> Int
      solve' ar =
        L.foldl' go 0 ar' where
          ar' = zip ar (init $ L.scanr max 0 ar)
          go sr (p,m) = sr + m - p

main = do
    [n] <- parse <$> B.getLine
    replicateM n solve >>= mapM_ print
