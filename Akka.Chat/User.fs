namespace My.Akka.Chat

module User =
    open System
    open System.Collections.Generic
    open Akkling

    type Participant = {
        Id: Guid
        Name: string
        Description: string option
    }

    type Message =
    | Create of Participant
    | Read of Guid

    let userProvider (ctx: Actor<Message>) =
        let rec loop (participants : Dictionary<Guid, Participant>) = actor {
            let! message = ctx.Receive()
            match message with
            | Create participant ->
                participants.[participant.Id] <- participant
                return! loop participants
            | Read id ->
                ctx.Sender() <! participants.[id]
                ignored()
        }

        loop <| Dictionary<Guid, Participant>()