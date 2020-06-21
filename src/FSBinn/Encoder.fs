namespace FSBinn

open System

module private EncoderFunctions =
    let private toVarint (value: int) = 
        if value > 127 then
            BitConverter.GetBytes(value ||| 0x80000000) |> Array.rev
        else
            [|(byte)value|]

    let encodeString (value: string): byte[] =
        (Text.Encoding.UTF8.GetBytes value)
            |> Array.append (toVarint value.Length)
            |> Array.append [|BinnDataTypes.string|]

module Encoder =
    let encode (object: obj): byte[] =
        match object with
            | :? string as s -> EncoderFunctions.encodeString s
            | :? int as i -> Array.empty<byte>
            | _ -> Array.empty<byte>