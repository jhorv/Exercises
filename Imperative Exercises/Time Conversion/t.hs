module Main where
  import Data.IORef
  import Data.Char
  import Control.Monad

  main :: IO ()
  main = do
    n <- getLine
    let
      tc :: IO String
      tc = do
        s <- newIORef ""
        return $
          case n of
            v@('1':'2':':':_:_:':':_:_:'P':_) -> take 8 v
            x:y:v@(':':_:_:':':_:_:'P':_) ->
              (intToDigit . (+1) . digitToInt) x :
              (intToDigit . (+2) . digitToInt) y :
              take 6 v
            '1':'2':v@(':':_:_:':':_:_:'A':_) -> "00" ++ take 6 v
            v@(x:y:':':_:_:':':_:_:'A':_) -> take 8 v
      in do
        s <- tc
        putStr s
