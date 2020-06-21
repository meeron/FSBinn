module EncoderTests

open System
open Xunit
open FSBinn.Encoder
open FSBinn.Decoder

[<Fact>]
let ``Encode string`` () =
    let value = "May The Force be with you"
    let longValue = "May The Force be with you May The Force be with you May The Force be with you May The Force be with you May The Force be with you May The Force be with you"

    let result = value |> encode |> decode
    let resultLong = longValue |> encode |> decode

    Assert.Equal(Some value, result)
    Assert.Equal(Some longValue, resultLong)

[<Fact>]
let ``Encode int32`` () =
    let value = 234234

    let result = value |> encode |> decode<int>

    Assert.Equal(Some value, result)

[<Fact>]
let ``Encode byte`` () =
    let value = byte 123

    let result = value |> encode |> decode<byte>

    Assert.Equal(Some value, result)
