module Main where
  import Data.Vector.Unboxed (Vector)
  import qualified Data.Vector.Unboxed as V

  (|>) :: a -> (a -> b) -> b
  x |> f = f x

  main = do
    s <- do
      x <- return $ V.fromList [1,2,3,4] :: IO (Vector Int)
      d <- return $ V.fromList [1,2,3,4] :: IO (Vector Int)
      let
        (x,d) = xd
        xd :: (Vector Int, Vector Int)
        xd =
          V.zip x d
          |> V.unzip
        --(x,d) = xd  -- here is where the error happens
                    -- returning xd works
                    -- removing the shadowing also works
        in return x
    print s
