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

    let ecnodeInt32 (value: int): byte[] =
        BitConverter.GetBytes value |> Array.append [|BinnDataTypes.int32|]

    let ecnodeByte(value: byte): byte[] =
        [|value|] |> Array.append [|BinnDataTypes.uint8|]

module Encoder =
    let encode (object: obj): byte[] =
        match object with
            | :? string as s -> EncoderFunctions.encodeString s
            | :? int as i -> EncoderFunctions.ecnodeInt32 i
            | :? byte as b -> EncoderFunctions.ecnodeByte b
            | _ -> [||]
