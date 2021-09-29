module Akka.Chat.Service.Tests

open NUnit.Framework

[<SetUp>]
let Setup () =
    ()

[<Test>]
let ``Parse valid guest should work`` () =
    "/guest Robert"
    |> ConsoleInput.parseMessage
    |> function
    | Some (ConsoleInput.AddGuest "Robert") -> ()
    | Some (ConsoleInput.AddGuest otherName) -> sprintf "Wrong name \"%s\" was found" otherName |> invalidOp |> raise
    | Some other -> sprintf "Wrong case %A was found" other |> invalidOp |> raise
    | None -> invalidOp "No command was parsed" |> raise

[<Test>]
let ``Parse valid assistant should work`` () =
    "/assistant Jack"
    |> ConsoleInput.parseMessage
    |> function
    | Some (ConsoleInput.AddAssistant "Jack") -> ()
    | Some (ConsoleInput.AddAssistant otherName) -> sprintf "Wrong name \"%s\" was found" otherName |> invalidOp |> raise
    | Some other -> sprintf "Wrong case %A was found" other |> invalidOp |> raise
    | None -> invalidOp "No command was parsed" |> raise

[<Test>]
let ``Parse assignation should work`` () =
    "/assign Jack Bob"
    |> ConsoleInput.parseMessage
    |> function
    | Some (ConsoleInput.AssignAssistant ("Jack", "Bob")) -> ()
    | Some (ConsoleInput.AssignAssistant (a, b)) -> sprintf "Wrong names \"%s\", \"%s\" were found" a b |> invalidOp |> raise
    | Some other -> sprintf "Wrong case %A was found" other |> invalidOp |> raise
    | None -> invalidOp "No command was parsed" |> raise

[<Test>]
let ``Parse assignation without enough arguments should return nothing`` () =
    "/assign Jack"
    |> ConsoleInput.parseMessage
    |> function
    | None -> ()
    | other -> sprintf "Wrong parsed value %A was found" other |> invalidOp |> raise

[<Test>]
let ``Parse just text message should return nothing`` () =
    "some message"
    |> ConsoleInput.parseMessage
    |> function
    | None -> ()
    | other -> sprintf "Wrong parsed value %A was found" other |> invalidOp |> raise
