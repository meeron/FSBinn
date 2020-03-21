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

module Decoder =
    type DecodedValue<'T> = Value of 'T | None

    let decode (data: byte[]) =
        if data.Length > 0 then
            match data.[0] with
                | BinnDataTypes.string -> Value (DecoderFunctions.decodeString data.[1..])
                | _ -> None
        else
            None