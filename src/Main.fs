module Main

open Elmish
open Elmish.React

Fable.Core.JsInterop.importSideEffects "./index.css"

Program.mkSimple App.init App.update App.render
|> Program.withConsoleTrace
|> Program.withReactSynchronous "elmish-app"
|> Program.run