module Main where
import Data.Char

data States =
  SentenceStart
  | SentenceMiddle
  | Quoting

splitStr :: String -> String -> States -> String
splitStr ('\"':xs) acc s
   | SentenceStart  <- s = result
   | SentenceMiddle <- s = result
   where result = splitStr xs ('\"':acc) Quoting
splitStr ('\"':xs) acc Quoting =
  splitStr xs ('\"':acc) SentenceMiddle
splitStr (x:xs) acc Quoting =
  splitStr xs (x:acc) Quoting
splitStr (' ':xs) acc SentenceStart =
  splitStr xs acc SentenceStart
splitStr xs acc SentenceStart =
  splitStr xs acc SentenceMiddle
splitStr (x:'.':'.':'.':xs) acc SentenceMiddle =
  splitStr xs ('\n':'.':'.':'.':x:acc) SentenceStart
splitStr ('D':'r':'.':xs) acc SentenceMiddle =
  splitStr xs ('.':'r':'D':acc) SentenceMiddle
splitStr ('M':'r':'.':xs) acc SentenceMiddle =
  splitStr xs ('.':'r':'M':acc) SentenceMiddle
splitStr ('M':'i':'s':'s':'.':xs) acc SentenceMiddle =
  splitStr xs ('.':'s':'s':'i':'M':acc) SentenceMiddle
splitStr ('P':'r':'o':'f':'.':xs) acc SentenceMiddle =
  splitStr xs ('.':'f':'o':'r':'P':acc) SentenceMiddle
splitStr (x:'.':xs) acc SentenceMiddle
  | isUpper x = splitStr xs ('.':x:acc) SentenceMiddle
  | otherwise = splitStr xs ('\n':'.':x:acc) SentenceStart
splitStr (x:'?':xs) acc SentenceMiddle =
  splitStr xs ('\n':'?':x:acc) SentenceStart
splitStr (x:'!':xs) acc SentenceMiddle =
  splitStr xs ('\n':'!':x:acc) SentenceStart
splitStr (x:xs) acc SentenceMiddle =
  splitStr xs (x:acc) SentenceMiddle
splitStr [] acc _ = acc

main :: IO ()
main = do
  str <- getLine
  putStr $ reverse $ splitStr str [] SentenceStart --"This is a sentence. \"This is one.\", I said. This is another." [] SentenceStart
