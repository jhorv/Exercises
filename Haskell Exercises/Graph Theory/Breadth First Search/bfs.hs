module Main where
  import Control.Monad
  import Data.IORef
  import qualified Data.Map as M
  import qualified Data.Set as S
  import Debug.Trace
  import Data.List
  import Data.Maybe

  (|>) :: a -> (a -> b) -> b
  x |> f = f x

  data TestCase =
    TestCase
            {
             numNodes :: Int
            ,numEdges :: Int
            ,startingNode :: Int
            ,edges :: M.Map Int (S.Set Int)
            } deriving (Show)

  main :: IO ()
  main = do
    t <- fmap read getLine :: IO Int
    s <-
      forM [1..t] $ \_ -> do
        [nn,ne] <- fmap (map (read :: String -> Int) . words) getLine
        m <- newIORef M.empty -- :: IORef (M.Map Int ((S.Set Int), (Maybe Int)))
        forM_ [1..ne] $ \_ -> do
          [s,e] <- fmap (map (read :: String -> Int) . words) getLine
          modifyIORef' m (M.insertWith' S.union s (S.singleton e))
          modifyIORef' m (M.insertWith' S.union e (S.singleton s))
        sn <- fmap (read :: String -> Int) getLine

        m <- readIORef m
        return TestCase {numNodes = nn, numEdges=ne, startingNode=sn, edges=m}
    let
      bfs :: Int -> S.Set Int -> M.Map Int (S.Set Int) -> M.Map Int Int
      bfs turn curNodes edgesMap =
        let
          edgesMap' = S.foldl' (flip M.delete) edgesMap curNodes -- Remove the edges in the current node set
          curNodes' = -- Get the edges for the next nodes set.
            M.foldl' S.union S.empty (M.filterWithKey (\k _ -> S.member k curNodes) edgesMap)
            |> S.filter (\x -> -- Remove the edges that were eliminated.
              case M.lookup x edgesMap' of
                Just _ -> True
                _ -> False
            )
          r = M.fromSet (const turn) curNodes' -- Turn the set into a map.
          in
            if S.null curNodes'
            then r
            else M.union r $ bfs (turn+6) curNodes' edgesMap' -- Union favors the left argument.
      printFun :: Int -> Int -> M.Map Int Int -> String
      printFun excl nn m =
        let
          f i = fromMaybe (-1) (M.lookup i m)
          in
            unwords $ map show $
            mapMaybe (\x -> if x /= excl then Just $ f x else Nothing) [1..nn]
      in putStr $ unlines $ map (\x -> printFun (startingNode x) (numNodes x) $ bfs 6 (S.singleton $ startingNode x) (edges x)) s
