namespace FSBinn

open System
open System.IO

module private EncoderFunctions =
    let private toVarint (value: int) = 
        if value > 127 then
            BitConverter.GetBytes(value ||| 0x80000000) |> Array.rev
        else
            [|(byte)value|]

    let private writeTo (stream: Stream) (bytes: byte[]) = stream.Write(bytes, 0, bytes.Length)

    let encodeString (value: string) =
        let stream = new MemoryStream()    
    
        [|BinnDataTypes.string|] |> writeTo stream
        value.Length |> toVarint |> writeTo stream
        value |> Text.Encoding.UTF8.GetBytes |> writeTo stream

        // Write string termination byte
        [|0uy|] |> writeTo stream

        stream.ToArray()

module Encoder =
    let encode (object: obj): byte[] =
        match object with
            | :? string as s -> EncoderFunctions.encodeString s
            | :? int as i -> Array.empty<byte>
            | _ -> Array.empty<byte>