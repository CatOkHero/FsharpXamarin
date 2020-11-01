namespace FsharpXamarin.MainPage

open Fabulous
open Fabulous.XamarinForms
open FsharpXamarin.Pages
open Xamarin.Forms

module Navigation = 
    type Msg =
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
        { LoggedIn = false; PageStack = [Some HomePage.Tag] }

    let update msg model =
        match msg with        
        | GoToLoginPage ->
            { model with PageStack = [ Some LoginPage.Tag ] }, []
        | GoHomePage -> 
            { model with PageStack = [ Some HomePage.Tag ] }, []
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

       View.NavigationPage(
            popped = (fun _ -> dispatch PagePopped), 
            poppedToRoot = (fun _ -> dispatch GoHomePage),
            pages = [
                if not(msg.LoggedIn) then
                    yield (LoginPage.view dispatch)
                else 
                    yield (HomePage.view dispatch)
            ]
        )