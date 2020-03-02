module DomainForMapped

type ProductCode = ProductCode of string

/// Domain types
type OrderLineId = OrderLineId of int
type OrderLineQty = OrderLineQty of int
type OrderLine = {
    OrderLineId : OrderLineId
    ProductCode : ProductCode
    Quantity : OrderLineQty option
    Description : string option
    }

/// Domain type
type Price = Price of decimal
type PriceLookup = Map<ProductCode,Price>

/// Domain type
type Order = {
    Lines : OrderLine list
    }