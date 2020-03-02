module DomainForRecord

open System

/// constrained to be not null and at most 50 chars
type String50 = private String50 of string 

module String50 = 
    let create fieldName str : Result<String50,string> = 
        if String.IsNullOrEmpty(str) then
            Error (fieldName + " must be non-empty")
        elif str.Length > 50 then
            Error (fieldName + " must be less that 50 chars")
        else
            Ok (String50 str)
    let value (String50 v) = v

/// constrained to be bigger than 1/1/1900 and less than today's date
type Birthdate = private Birthdate of DateTime 

module Birthdate =           // functions for Birthdate
    let create fieldName date : Result<Birthdate, string> =
        if (date < DateTime.Parse("1/1/1900")) then
            Error (fieldName + " must be greater than 1/1/1900")
        else
            Ok (Birthdate date)
    let value (Birthdate date) = date  // value extractor

/// Domain type
type Person = {
    First: String50
    Last: String50
    Birthdate : Birthdate
    }