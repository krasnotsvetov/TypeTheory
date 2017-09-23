module Rational

import Integer

%access public export
%default total


multNat : MyInteger -> Nat -> MyInteger
multNat (Sub a b) k = Sub (a * k) (b * k)

-- Rational ::= MyInteger / (1 + Nat)

data Rational = MkRat MyInteger Nat

implementation Num Rational where
    (+) (MkRat a b) (MkRat a' b') = MkRat (multNat a (1 + b') + multNat a' (1 + b)) (b + b' + b * b')
    (*) (MkRat a b) (MkRat a' b') = MkRat (a * a') (b + b' + b * b')

    fromInteger a = MkRat (fromInteger a) 0

implementation Neg Rational where
    negate (MkRat a b) = MkRat (negate a) b
    a - b = a + negate b
    abs (MkRat a b) = MkRat (abs a) b