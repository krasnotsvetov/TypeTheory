module RationalSetoid

import Setoid
import Integer
import Rational
import IntegerSetoid

%access public export
%default total

data RationalEq : Rational -> Rational -> Type where
    RationalRefl : (eq : multNat a ((1 + d)) `MyIntegerEq` multNat c ((1 + b))) -> RationalEq (MkRat a b) (MkRat c d)


rationalRefl : Reflx RationalEq
rationalRefl (MkRat a b) = RationalRefl (myIntegerRefl (multNat a ((1 + b)))) 

rationalSymmetric : Sym RationalEq
rationalSymmetric (MkRat a b) (MkRat c d) (RationalRefl eq) = RationalRefl $ myIntegerSymmetric (multNat a ((1 + d))) (multNat c ((1 + b))) eq

-- Идея доказательства базируется на следующем:
--   Из свойств транзитивности имеем:
--           (a1 * (1 + d)) + (c2 * (1 + b)) = (c1 * (1 + b)) + (a2 * (1 + d)) &&
--           (c1 * (1 + f)) + (e2 * (1 + d)) = (e1 * (1 + d)) + (c2 * (1 + f))
--

-- part1Common:  (a1 * (1 + d)) * (1 + f) + (c2 * (1 + b)) * (1 + f) = (c1 * (1 + b)) * (1 + f) + (a2 * (1 + d)) * (1 + f) &&
--               (c1 * (1 + f)) * (1 + b) + (e2 * (1 + d)) * (1 + b) = (e1 * (1 + d)) * (1 + b) + (c2 * (1 + f)) * (1 + b)
 
-- part2Common:  (a1 * (1 + d)) * (1 + f) + (c2 * (1 + b)) * (1 + f) + (c1 * (1 + f)) * (1 + b) + (e2 * (1 + d)) * (1 + b) =
--               (c1 * (1 + b)) * (1 + f) + (a2 * (1 + d)) * (1 + f) + (e1 * (1 + d)) * (1 + b) + (c2 * (1 + f)) * (1 + b)

-- part3Common   (a1 * (1 + d)) * (1 + f) + (e2 * (1 + d)) * (1 + b) = (a2 * (1 + d)) * (1 + f) + (e1 * (1 + d)) * (1 + b)

-- part4Common   (1 + d) * (a1 * (1 + f) + e2 * (1 + b)) = (1 + d) * (a2 * (1 + f) + e1 * (1 + b))

-- part5Common   a1 * (1 + f) + e2 * (1 + b) = e1 * (1 + b) + a2 * (1 + f)


multBy : (u1 * S y) + (v2 * S x)  = (v1 * S x) + (u2 * S y) ->
    (u1 * S y) * S z + (v2 * S x) * S z = (v1 * S x) * S z + (u2 * S y) * S z
multBy prf {u1 = u1} {u2 = u2} {v1 = v1} {v2 = v2} {x = x} {y = y} {z = z} =
    rewrite sym $ multDistributesOverPlusLeft (u1 * S y) (v2 * S x) (S z) in
    rewrite sym $ multDistributesOverPlusLeft (v1 * S x) (u2 * S y) (S z) in cong {f = (* S z)} prf


rationalTransitive : Trans RationalEq
rationalTransitive (MkRat (Sub a a') b) (MkRat (Sub c c') d) (MkRat (Sub e e') f)
        (RationalRefl (MyIntegerRefl eq1)) (RationalRefl (MyIntegerRefl eq2)) = RationalRefl $ MyIntegerRefl part5Common
        where
            -- домножим каждое равенство
            part1Common1 : (a * (1 + d)) * (1 + f) + (c' * (1 + b)) * (1 + f) = (c * (1 + b)) * (1 + f) + (a' * (1 + d)) * (1 + f)
            part1Common1 = multBy eq1

            part1Common2 : (c * (1 + f)) * (1 + b) + (e' * (1 + d)) * (1 + b) = (e * (1 + d)) * (1 + b) + (c' * (1 + f)) * (1 + b)
            part1Common2 = multBy eq2
        
            -- покажем, что можно сложить части равенств
            part2Common : ((a * (1 + d)) * (1 + f) + (c' * (1 + b)) * (1 + f)) + ((c * (1 + f)) * (1 + b) + (e' * (1 + d)) * (1 + b)) =
                  ((c * (1 + b)) * (1 + f) + (a' * (1 + d)) * (1 + f)) + ((e * (1 + d)) * (1 + b) + (c' * (1 + f)) * (1 + b))
            part2Common = sumEq part1Common1 part1Common2 where
                sumEq : {u : Nat} -> {v : Nat} -> {x : Nat} -> {y : Nat} -> (u = v) -> (x = y) -> (u + x = v + y)
                sumEq prf1 prf2 = rewrite prf1 in rewrite prf2 in Refl
        
            thirdCommon : (a *  (1 + d)) * (1 + f) + (e' * (1 + d)) * (1 + b) = (a' * (1 + d)) * (1 + f) + (e *  (1 + d)) * (1 + b)
            thirdCommon = rewrite plusCommutative (a * (1 + d) * (1 + f)) (e' * (1 + d) * (1 + b)) in fifth
              where
                first : ((c * (1 + f)) * (1 + b) + (e' * (1 + d)) * (1 + b)) + (a * (1 + d)) * (1 + f) + (c' * (1 + f)) * (1 + b) =
                       ((c * (1 + b)) * (1 + f) + (a' * (1 + d)) * (1 + f)) + (e * (1 + d)) * (1 + b) + (c' * (1 + f)) * (1 + b)
                first =
                    rewrite sym $ plusAssociative
                     ((c * (1 + f)) * (1 + b) + (e' * (1 + d)) * (1 + b)) ((a * (1 + d)) * (1 + f)) ((c' * (1 + f)) * (1 + b)) in
                    rewrite sym $ plusAssociative
                     ((c * (1 + b)) * (1 + f) + (a' * (1 + d)) * (1 + f)) ((e * (1 + d)) * (1 + b)) ((c' * (1 + f)) * (1 + b)) in 
                    rewrite sym $ left in part2Common where
                        left : ((a * (1 + d)) * (1 + f) + (c' * (1 + b)) * (1 + f)) + ((c * (1 + f)) * (1 + b) + (e' * (1 + d)) * (1 + b)) =
                            ((c * (1 + f)) * (1 + b) + (e' * (1 + d)) * (1 + b)) + ((a * (1 + d)) * (1 + f) + (c' * (1 + f)) * (1 + b))
                        left = rewrite sym $ multAssociative c' ((1 + f)) ((1 + b)) in 
                            rewrite sym $ multAssociative c' ((1 + b)) ((1 + f)) in
                            rewrite sym $ multCommutative ((1 + f)) ((1 + b)) in
                            rewrite plusCommutative ((a * (1 + d)) * (1 + f) + c' * (S (b + f * (1 + b)))) 
                                ((c * (1 + f)) * (1 + b) + (e' * (1 + d)) * (1 + b)) in Refl

        
                second : (c  * (1 + f)) * (1 + b) + (e' * (1 + d)) * (1 + b) + (a  * (1 + d)) * (1 + f) =
                     (c  * (1 + b)) * (1 + f) + (a' * (1 + d)) * (1 + f) + (e  * (1 + d)) * (1 + b)
                second = plusRightCancel ((c * (1 + f)) * (1 + b) + (e' * (1 + d)) * (1 + b) + (a * (1 + d)) * (1 + f)) 
                    ((c * (1 + b)) * (1 + f) + (a' * (1 + d)) * (1 + f) + (e * (1 + d)) * (1 + b)) (c' * (1 + f) * (1 + b)) first
        
                third : (c * (1 + f)) * (1 + b) + ((e' * (1 + d)) * (1 + b) + (a * (1 + d)) * (1 + f)) =
                     (c * (1 + b)) * (1 + f) + ((a' * (1 + d)) * (1 + f) + (e * (1 + d)) * (1 + b))
                third = rewrite plusAssociative ((c * (1 + f)) * (1 + b)) ((e' * (1 + d)) * (1 + b)) ((a * (1 + d)) * (1 + f)) in
                    rewrite plusAssociative ((c * (1 + b)) * (1 + f)) ((a' * (1 + d)) * (1 + f)) ((e * (1 + d)) * (1 + b)) in second
        
               
        
                fourth : (c * (1 + b)) * (1 + f) + ((e' * (1 + d)) * (1 + b) + (a * (1 + d)) * (1 + f)) = 
                    (c * (1 + b)) * (1 + f) + ((a' * (1 + d)) * (1 + f) + (e * (1 + d)) * (1 + b))
                fourth = rewrite left in third where 
                    left : (c * (1 + b)) * (1 + f) + ((e' * (1 + d)) * (1 + b) + (a * (1 + d)) * (1 + f)) = 
                        (c * (1 + f)) * (1 + b) + ((e' * (1 + d)) * (1 + b) + (a * (1 + d)) * (1 + f))
                    left = rewrite sym $ multAssociative c ((1 + b)) ((1 + f)) in rewrite multCommutative ((1 + b)) ((1 + f)) in
                        rewrite multAssociative c ((1 + f)) ((1 + b)) in Refl
        
                fifth : (e' * (1 + d)) * (1 + b) +  (a  * (1 + d)) * (1 + f) = (a' * (1 + d)) * (1 + f) + (e  * (1 + d)) * (1 + b)
                fifth = plusLeftCancel (c * (1 + b) * (1 + f))  ((e' * (1 + d)) * (1 + b) + (a * (1 + d)) * (1 + f)) 
                    ((a' * (1 + d)) * (1 + f) + (e * (1 + d)) * (1 + b)) fourth
        

            statement : (x : Nat) -> (y : Nat) -> (z : Nat) -> x * (y * z) = y * x * z
            statement x y z = rewrite multAssociative x y z in rewrite multCommutative x y in Refl

            part4Common : (1 + d) * (a  * (1 + f) + e' * (1 + b)) = (1 + d) * (a' * (1 + f) + e  * (1 + b))
            part4Common =
                rewrite multDistributesOverPlusRight ((1 + d)) (a * (1 + f)) (e' * (1 + b)) in
                rewrite multDistributesOverPlusRight ((1 + d)) (a' * (1 + f)) (e * (1 + b)) in
                rewrite statement ((1 + d)) a ((1 + f)) in rewrite statement ((1 + d)) a' ((1 + f)) in
                rewrite statement ((1 + d)) e ((1 + b)) in rewrite statement ((1 + d)) e' ((1 + b)) in thirdCommon
              
        
            part5Common : a * (1 + f) + e' * (1 + b) = e * (1 + b) + a' * (1 + f)
            part5Common = rewrite plusCommutative (e * (1 + b)) (a' * (1 + f)) in
                multLeftCancel _ _ _ part4Common
              where
                addensumIsZero : (x : Nat) -> (y : Nat) -> x + y = 0 -> x = 0
                addensumIsZero x (S k) prf = void $ SIsNotZ int
                  where
                    int : (S k) + x = 0
                    int = rewrite plusCommutative (S k) x in prf
                addensumIsZero x Z prf = rewrite plusCommutative 0 x in prf
        
                multLeftCancel : (x : Nat) -> (y : Nat) -> (z : Nat) -> S x * y = S x * z -> y = z
                multLeftCancel x (S k) (S j) prf =
                    let rec = multLeftCancel x k j in
                    rewrite rec (plusLeftCancel _ _ _ int) in Refl
                  where
                    int : x + (k + x * k) = x + (j + x * j)
                    int = rewrite plusAssociative x k (x * k) in
                        rewrite plusAssociative x j (x * j) in
                        rewrite plusCommutative x k in
                        rewrite plusCommutative x j in
                        rewrite sym $ plusAssociative k x (x * k) in
                        rewrite sym $ plusAssociative j x (x * j) in 
                        rewrite sym $ multRightSuccPlus x k in
                        rewrite sym $ multRightSuccPlus x j in succInjective _ _ prf
                multLeftCancel x y Z prf = addensumIsZero _ _ int
                  where
                    int : y + (x * y) = 0 * x
                    int = rewrite multCommutative 0 x in prf  
                multLeftCancel x Z z prf = sym $ addensumIsZero _ _ (sym int)
                  where
                    int : 0 * x = z + (x * z)
                    int = rewrite multCommutative 0 x in prf

               

               
        
RationalSetoid : Setoid
RationalSetoid = MkSetoid Rational RationalEq $ EqProof RationalEq rationalRefl rationalSymmetric rationalTransitive