namespace FsharpXamarin.Pages

open System
open Fabulous
open Fabulous.XamarinForms
open Xamarin.Forms

module LoginPage =
    let [<Literal>] Tag = "Login"

    type Msg =
        | LoginFailed
        | LoginSuccessFull
        | LoginValidating
        | ShowErrorMessage of string
        | DoNothing

    type ExternalMsg =
        | NoOp
        | GoToHomePage

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
          LoginFailedMessage = String.Empty }, Cmd.ofMsg(DoNothing)

    let update msg (model : Model option) =
        match msg with
        | LoginFailed ->
            model, Cmd.none, NoOp
        | LoginSuccessFull ->
            model, Cmd.none, GoToHomePage
        | LoginValidating ->
            model, Cmd.none, NoOp
        | ShowErrorMessage e ->
            model, Cmd.none, NoOp
        | DoNothing -> 
            model, Cmd.none, NoOp

    let view dispatch =
        let userName =
            View.Entry(
                keyboard = Keyboard.Plain,
                placeholder = "Username or email")

        let usernameErrors = 
            View.Label()

        let password =
            View.Entry(
                isPassword = true,
                placeholder = "Password")

        let passwordErrors =
            View.Label()

        let loginButton =
            View.Button(
                text = "Login",
                command = (fun () -> dispatch LoginSuccessFull))

        View.ContentPage(
            content = 
                View.StackLayout(
                    verticalOptions = LayoutOptions.CenterAndExpand,
                    children = [
                        //lottie
                        userName
                        usernameErrors
                        password
                        passwordErrors
                        loginButton
                    ])
        )
