module DtoForList

open System

/// Corresponding DTO type 
type OrderLineDto = {
    OrderLineId : int
    ProductCode : string
    Quantity : Nullable<int>
    Description : string 
    }

/// Corresponding DTO type
type OrderDto = {
    Lines : OrderLineDto[] 
    }