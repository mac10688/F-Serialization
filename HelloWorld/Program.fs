module Main
open System
open ResultFunctions
open DomainForRecord
open DtoForRecord
open DomainForComplicatedUnion
open DtoForComplicatedUnion


[<EntryPoint>]
let main argv =
    let (basicRecord: Result<Person, string>) = result {
                    let! first = String50.create "Name" "Alex"
                    let! last = String50.create "Last" "Adams"
                    let! birthdate = Birthdate.create "Birth" (DateTime(1980,1,1))
                    return {
                        First =  first
                        Last = last
                        Birthdate = birthdate
                    }
                }
    // input to test with
    
    
    // use the serialization pipeline
    let basicRecordJson = 
        match basicRecord with
            | Ok s -> DtoForRecord.jsonFromDomain s
            | Error s -> s
    
    Console.WriteLine basicRecordJson

    // JSON string to test with
    let jsonPerson = """{ "First": "Alex", "Last": "Adams", "Birthdate": "1980-01-01T00:00:00" }"""
    
    // call the deserialization pipeline
    jsonToDomain jsonPerson |> printfn "%A"
    
    // The output is:
    // Ok {First = String50 "Alex";
    // Last = String50 "Adams";
    // Birthdate = Birthdate 01/01/1980 00:00:00;}
    
    // The output is
    // "{"First":"Alex","Last":"Adams","Birthdate":"1980-01-01T00:00:00"}"

    let jsonPersonWithErrors = """{ "First": "", "Last": "Adams", "Birthdate": "1776-01-01T00:00:00" }"""
    
    // call the deserialization pipeline
    jsonToDomain jsonPersonWithErrors |> printfn "%A"
    
    // The output is:
    // Error (ValidationError [
    // "First must be non-empty" 
    // ])

    let (name: Result<Name, string>) = result {
        let! first = String50.create "Name" "Alex"
        let! last = String50.create "Last" "Adams"
        return {
            First =  first
            Last = last
        }
    }

    let exampleComplicatedJson = 
        name
        //|> Result.map (fun n -> Example.D n)
        |> Result.map Example.D
        |> Result.map ExampleDto.domainToJson

    match exampleComplicatedJson with
        | Ok exampleJson -> printfn "%s" exampleJson
        | Error s -> printfn "%s" s

    let exampleComplicatedDomain =
        exampleComplicatedJson
        |> Result.map ExampleDto.jsonToDomain

    match exampleComplicatedDomain with
        | Ok domain -> printfn "%A" domain
        | Error s -> printfn "%s" s

     

    0 // return an integer exit code