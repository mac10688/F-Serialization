module DtoForUnionEnum

/// Corresponding DTO type
type ColorDto = 
    | Red = 1
    | Green = 2
    | Blue = 3

/// Corresponding DTO types
type SuitDto = Heart = 1 | Spade = 2 | Diamond = 3 | Club = 4 
type RankDto = Ace = 1 | Two = 2 | Queen = 12 | King = 13
type CardDto = {
    Suit : SuitDto
    Rank : RankDto
    }