namespace FSBinn

open System
open System.IO

open DataTypes

module Encoder =
    let private toVarint (value: int) = 
        if value > 127 then
            BitConverter.GetBytes(value ||| 0x80000000) |> Array.rev
        else
            [|(byte)value|]

    let private writeTo (stream: Stream) (bytes: byte[]) = stream.Write(bytes, 0, bytes.Length)

    let private encodeString (value: string) =
        let stream = new MemoryStream()    
        
        [|BINN_STRING|] |> writeTo stream
        value |> String.length |> toVarint |> writeTo stream
        value |> Text.Encoding.UTF8.GetBytes |> writeTo stream

        stream.ToArray()

    let encode (object: obj): byte[] =
        match object with
            | :? string as s -> encodeString s
            | :? int as i -> Array.empty<byte>
            | _ -> Array.empty<byte>