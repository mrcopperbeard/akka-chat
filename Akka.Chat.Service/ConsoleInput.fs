module ConsoleInput

type ConsoleCommand =
    | AddGuest of string
    | AddAssistant of string
    | AssignAssistant of string * string

let parseMessage =
    let tryParse (skipping: string) (message: string) =
        let index = message.IndexOf(skipping)
        if index <> -1 then Some <| message.Substring(skipping.Length).Trim() else None

    let getGuest message = message |> tryParse "/guest" |> Option.map AddGuest
    let getAssistant message () = message |> tryParse "/assistant" |> Option.map AddAssistant
    let getAssignation =
        let parseAssignation (input: string) =
            let parts = input.Split([|' '|])
            if parts.Length < 2 then None else Some (parts.[0], parts.[1])
        fun message () ->
            message
            |> tryParse "/assign"
            |> Option.bind parseAssignation
            |> Option.map AssignAssistant

    fun message ->
        getGuest message
        |> Option.orElseWith (getAssistant message)
        |> Option.orElseWith (getAssignation message)

