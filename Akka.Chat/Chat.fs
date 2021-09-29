namespace My.Akka.Chat
open System
open System.Collections.Generic

type Message = {
    ChatId: Guid
    AuthorId: Guid
    Text: string
    Sent: DateTimeOffset
}

type ChatMessage =
| Say of Message
| GetHistory

type ChatServerMessage =
| Say of Message
| GetHistory of Guid
| Create

module Chat =
    open Akkling

    let private serve (ctx: Actor<ChatMessage>) =
        let rec loop history = actor {
            let! command = ctx.Receive()
            match command with
            | ChatMessage.Say message -> return! loop (message :: history)
            | ChatMessage.GetHistory ->
                ctx.Sender () <! history
                ignored ()
        }

        loop []

    let chatServer (ctx: Actor<ChatServerMessage>) =
        let rec loop (chats: IDictionary<Guid, IActorRef<ChatMessage>>) = actor {
            let! message = ctx.Receive()
            match message with
            | Create ->
                let chatId = Guid.NewGuid()
                let chat = spawn ctx $"chat-{chatId}" <| (props serve)
                chats.Add(chatId, chat)
                ctx.Sender () <! chatId

                return! loop chats
            | Say msg -> chats.[msg.ChatId] <! ChatMessage.Say msg
            | GetHistory chatId ->
                let history = chats.[chatId] <? ChatMessage.GetHistory
                ctx.Sender () <! history
                ignored()
        }

        new Dictionary<Guid, _> () |> loop