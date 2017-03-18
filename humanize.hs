padLeft :: a -> Int -> [a] -> [a]
padLeft x minLen xs
    | len < minLen = replicate (minLen - len) x ++ xs
    | otherwise    = xs
    where len = length xs

humanize :: Int -> Int -> String
humanize digits n
    | n < 0     = '-' : humanize digits n
    | n < m     = show n
    | otherwise = humanize digits (n `div` m) ++ "," ++ padLeft '0' digits (show $ n `mod` m)
    where m = 10 ^ digits

myRound :: (RealFrac a, Fractional b) => a -> b
myRound n = fromIntegral (floor $ n / 100 + 0.5) / 10.0