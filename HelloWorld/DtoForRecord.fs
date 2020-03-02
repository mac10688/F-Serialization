module DtoForRecord

    open System
    open DomainForRecord
    open ResultFunctions
    open DtoError

    type PersonDto = {
        First: string
        Last: string
        Birthdate : DateTime
        }

    /// create a DTO from a domain object
    let fromDomain (person:Person) : PersonDto =
        // get the primitive values from the domain object
        let first = person.First |> String50.value
        let last = person.Last |> String50.value
        let birthdate = person.Birthdate |> Birthdate.value 
      
        // combine the components to create the DTO
        {First = first; Last = last; Birthdate = birthdate}


          /// create a domain object from a DTO
    let toDomain (dto:PersonDto) :Result<Person,string> =
        result {
            // get each (validated) simple type from the DTO as a success or failure 
            let! first = dto.First |> String50.create "First"
            let! last = dto.Last |> String50.create "Last"
            let! birthdate = dto.Birthdate |> Birthdate.create "Birthdate"
            // combine the components to create the domain object
            return {
                First = first
                Last = last
                Birthdate = birthdate
            }
        }

    /// Serialize a Person into a JSON string
    let jsonFromDomain (person:Person) = 
        person
        |> fromDomain
        |> Json.serialize

    /// Deserialize a JSON string into a Person 
    let jsonToDomain jsonString :Result<Person, DtoError> = 
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