module BikeHackers.App.TyreQueryDesigner

open Fable.Core.JsInterop
open Elmish
open Browser.Types
open Feliz
open BikeHackers.Components

type Model =
  {
    BeadSeatDiameter : WheelBsd
    MinimumWidth : int
    MaximumWidth : int
    Types : Set<TyreType>
    FilterApplications : bool
    Applications : Set<TyreApplication>
  }

type Message =
  | UpdateModel of (Model -> Model)

let init () =
  {
    BeadSeatDiameter = WheelBsd.W700C
    MinimumWidth = 40
    MaximumWidth = 42
    Types = Set.ofSeq [ TyreType.Tubeless ]
    FilterApplications = false
    Applications = Set.ofSeq [ TyreApplication.LightGravel; TyreApplication.RoughGravel ]
  }

let update msg model =
  let (UpdateModel f) = msg
  f model, Cmd.none

let private tyreTypeCheckbox model dispatch tyreType =
  let id = "checkbox-type-" + (string tyreType).ToLowerInvariant ()
  [
    Html.input [
      prop.id id
      prop.type'.checkbox
      prop.isChecked (model.Types |> Set.contains tyreType)
      prop.onCheckedChange
        (fun isChecked ->
          dispatch
            (UpdateModel (fun model ->
              {
                model with
                  Types =
                    if isChecked
                    then
                      Set.add tyreType model.Types
                    else
                      Set.remove tyreType model.Types
                        })))
    ]
    Html.label [
      prop.htmlFor id
      prop.children [ Html.span (string tyreType) ]
    ]
  ]

let private bsdRadio model dispatch bsd =
  let id = "radio-bsd-" + (string bsd).ToLowerInvariant ()

  let isChecked = model.BeadSeatDiameter = bsd

  let friendlyName =
    match bsd with
    | W700C -> "700c (29er)"
    | W650B -> "650b (27.5)"
    | W26 -> "26\""

  [
    Html.input [
      prop.id id
      prop.name "bsd"
      prop.type'.radio
      prop.isChecked isChecked
      prop.onCheckedChange
        (fun _ ->
          dispatch
            (UpdateModel (fun model -> { model with BeadSeatDiameter = bsd })))
    ]
    Html.label [
      prop.htmlFor id
      prop.children [ Html.span friendlyName ]
    ]
  ]

let private tyreApplicationCheckbox model dispatch tyreApplication =
  let id = "checkbox-application-" + (string tyreApplication).ToLowerInvariant ()

  let label =
    match tyreApplication with
    | TyreApplication.Track -> "Track"
    | TyreApplication.Road -> "Road"
    | TyreApplication.RoughRoad -> "Rough Road"
    | TyreApplication.LightGravel -> "Light Gravel"
    | TyreApplication.RoughGravel -> "Rough Gravel"
    | TyreApplication.Singletrack -> "Singletrack"

  [
    Html.input [
      prop.id id
      prop.type'.checkbox
      prop.disabled (not model.FilterApplications)
      prop.isChecked (model.Applications |> Set.contains tyreApplication)
      prop.onCheckedChange
        (fun isChecked ->
          dispatch
            (UpdateModel (fun model ->
              {
                model with
                  Applications =
                    if isChecked
                    then
                      Set.add tyreApplication model.Applications
                    else
                      Set.remove tyreApplication model.Applications
                        })))
    ]
    Html.label [
      prop.htmlFor id
      prop.disabled (not model.FilterApplications)
      prop.children [ Html.span label ]
    ]
  ]

let view model dispatch =
  Html.div [

    Html.h3 "Type"

    yield! tyreTypeCheckbox model dispatch TyreType.Tubeless
    yield! tyreTypeCheckbox model dispatch TyreType.Clincher
    yield! tyreTypeCheckbox model dispatch TyreType.Tubular

    Html.h3 "Rim Diameter"

    yield! bsdRadio model dispatch WheelBsd.W700C
    yield! bsdRadio model dispatch WheelBsd.W650B
    yield! bsdRadio model dispatch WheelBsd.W26

    Html.h3 "Tyre Width (mm)"

    Html.input [
      prop.type'.number
      prop.value model.MinimumWidth
      prop.min 18
      prop.max 100
      prop.onChange
        (fun (e : Event) ->
          let x = int (e?target?value : float)

          dispatch
            (UpdateModel (fun model ->
              { model with
                  MinimumWidth = x
                  MaximumWidth = max x model.MaximumWidth
                  })))
    ]

    Html.input [
      prop.type'.number
      prop.value model.MaximumWidth
      prop.min 18
      prop.max 100
      prop.onChange
        (fun (e : Event) ->
          let x = int (e?target?value : float)

          dispatch
            (UpdateModel (fun model ->
              { model with
                  MinimumWidth = min x model.MinimumWidth
                  MaximumWidth = x
                  })))
    ]

    Html.h3 "Application"

    Html.input [
      prop.id "checkbox-application-filter"
      prop.type'.checkbox
      prop.isChecked (model.FilterApplications)
      prop.onCheckedChange
        (fun isChecked ->
          dispatch
            (UpdateModel (fun model ->
              {
                model with
                  FilterApplications = not model.FilterApplications })))
    ]
    Html.label [
      prop.htmlFor "checkbox-application-filter"
      prop.children [ Html.span "Filter by application?" ]
    ]

    let applications =
      [
        TyreApplication.Track
        TyreApplication.Road
        TyreApplication.RoughRoad
        TyreApplication.LightGravel
        TyreApplication.RoughGravel
        TyreApplication.Singletrack
      ]

    Html.div
      (seq {
        for application in applications do
          yield! tyreApplicationCheckbox model dispatch application
      }
      |> Seq.toList)
  ]
