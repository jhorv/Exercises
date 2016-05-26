module Main where
  import qualified Data.Text as T
  import qualified Data.Text.IO as TIO
  import Data.Foldable
  import Data.Maybe
  import Data.Char
  import Data.Either
  import Text.ParserCombinators.Parsec hiding (space,spaces)
  import Text.ParserCombinators.Parsec.Perm
  import System.Environment
  import Control.Monad
  import qualified Data.List as L

  space :: Parser()
  space =  void (satisfy (\x -> not (isLetter x || isDigit x)))

  letterOrDigit :: Parser Char
  letterOrDigit = satisfy (\x -> isLetter x || isDigit x)

  spaces :: Parser ()
  spaces = eof <|> skipMany1 space

  data OccurenceCount =
    OccurenceCount
    { a :: Int
    , an :: Int
    , the :: Int
    , date :: Int
    }

  instance Show OccurenceCount where
    show (OccurenceCount a an the date) =
      show a ++ '\n' : show an ++ '\n' : show the ++ '\n' : show date

  data OccurenceCounters = OccAn | OccA | OccThe | OccDate | OccNothing deriving (Show)

  main :: IO ()
  main = do
    conts <- liftM (filter (/= "") . tail . lines) getContents
    let
      -- conts = "  an  "
      parseOccurence :: Parser OccurenceCounters
      parseOccurence =
        let
          parseAnOrA = do
            char 'a' <|> char 'A'
            (char 'n' >> spaces >> return OccAn) <|> (spaces >> return OccA)
          parseThe = do
            char 't' <|> char 'T'
            string "he" >> spaces >> return OccThe
          --'15/11/2012','15/11/12', '15th March 1999','15th March 99' or '20th of March, 1999'
          parseDay =
            let
              oneDigitDay :: Parser ()
              oneDigitDay = do
                d <- digit
                case d of
                  '0' -> fail "Day cannot be zero."
                  '1' -> optional $ string "st"
                  '2' -> optional $ string "nd"
                  '3' -> optional $ string "rd"
                  otherwise -> optional $ string "th"
                spaces
              twoDigitDay :: Parser ()
              twoDigitDay = do
                d1 <- digit >>= \x -> return $ digitToInt x
                d2 <- digit >>= \x -> return $ digitToInt x
                case d2 of
                  1 -> optional $ string "st"
                  2 -> optional $ string "nd"
                  3 -> optional $ string "rd"
                  otherwise -> optional $ string "th"
                spaces
                let r = d1*10+d2
                  in unless (r < 32) $ fail "Day in date has to be less than 32."
            in choice (map try [oneDigitDay, twoDigitDay]) >> optional (string "of" >> spaces)
          parseMonth =
            let
              oneDigitMonth :: Parser ()
              oneDigitMonth = digit >> spaces
              twoDigitMonth :: Parser ()
              twoDigitMonth = do
                d1 <- digit >>= \x -> return $ digitToInt x
                d2 <- digit >>= \x -> return $ digitToInt x
                spaces
                let r = d1*10+d2
                  in unless (r < 13) $ fail "Day in date has to be less than 32."
              writtenMonth :: Parser ()
              writtenMonth = choice $ map ((try . (>> spaces)) . string) ["January","February","March",
                "April","May","June","July","August","September","October","November","December"]
            in choice $ map try [oneDigitMonth, twoDigitMonth, writtenMonth]
          parseYear =
            let
              twoDigitYear :: Parser ()
              twoDigitYear = digit >> digit >> spaces
              fourDigitYear :: Parser ()
              fourDigitYear = digit >> digit >> digit >> digit >> spaces
            in choice $ map try [twoDigitYear, fourDigitYear]
          parseDate =
            let
              p :: [[Parser()]]
              p = L.permutations [parseDay,parseMonth,parseYear]
              f = map (L.foldl' (>>) (return ())) p -- id is not the correct neutral element here
            in
              choice (map try f) >> return OccDate
          parseRandomWord = many1 letterOrDigit >> spaces >> return OccNothing
          in choice $ map try [parseAnOrA, parseThe, parseDate, parseRandomWord]

      parseOccurences = optional spaces >> many parseOccurence
      getCount :: String -> OccurenceCount
      getCount text =
        case parse parseOccurences "A Text Processing Warmup" text of
          Left a -> error $ show a
          Right b ->
            foldl' (\s x ->
              case x of
                OccAn -> s {an=an s + 1}
                OccA -> s {a=a s + 1}
                OccThe -> s {the=the s + 1}
                OccDate -> s {date=date s + 1}
                OccNothing -> s
            ) (OccurenceCount 0 0 0 0) b
      getCounts = map getCount conts
      in putStr $ unlines $ map show getCounts
