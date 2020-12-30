module BikeHackers.App.Api

open FsToolkit.ErrorHandling
open Fable.SimpleHttp

#if FABLE_COMPILER
open Thoth.Json
#else
open Thoth.Json.Net
#endif

open BikeHackers.Components
open BikeHackers.Components.Thoth

let fetchTyres =
  asyncResult {
    let! response = Http.get "/tyres.json"

    let statusCode, content = response

    match statusCode with
    | 200 ->
      let! tyres = Decode.fromString (Decode.list Decode.tyre) content

      return tyres
    | _ ->
      return! Error (sprintf "%i: %s" statusCode content)
  }
