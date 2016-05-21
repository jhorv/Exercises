module Main where
  import qualified Data.Set as S
  import qualified Data.Map.Strict as M
  import qualified Data.Text as T
  import qualified Data.Text.IO as TIO
  import Data.Text.Read
  import Data.Char
  import Data.Maybe
  import qualified Data.Vector.Unboxed as V
  import qualified Data.Vector as VB

  main :: IO ()
  main = do
    conts <- TIO.getContents
    let
       --dec = E.decodeUtf8 conts
      dec = conts

      filterWords :: T.Text -> [T.Text]
      filterWords = filter (\x -> T.length x > 1) . map (T.filter isLetter) . T.words . T.toLower

      index_of_word :: T.Text -> Int
      index_of_word k =
        let
          word_index_map :: M.Map T.Text Int
          word_index_map =
            let
              unique_corpus_words = (S.toList . S.fromList . filterWords) dec :: [T.Text]
              in M.fromList $ zipWith (\a b -> (a,b)) unique_corpus_words [0..]
          in fromMaybe (error "Key not found") (M.lookup k word_index_map)

      line_by_line = T.lines dec
      num_items_per_set =
        case decimal (head line_by_line) of
          Left er -> error er
          Right (v,_) -> v :: Int

      (set_a, set_b) =
        ((map filterWords . take num_items_per_set . tail) line_by_line
        ,(map filterWords . take num_items_per_set . drop (num_items_per_set + 2)) line_by_line)
        :: ([[T.Text]],[[T.Text]])

      (set_a_tf,set_b_tf) =
        let
          getFrequencyMap :: [T.Text] -> M.Map Int Int
          getFrequencyMap =
              foldl (\m k -> M.insertWith (+) (index_of_word k) 1 m) M.empty
          in (map getFrequencyMap set_a,map getFrequencyMap set_b)
          :: ([M.Map Int Int],[M.Map Int Int])

      (weighted_set_a,weighted_set_b) =
        let
          weighted_df :: M.Map Int Float
          weighted_df =
            let
              set_ab_term_frequencies = set_a_tf ++ set_b_tf
              set_ab_term_frequencies_normalized_to_1 = map (M.map $ const 1) set_ab_term_frequencies :: [M.Map Int Int]
              in let
                document_frequencies = foldl (M.foldlWithKey (\s k v -> M.insertWith (+) k v s)) M.empty set_ab_term_frequencies_normalized_to_1
                number_of_documents = 2.0 * fromIntegral num_items_per_set :: Float
                in M.map (\x -> logBase 10.0 (number_of_documents / (fromIntegral x :: Float))) document_frequencies
          get_weighted_tf :: M.Map Int Int -> V.Vector (Int, Float)
          get_weighted_tf =
            V.fromList . M.toList .
            M.mapWithKey (\k v' ->
              let
                df = fromMaybe (error "No df found!") (M.lookup k weighted_df)
                v = fromIntegral v' :: Float
                tf = if v > 0 then 1 + log v else 0
                  in tf*df)
          normalize :: V.Vector (Int, Float) -> V.Vector (Int, Float)
          normalize vec =
            let l = sqrt $ V.foldl (\s (_,v) -> s+v*v) 0 vec
              in V.map (\(i,v) -> (i, v / l)) vec
          f :: [M.Map Int Int] -> VB.Vector (V.Vector (Int, Float))
          f = VB.fromList . map (normalize . get_weighted_tf)
          in (f set_a_tf, f set_b_tf)

      max_matches =
        let
          cosine_similarity :: V.Vector (Int, Float) -> V.Vector (Int, Float) -> Float
          cosine_similarity a b =
            let
              loop :: Float -> Int -> Int -> Float
              loop r i_a i_b
                | fst (a V.! i_a) >= V.length a || fst (b V.! i_b) >= V.length b = r
                | fst (a V.! i_a) == fst (b V.! i_b) = loop (r + snd (a V.! i_a) * snd (b V.! i_b)) (i_a + 1) (i_b + 1)
                | fst (a V.! i_a) < fst (b V.! i_b) = loop r (i_a + 1) i_b
                | otherwise = loop r i_a (i_b + 1)
              in loop 0.0 0 0
          in
            VB.map (\a ->
              snd $ VB.ifoldl(\val@(max_v,_) i b_i ->
                let cs = cosine_similarity a b_i in
                if max_v < cs then (cs, i) else val) (0.0,0) weighted_set_b) weighted_set_a

      text_out = unlines $ map (show . (+) 1) $ VB.toList max_matches
      in putStr text_out
