module DtoForComplicatedUnion

open System
open DomainForRecord
open DomainForComplicatedUnion
open ResultFunctions
open DtoError

/// Corresponding DTO types
type NameDto = {
    First : string
    Last : string
    }

type ExampleDto = {
    Tag : string // one of "A","B", "C", "D" 
    // no data for A case
    BData : Nullable<int>  // data for B case
    CData : string[]       // data for C case
    DData : NameDto        // data for D case 
    }

module ExampleDto =
    let nameDtoFromDomain (name:Name) :NameDto =
          let first = name.First |> String50.value
          let last = name.Last |> String50.value
          {First=first; Last=last}

    let fromDomain (domainObj:Example) :ExampleDto =
        let nullBData = Nullable()
        let nullCData = null
        let nullDData = Unchecked.defaultof<NameDto>
        match domainObj with
        | A -> 
            {Tag="A"; BData=nullBData; CData=nullCData; DData=nullDData}
        | B i ->
            let bdata = Nullable i
            {Tag="B"; BData=bdata; CData=nullCData; DData=nullDData}
        | C strList -> 
            let cdata = strList |> List.toArray
            {Tag="C"; BData=nullBData; CData=cdata; DData=nullDData}
        | D name -> 
            let ddata = name |> nameDtoFromDomain 
            {Tag="D"; BData=nullBData; CData=nullCData; DData=ddata}

    let domainToJson (example:Example) =
        example
        |> fromDomain
        |> Json.serialize

    let nameDtoToDomain (nameDto:NameDto) :Result<Name,string> =
      result {
        let! first = nameDto.First |> String50.create "First Name"
        let! last = nameDto.Last |> String50.create "Last Name"
        return {First=first; Last=last}
      }

    let toDomain dto : Result<Example,string> =
      match dto.Tag with
      | "A" -> 
        Ok A 
      | "B" -> 
        if dto.BData.HasValue then
            dto.BData.Value |> B |> Ok
        else
            Error "B data not expected to be null"
      | "C" -> 
        match dto.CData with
        | null -> 
            Error "C data not expected to be null"
        | _ -> 
            dto.CData |> Array.toList |> C |> Ok 
      | "D" -> 
        match box dto.DData with
        | null -> 
            Error "D data not expected to be null"
        | _ -> 
            dto.DData 
            |> nameDtoToDomain  // returns Result...
            |> Result.map D     // ...so must use "map"
      | _ ->
        // all other cases
        let msg = sprintf "Tag '%s' not recognized" dto.Tag 
        Error msg

    let jsonToDomain jsonString : Result<Example, DtoError> =
        result {
            let! deserializedValue =
                jsonString
                |> Json.deserialize
                |> Result.mapError DeserializationException

            let! domainValue =
                deserializedValue
                |> toDomain
                |> Result.mapError ValidationError

            return domainValue
        }