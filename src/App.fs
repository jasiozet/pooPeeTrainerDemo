module App

open Elmish
open Feliz
open Feliz.DaisyUI
open Logic
open GraphHelper
open State
open LocalStorage
open DownloadUpload
open Fable.SimpleJson

type Msg =
  | NewEvent of EventType
  | DeleleteLastEvent
  | SwitchPresentationType of PresentationType
  | SaveState
  | IsFileUploadVisible of bool
  | UploadState of string

let ButtonForNewEvent dispatch eventType =
  Daisy.button.button [
    prop.onClick (fun _ -> dispatch (NewEvent eventType))
    prop.text (GetIconForEventType eventType)
  ]

let init() =
  RetrieveStateFromLocalStorage,
  Cmd.none

let update (msg: Msg) (state: State): State * Cmd<Msg> =
  match msg with
  | NewEvent eventType -> {state with Events=(GetNewEventNow eventType)::state.Events}, Cmd.ofMsg SaveState
  | SaveState ->
      StoreStateInLocalStorage state
      state, Cmd.none
  | DeleleteLastEvent -> {state with Events= (List.tail state.Events)}, Cmd.ofMsg SaveState
  | SwitchPresentationType presentationType -> {state with PresentationType=presentationType}, Cmd.none
  | IsFileUploadVisible isIt -> {state with IsFileUploadVisible=isIt}, Cmd.none
  | UploadState s -> TryParseState s, Cmd.ofMsg (IsFileUploadVisible false)

let render (state: State) (dispatch: Msg -> unit) =
  Html.div [
    prop.style [
      style.alignItems.center
      style.display.flex
      style.flexDirection.column
      style.flexWrap.nowrap
      style.marginTop 10
    ]
    prop.children [

      //Event buttons
      Html.div [
        ButtonForNewEvent dispatch Poo
        ButtonForNewEvent dispatch Pee
        ButtonForNewEvent dispatch Play
        ButtonForNewEvent dispatch Eat
        ButtonForNewEvent dispatch Walk
        ButtonForNewEvent dispatch Sleep

        Daisy.button.button [
          prop.onClick (fun _ -> dispatch DeleleteLastEvent)
          prop.text ("❌ last")
        ]
      ]
      // Pup info
      Html.div [
        prop.classes [ "w-96" ]
        prop.style [ style.marginTop 10]
        prop.children [
          Daisy.card [
            prop.children [
              Html.figure [
                Html.img [
                  mask.circle
                  prop.src "lusiaC.png"
                  prop.onClick (fun _ -> Browser.Dom.window.alert("Try petting the real Lusia 💛"))
                ]
              ]
              Daisy.cardBody [
                prop.style [ style.alignItems.center]
                prop.children [
                  Daisy.cardTitle (state.Pup.Name + " " + GetEmojiForGender state.Pup.Gender)
                  Daisy.cardTitle (GetFormattedDogAge state.Pup.Birthday)
                ]
              ]
            ]
          ]
        ]
      ]

      //Buttons for event handling logic
      Html.div [
        Daisy.button.button [
          prop.onClick (fun _ -> dispatch (SwitchPresentationType EventStack))
          prop.text ("📋 list")
        ]
        Daisy.button.button [
          prop.onClick (fun _ ->  dispatch (SwitchPresentationType Graph))
          prop.text ("📈 graph")
        ]
        Daisy.button.button [
          prop.onClick (fun _ -> Download "pooPeeTrainer.json" (Json.serialize state))
          prop.text ("💾 save")
        ]
        Daisy.button.button [
          prop.onClick (fun _ -> dispatch (IsFileUploadVisible true))
          prop.text ("📤 upload")
        ]
        if state.IsFileUploadVisible then
          CreateFileUpload (fun x -> dispatch (UploadState x))
      ]

      //Event presentation:
      Html.div [
        prop.children [
          if state.PresentationType = EventStack then
            Html.div [
              prop.style [style.marginTop 10]
              prop.children [
              for event in state.Events do
                Html.div [
                  Daisy.labelText (GetIconForEventType event.Type)
                  let formattedTime = event.Time.ToString("dd.MM HH:mm ")
                  Daisy.labelText ($" {formattedTime}")
                  Daisy.labelText (GetIconForEventType event.Type)
                ]
              ]
            ]
          else
            Html.div [
              Charts(state.Events)
            ]
        ]
      ]
    ]
  ]
