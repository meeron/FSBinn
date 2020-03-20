module EncoderTests

open System
open Xunit
open FSBinn.Encoder
open FSBinn.Decoder

[<Fact>]
let ``Encode string`` () =
    let value = "test"
    let result = value |> encode |> decode<string>

    Assert.Equal(value, result)

