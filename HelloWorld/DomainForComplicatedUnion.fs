module DomainForComplicatedUnion

open DomainForRecord

/// Domain types
type Name = {
    First : String50
    Last : String50
    }

type Example = 
    | A 
    | B of int
    | C of string list
    | D of Name