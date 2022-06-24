module App
open Feliz
open Feliz.DaisyUI

type State =
    { Count: int }

type Msg =
    | Increment
    | Decrement

let defaultValue count =
    if count = null then
        0
    else
        count |> int

let init() =
    { Count = Browser.WebStorage.localStorage.getItem("count") |> defaultValue }

let update (msg: Msg) (state: State): State =
    match msg with
    | Increment ->
        Browser.WebStorage.localStorage.setItem("count", string(state.Count + 1))
        { state with Count = state.Count + 1 }

    | Decrement ->
        Browser.WebStorage.localStorage.setItem("count", string(state.Count - 1))
        { state with Count = state.Count - 1 }

let render (state: State) (dispatch: Msg -> unit) =
  Html.div [
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