module DtoError

type DtoError =
    | ValidationError of string
    | DeserializationException of exn