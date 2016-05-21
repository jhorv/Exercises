-- Does really poorly on submission. 0/100.

module Main where
  import qualified Data.Text as T
  import qualified Data.Text.IO as TIO
  import qualified Data.Text.Encoding as E
  import qualified Data.ByteString as B
  import Data.Foldable
  import Data.Maybe
  import Data.Char
  import Data.Either
  import Text.ParserCombinators.Parsec hiding (space,spaces,Parser)
  import Text.Parsec.Text
  import Control.Monad
  import qualified Data.List as L
  import qualified Data.Vector.Unboxed as V
  import qualified Data.Vector as VB
  import qualified Data.Set as S
  import qualified Data.Map.Strict as M

  words' :: T.Text -> [T.Text]
  words' text =
    let
      space :: Parser()
      space =  void (satisfy (not . isLetter))

      spaces :: Parser ()
      spaces = eof <|> skipMany1 space

      cT :: Parser Char -> Parser T.Text
      cT = liftM T.singleton

      word :: Parser T.Text
      word = liftM T.concat (many1 $ cT letter)

      words :: Parser [T.Text]
      words = sepEndBy1 word spaces

      in case parse (optional spaces >> words) "Parsing words" text of
          Left a -> error $ show a
          Right b -> b

  wordIndexMapFun :: [T.Text] -> M.Map T.Text Int
  wordIndexMapFun corpus =
    let
      unique_corpus_words = S.toList $ S.fromList corpus :: [T.Text]
      in M.fromList $ zipWith (\a b -> (a,b)) unique_corpus_words [0..]

  indexOfWord' :: M.Map T.Text Int -> T.Text -> Int
  indexOfWord' word_index_map k =
    fromMaybe (error "Key not found") (M.lookup k word_index_map)

  makeFrequencyMap :: (T.Text -> Int) -> [T.Text] -> M.Map Int Int
  makeFrequencyMap index_of_word = foldl' (\m k -> M.insertWith (+) (index_of_word k) 1 m) M.empty

  idfFromFrMaps :: [M.Map Int Int] -> M.Map Int Float
  idfFromFrMaps frequencies =
    let
      mergeFrequencyMaps :: [M.Map Int Int] -> M.Map Int Int
      mergeFrequencyMaps = foldl' (M.foldlWithKey' (\m k v -> M.insertWith (+) k v m)) M.empty

      frequencies_set_to_1 = map (M.map $ const 1) frequencies :: [M.Map Int Int]
      merged_document_frequencies = mergeFrequencyMaps frequencies_set_to_1
      number_of_documents = fromIntegral $ length frequencies :: Float
      in M.map (\x -> logBase 10.0 (number_of_documents / (fromIntegral x :: Float))) merged_document_frequencies

  tfFromFrMap :: M.Map Int Int -> M.Map Int Float
  tfFromFrMap = M.map ((\v -> if v > 0 then 1 + log v else 0) . fromIntegral)

  weightedTfIdfVectorFromWeigtedMap :: M.Map Int Float -> M.Map Int Float -> V.Vector (Int,Float)
  weightedTfIdfVectorFromWeigtedMap idf tf =
    let
      normalize :: V.Vector (Int, Float) -> V.Vector (Int, Float)
      normalize vec =
        let l = sqrt $ V.foldl (\s (_,v) -> s+v*v) 0 vec
          in V.map (\(i,v) -> (i, v / l)) vec
      in
        normalize $ V.fromList $ M.toList $
        M.mapWithKey (\k tf ->
          let idf' = fromMaybe (error "No df found!") (M.lookup k idf)
            in tf*idf') tf

  cosineSimilarity :: V.Vector (Int, Float) -> V.Vector (Int, Float) -> Float
  cosineSimilarity a b =
    let
      loop :: Float -> Int -> Int -> Float
      loop r i_a i_b
        | fst (a V.! i_a) >= V.length a || fst (b V.! i_b) >= V.length b = r
        | fst (a V.! i_a) == fst (b V.! i_b) = loop (r + snd (a V.! i_a) * snd (b V.! i_b)) (i_a + 1) (i_b + 1)
        | fst (a V.! i_a) < fst (b V.! i_b) = loop r (i_a + 1) i_b
        | otherwise = loop r i_a (i_b + 1)
      in loop 0.0 0 0

  main :: IO ()
  main = do
    conts <- liftM (tail . T.lines . E.decodeUtf8) B.getContents
    appleComputers <- liftM E.decodeUtf8 $ B.readFile "apple-computers.txt"
    appleFruit <- liftM E.decodeUtf8 $ B.readFile "apple-fruit.txt"
    let
      processWords = map T.toLower . filter (\x -> T.length x >= 2) . words'
      wordsInInput :: [[T.Text]]
      wordsInInput = map processWords conts
      wordsInAppleComputer :: [T.Text]
      wordsInAppleComputer = processWords appleComputers
      wordsInAppleFruit :: [T.Text]
      wordsInAppleFruit = processWords appleFruit
      wordsAll :: [T.Text]
      wordsAll = wordsInAppleComputer ++ wordsInAppleFruit ++ L.concat wordsInInput

      wordIndexMap = wordIndexMapFun wordsAll
      indexOfWord = indexOfWord' wordIndexMap
      frMapFun = makeFrequencyMap indexOfWord
      mapsOfInput = map frMapFun wordsInInput
      mapOfAppleComputer = frMapFun wordsInAppleComputer
      mapOfAppleFruit = frMapFun wordsInAppleFruit
      allMaps = mapOfAppleFruit : mapOfAppleComputer : mapsOfInput
      idf = idfFromFrMaps allMaps

      inputTfs = map tfFromFrMap mapsOfInput
      compTfs = tfFromFrMap mapOfAppleComputer
      fruitTfs = tfFromFrMap mapOfAppleFruit

      tfIdfFun = weightedTfIdfVectorFromWeigtedMap idf
      vecInput = map tfIdfFun inputTfs
      vecComp = tfIdfFun compTfs
      vecFruit = tfIdfFun fruitTfs

      compScores = map (cosineSimilarity vecComp) vecInput
      fruitScores = map (cosineSimilarity vecFruit) vecInput

      result = unlines $ zipWith (\c f -> if c < f then "fruit" else "computer-company") compScores fruitScores
    putStr result
