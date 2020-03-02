module DomainForSimpleUnion

/// Domain type
type Color = 
    | Red
    | Green
    | Blue

/// Domain types
type Suit = Heart | Spade | Diamond | Club
type Rank = Ace | Two | Queen | King // incomplete for clarity
type Card = Suit * Rank  // <---- a tuple
