module Setoid

%default total
%access public export

Reflx : {A : Type} -> (R : A -> A -> Type) -> Type
Reflx {A} R = (x : A) -> R x x

Sym : {A : Type} -> (R : A -> A -> Type) -> Type
Sym {A} R = (x : A) -> (y : A) -> R x y -> R y x

Trans : {A : Type} -> (R : A -> A -> Type) -> Type
Trans {A} R = (x : A) -> (y : A) -> (z : A) -> R x y -> R y z -> R x z

data IsEquivalence : {A : Type} -> (R : A -> A -> Type) -> Type where
    EqProof : {A : Type} -> (R : A -> A -> Type) -> Reflx {A} R -> Sym {A} R -> Trans {A} R -> IsEquivalence {A} R

record Setoid where
    constructor MkSetoid
    Carrier : Type
    Equiv : Carrier -> Carrier -> Type
    EquivProof : IsEquivalence Equiv
