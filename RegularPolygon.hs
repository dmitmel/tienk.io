module RegularPolygon where

import           System.Environment

main :: IO ()
main = do
    [polygon] <- getArgs
    putStrLn $ join ", " $ map (\(x, y) -> show x ++ " " ++ show y) $ regularPolygonPoints $ read polygon

join :: [a] -> [[a]] -> [a]
join _   []     = []
join _   [x]    = x
join sep (x:xs) = x ++ sep ++ join sep xs

data RegularPolygon
    = RegularPolygon
    { polygonPosition :: Point
    , polygonRadius   :: Float
    , polygonEdges    :: Int
    , polygonRotation :: Float
    } deriving (Show, Eq, Ord, Read)

type Point = (Float, Float)

regularPolygonPoints :: RegularPolygon -> [Point]
regularPolygonPoints (RegularPolygon position radius edges rotationDeg) = map edgeToPoint [0..floatEdges - 1]
    where
        floatEdges = fromIntegral edges :: Float
        step = 2 * pi / floatEdges
        rotationRad = rotationDeg * 0.0174533
        edgeToPoint n = (fst position + radius * cos (n * step + rotationRad),
                         snd position + radius * sin (n * step + rotationRad))
