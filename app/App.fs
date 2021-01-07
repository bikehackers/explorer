module BikeHackers.App.Entry

open FsToolkit.ErrorHandling
open Elmish
open Elmish.React
open Feliz
open BikeHackers.Components
open BikeHackers.App

type Model =
  {
    TyreFetchError : string option
    IsLoading : bool
    Tyres : Tyre list
    QueryDesigner : TyreQueryDesigner.Model
  }

type Message =
  | FetchTyres
  | UpdateModel of (Model -> Model)
  | QueryDesigner of TyreQueryDesigner.Message

let init () =
  {
    TyreFetchError = None
    IsLoading = false
    Tyres = []
    QueryDesigner = TyreQueryDesigner.init ()
  }, Cmd.ofMsg FetchTyres

let update msg model =
  match msg with
  | FetchTyres ->
    {
      model with
        IsLoading = true
    },
    (Cmd.OfAsync.result
      (async {
        let! result = Api.fetchTyres

        return
          UpdateModel (fun model ->
            {
              model with
                IsLoading = false
                TyreFetchError = Result.toErrorOption result
                Tyres =
                  match result with
                  | Ok tyres -> tyres
                  | _ -> model.Tyres
            })
      }))
  | UpdateModel f ->
    f model, Cmd.none
  | QueryDesigner m ->
    let qd, cmd = TyreQueryDesigner.update m model.QueryDesigner

    {
      model with
        QueryDesigner = qd
    }, (cmd |> Cmd.map Message.QueryDesigner)

let view model dispatch =
  Html.main [
    Html.h1 "Tyre Search"

    if model.IsLoading
    then
      Html.section [
        Html.p "Loading... "
      ]
    else
      let filteredTyres =
        model.Tyres
        |> Seq.collect (fun tyre ->
          tyre.Sizes
          |> Seq.map (fun tyreSize -> tyre, tyreSize)
        )
        |> Seq.filter (fun (tyre, tyreSize) ->
          tyreSize.BeadSeatDiameter = WheelBsd.toMillimeters model.QueryDesigner.BeadSeatDiameter
          && Set.contains tyreSize.Type model.QueryDesigner.Types
          && model.QueryDesigner.MinimumWidth <= tyreSize.Width
          && model.QueryDesigner.MaximumWidth >= tyreSize.Width
          && (
              not model.QueryDesigner.FilterApplications
              || (
                tyre.Application
                |> Option.defaultValue Set.empty
                |> Set.intersect model.QueryDesigner.Applications
                |> Seq.isEmpty
                |> not)
            )
        )
        |> Seq.toList

      Html.section [
        TyreQueryDesigner.view model.QueryDesigner (Message.QueryDesigner >> dispatch)

        Html.h2 "Results"
        TyreTable.view filteredTyres
      ]
  ]

Program.mkProgram init update view
|> Program.withReactSynchronous "root"
|> Program.run
