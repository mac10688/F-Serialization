module DtoForMapped

/// DTO type to represent a map
type PriceLookupPairDto = {
    Key : string
    Value : decimal
    }

type PriceLookupDto = {
    KVPairs : PriceLookupPairDto []
    }