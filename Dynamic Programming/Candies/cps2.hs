module Main where
  trans :: [Int] -> [Int]
  trans m =
    foldr go (\_ -> []) m 1 where
      go x f y = (x + y) : f (2*y+1)

  main = do
    s <- return $ trans [1,2,3]
    print s

    2 : 5 : (\y -> (3 + y) : (\_ -> []) (2*y+1)) (2*y+1) 3
