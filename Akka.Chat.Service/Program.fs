// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp

open System
open Akkling
open My.Akka.Chat

[<EntryPoint>]
let main _ =
    printfn "Application started"
    let system = System.create "akka-chat-system" <| Configuration.defaultConfig()
    let chat = spawn system "chat-server" <| (props Chat.chatServer)
    let users = spawn system "user-repository" <| (props User.userProvider)
    

    // // let mutable shouldContinue = true

    // // while shouldContinue do
    // //     Console.ReadLine()
    // //     |> function
    // //     | 

    0