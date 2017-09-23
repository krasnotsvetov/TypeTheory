module IntegerSetoid

import Setoid
import Integer

%access public export


data MyIntegerEq : MyInteger -> MyInteger -> Type where
    MyIntegerRefl : (eq : a + d = c + b) -> MyIntegerEq (Sub a b) (Sub c d)

myIntegerRefl : Reflx MyIntegerEq
myIntegerRefl (Sub a b) = MyIntegerRefl $ Refl {x = a + b}

myIntegerTransitive : Trans MyIntegerEq
myIntegerTransitive (Sub a b) (Sub c d) (Sub e f) (MyIntegerRefl e1) (MyIntegerRefl e2) = MyIntegerRefl commonReduce
  where
    e3  : (a + d) + (c + f) = (c + b) + (e + d)
    e3  = rewrite e1 in rewrite e2 in Refl
    
    commonRegroup : ((c + f) + a) + d = ((c + b) + e) + d
    commonRegroup = trans (sym changePriority2) $ trans (sym swapBrackets) $ trans e3 changePriority
        where
            swapBrackets : (a + d) + (c + f) = (c + f) + (a + d)
            swapBrackets = plusCommutative (a + d) (c + f) -- el for eliminate
        
            changePriority : (c + b) + (e + d) = ((c + b) + e) + d
            changePriority = plusAssociative (c + b) e d
        
            changePriority2 : (c + f) + (a + d) = ((c + f) + a) + d
            changePriority2 = plusAssociative (c + f) a d
    -- final transitive
    commonReduce : a + f = e + b
    commonReduce = trans swapOperands3 $ plusCommutative b e where
        reduceD  : (c + f) + a = (c + b) + e
        reduceD  = plusRightCancel ((c + f) + a) ((c + b) + e) d commonRegroup

        swapBrackets : c + (f + a) = (c + b) + e
        swapBrackets = trans (plusAssociative c f a) reduceD

        swapOperands : (f + a) + c = (c + b) + e
        swapOperands = trans (plusCommutative (f + a) c) swapBrackets

        swapBrackets2 : (f + a) + c = c + (b + e)
        swapBrackets2 = trans swapOperands $ sym $ plusAssociative c b e

        swapOperands2 : (f + a) + c = (b + e) + c
        swapOperands2 = trans swapBrackets2 $ plusCommutative c (b + e)

        swapOperands3 : a + f = b + e
        swapOperands3 = trans (plusCommutative a f) reduceE where
            reduceE  : f + a = b + e
            reduceE  = plusRightCancel (f + a) (b + e) c swapOperands2

myIntegerSymmetric : Sym MyIntegerEq
myIntegerSymmetric (Sub a b) (Sub c d) (MyIntegerRefl eq) = MyIntegerRefl $ sym eq

MyIntegerSetoid : Setoid
MyIntegerSetoid = MkSetoid MyInteger MyIntegerEq $ EqProof MyIntegerEq myIntegerRefl myIntegerSymmetric myIntegerTransitive