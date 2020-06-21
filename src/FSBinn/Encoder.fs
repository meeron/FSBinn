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

    let encodeInt16(value: int16) =
        value |> BitConverter.GetBytes |> Array.append [|BinnDataTypes.int16|]

    let encodeUInt32(value: uint32) =
        value |> BitConverter.GetBytes |> Array.append [|BinnDataTypes.uint32|]

module Encoder =
    let encode (object: obj): byte[] =
        match object with
            | :? string as s -> EncoderFunctions.encodeString s
            | :? int as i -> EncoderFunctions.ecnodeInt32 i
            | :? byte as b -> EncoderFunctions.ecnodeByte b
            | :? int16 as i16 -> EncoderFunctions.encodeInt16(i16)
            | :? uint32 as ui32 -> EncoderFunctions.encodeUInt32(ui32)
            | _ -> [||]
