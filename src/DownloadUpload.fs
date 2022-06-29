module DownloadUpload
open Fable.Core.JsInterop
open Feliz

let Download fileName fileContent =
    let anchor = Browser.Dom.document.createElement "a"
    let encodedContent = fileContent |> sprintf "data:text/plain;charset=utf-8,%s" |> Fable.Core.JS.encodeURI
    anchor.setAttribute("href",  encodedContent)
    anchor.setAttribute("download", fileName)
    anchor.click()

let HandleFileEvent onLoad (fileEvent:Browser.Types.Event) =
    let files:Browser.Types.FileList = !!fileEvent.target?files
    if files.length > 0 then
        let reader = Browser.Dom.FileReader.Create()
        reader.onload <- (fun _ -> reader.result |> unbox |> onLoad)
        reader.readAsText(files.[0])

let CreateFileUpload onLoad =
  Html.input [
    prop.type'.file
    prop.onChange (HandleFileEvent onLoad)
  ]