module Integer

import Setoid 

%default total
%access public export


data MyInteger = Sub Nat Nat

implementation Num MyInteger where
    (+) (Sub a b) (Sub a' b') = Sub (a + a') (b + b')
    (*) (Sub a b) (Sub a' b') = Sub (a * a' + b * b') (a * b' + b * a')
    fromInteger i =
        if i > 0 then Sub (fromInteger i) 0 else Sub 0 (fromInteger $ abs i)


implementation Neg MyInteger where
    negate (Sub a b) = Sub b a
    (Sub a b) - (Sub a' b') = Sub (a + b') (b + a')
    abs origin@(Sub a b) = if a <= b then Sub b a else origin