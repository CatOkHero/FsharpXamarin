namespace FsharpXamarin.Pages

open System
open Fabulous
open Fabulous.XamarinForms
open Xamarin.Forms

module HomePage =
    let [<Literal>] Tag = "Home"

    type Msg =
        | LoginFailed
        | LoginSuccessFull
        | LoginValidating
        | ShowErrorMessage of string
        | DoNothing

    type Model =
        { Username: string
          Password: string
          IsLoading: bool
          LoginFailed: bool
          LoginFailedMessage: string }

    let init =
        { Username = String.Empty; 
          Password = String.Empty; 
          IsLoading = false; 
          LoginFailed = false; 
          LoginFailedMessage = String.Empty }

    let update msg (model : Model) =
        match msg with
        | LoginFailed ->
            { model with LoginFailed = true }, Cmd.none, None
        | LoginSuccessFull ->
            model, Cmd.none, None
        | LoginValidating ->
            match model.Username, model.Password with
            | "", "" ->
                model, Cmd.none, None
            | _, _ ->
                model, Cmd.none, None
        | ShowErrorMessage e ->
            model, Cmd.none, None
        | DoNothing -> 
            model, Cmd.none, None

    let view dispatch =
        let userName =
            View.Entry(
                keyboard = Keyboard.Plain,
                placeholder = "Username or email")

        let usernameErrors = 
            View.Label(
                text = "Hello World"
            )

        let password =
            View.Entry(
                isPassword = true,
                placeholder = "Password")

        let passwordErrors =
            View.Label()

        let loginButton =
            View.Button(
                text = "Login")

        View.ContentPage(
            content = 
                View.StackLayout(
                    verticalOptions = LayoutOptions.CenterAndExpand,
                    children = [
                        //lottie
                        //userName
                        usernameErrors
                        //password
                        //passwordErrors
                        //loginButton
                    ])
        )

