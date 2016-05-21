module Main where
   import qualified Data.Map.Strict as Map
   import qualified Data.Text as T
   import qualified Data.ByteString as B
   import Data.Text.Encoding as E

   type Trimap = Map.Map T.Text (Map.Map T.Text (Map.Map T.Text Integer))

   addWordToTriMap :: Trimap  -> T.Text -> T.Text -> T.Text -> Trimap
   addWordToTriMap m1 k1 k2 k3 =
     Map.insert k1
       (case Map.lookup k1 m1 of
           Just m2 ->
              Map.insert k2
                (case Map.lookup k2 m2 of
                  Just m3 -> Map.insertWith (+) k3 1 m3
                  Nothing -> Map.singleton k3 1) m2
           Nothing -> Map.singleton k2 (Map.singleton k3 1)) m1

   getTrigrams :: Trimap -> [T.Text] -> Trimap
   getTrigrams m (w1:w2:w3:xs) = getTrigrams (addWordToTriMap m w1 w2 w3) (w2:w3:xs)
   getTrigrams m _ = m

   trigramCounts :: Trimap -> [(T.Text,T.Text,T.Text,Integer)]
   trigramCounts =
     concatMap (\(k1,v) -> concatMap (\(k2,v) -> map(\(k3,v) -> (k1,k2,k3,v)) $ Map.toList v) $ Map.toList v) . Map.toList

   toString :: (T.Text,T.Text,T.Text) -> T.Text
   toString (v1,v2,v3) = T.concat [v1, T.pack " ", v2, T.pack " ", v3]

   main :: IO ()
   main = do
     conts <- B.getContents
     let c = (T.words . T.toLower . E.decodeUtf8) conts
         m = getTrigrams Map.empty c
         counts = trigramCounts m
         trigram_max = foldl (\val@(s,m) (k1,k2,k3,v) -> if m < v then (toString (k1,k2,k3), v) else val) (T.pack "", 0) counts
         in
            putStr $ T.unpack $ fst trigram_max
