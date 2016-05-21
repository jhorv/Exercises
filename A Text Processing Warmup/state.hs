module Main where
  import qualified Data.Text as T
  import qualified Data.Text.IO as TIO
  import Data.Maybe
  import Data.Char
  import Data.Either
  import Text.ParserCombinators.Parsec hiding (space,spaces)
  import System.Environment
  import Control.Monad

  data OccurenceCounters =
    OccurenceCounters
    { an :: Int
    , a :: Int
    , the :: Int
    , date :: Int
    } deriving (Show)

  zeroCounter = OccurenceCounters {an=0,a=0,the=0,date=0}

  parseThis :: GenParser Char OccurenceCounters String
  parseThis = do
    updateState (\x -> x {an = an x + 5})
    return "zeroCounter"

  updateOcc :: OccurenceCounters -> OccurenceCounters
  updateOcc x = x {the=(the x) + 2}


  main :: IO ()
  main = do
    conts <- liftM head getArgs
    let
      p = case runParser parseThis zeroCounter "OccurenceCounters test" conts of
        Left a -> error $ show a
        Right b -> b
      in print p
