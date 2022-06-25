module App
open Elmish
open Feliz
open Feliz.DaisyUI
open Logic
open System

type State = {
    Count: int
    Pup : Pup
    Events : Event list
}

type Msg =
    | Increment
    | Decrement
    | SaveState

let defaultValue count =
    if count = null then
        0
    else
        count |> int

let storeStateInLocalStorage state =
    Browser.WebStorage.localStorage.setItem("count", string(state.Count))

let init() =
    {
      Count = Browser.WebStorage.localStorage.getItem("count") |> defaultValue;
      Pup = {Name = "Lusia"; Birthday=DateOnly(04,05,2022); Gender=Gender.Female};
      Events = []
    },
    Cmd.none

let update (msg: Msg) (state: State): State * Cmd<Msg> =
    match msg with
    | Increment ->
        { state with Count = state.Count + 1 }, Cmd.ofMsg SaveState
    | Decrement ->
        { state with Count = state.Count - 1 }, Cmd.ofMsg SaveState
    | SaveState ->
        storeStateInLocalStorage state
        state, Cmd.none

let render (state: State) (dispatch: Msg -> unit) =
  Html.div [
    Html.div [
      prop.classes [ "w-96" ]
      prop.children [
        Daisy.card [
          card.bordered
          card.full
          prop.children [
            Html.figure [
              Html.text "🐕"
            ]
            Daisy.cardBody [
              prop.style [ style.alignItems.center ]
              prop.children [
                Daisy.cardTitle state.Pup.Name
              ]
            ]
          ]
        ]
      ]
    ]
    Daisy.button.button [
      prop.onClick (fun _ -> dispatch Increment)
      prop.text "Increment"
    ]

    Daisy.button.button [
      prop.onClick (fun _ -> dispatch Decrement)
      prop.text "Decrement"
    ]

    Html.h1 state.Count
  ]