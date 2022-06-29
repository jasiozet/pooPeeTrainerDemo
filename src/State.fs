module State
open System
open Logic

type PresentationType = EventStack | Graph

type State = {
  Pup : Pup
  Events : Event list
  PresentationType : PresentationType
  IsFileUploadVisible : bool
}

let getBasicState =
  {
    Pup = {Name = "Lusia"; Birthday=DateOnly(2022,05,04); Gender=Female};
    PresentationType = EventStack;
    Events = [];
    IsFileUploadVisible = false;
  }
