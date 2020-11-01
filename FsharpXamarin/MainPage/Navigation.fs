namespace FsharpXamarin.MainPage

open Fabulous
open Fabulous.XamarinForms
open FsharpXamarin.Pages

module Navigation = 
    type Message =
        | GoToLoginPage
        | GoHomePage
        | PopPage 
        | PagePopped 
        | ReplacePage of string
        | PushPage of string

    type CmdMsg = Nothing

    type Model =
        { LoggedIn: bool
          PageStack: string option list }

    let mapToCmd _ = Cmd.none

    let init () =
        { LoggedIn = false; PageStack = [Some "Home" ] }

    let update msg model =
        match msg with        
        | GoToLoginPage ->
            { model with PageStack = [ Some "Login" ] }, []
        | GoHomePage -> 
            { model with PageStack = [ Some "Home"  ] }, []
        | PagePopped -> 
            if model.PageStack |> List.exists Option.isNone then 
                { model with PageStack = model.PageStack |> List.filter Option.isSome }, []
            else
                { model with PageStack = (match model.PageStack with [] -> model.PageStack | _ :: t -> t) }, []
        | PopPage -> 
            { model with PageStack = (match model.PageStack with [] -> model.PageStack | _ :: t -> None :: t) }, []
        | PushPage page -> 
            { model with PageStack = Some page :: model.PageStack}, []
        | ReplacePage page -> 
            { model with PageStack = (match model.PageStack with [] -> Some page :: model.PageStack | _ :: t -> Some page :: t) }, []

    let view msg dispatch =
        View.Label()
       //View.NavigationPage(
        //    popped = (fun _ -> dispatch PagePopped), 
        //    poppedToRoot = (fun _ -> dispatch GoHomePage),
        //    pages = [
        //        //if msg.LoggedIn then
        //        //    yield (LoginPage.view dispatch)
        //        //else 
        //            yield (HomePage.view dispatch)
        //    ]
        //)