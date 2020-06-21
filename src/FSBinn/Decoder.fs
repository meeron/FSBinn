namespace FSBinn

open System
open System.Text

module private DecoderFunctions =
    let private fromVarint (data: byte[]) =
        let size = BitConverter.ToInt32([|data.[0]; 0uy; 0uy; 0uy|], 0)

        if size &&& 0x80 > 0 then
            (BitConverter.ToInt32(data.[0..3] |> Array.rev, 0) &&& 0x7FFFFFFF, 4)
        else
            (size, 1)

    let decodeString (data: byte[]) =
        let (size, position) = fromVarint data.[0..3]
        
        // We need to include string termination byte so size - 1
        data.[position..position + size - 1] |> Encoding.UTF8.GetString

    let decodeInt32 (data: byte[]) =
        BitConverter.ToInt32(data.[0..3], 0)

    let decodeByte (data: byte[]) =
        data.[0]

module Decoder =
    let private toGeneric<'T>(value: obj) =
        value :?> 'T

    let decode<'T> (data: byte[]): Option<'T> =
        if data.Length > 0 then
            match data.[0] with
            | BinnDataTypes.string when typeof<'T> = typeof<string> -> data.[1..] |> DecoderFunctions.decodeString |> toGeneric |> Some
            | BinnDataTypes.int32 when typeof<'T> = typeof<int> -> data.[1..] |> DecoderFunctions.decodeInt32 |> toGeneric |> Some
            | BinnDataTypes.uint8 when typeof<'T> = typeof<byte> -> data.[1..] |> DecoderFunctions.decodeByte |> toGeneric |> Some
            | _ -> None
        else
            None
