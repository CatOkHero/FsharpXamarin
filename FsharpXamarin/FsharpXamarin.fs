// Copyright 2018-2019 Fabulous contributors. See LICENSE.md for license.
namespace FsharpXamarin

open System
open Fabulous
open Fabulous.XamarinForms
open Fabulous.XamarinForms.LiveUpdate
open FsharpXamarin.MainPage
open FsharpXamarin.Pages
open Xamarin.Forms

module App = 
    type Model = 
        { LoginPageModel: LoginPage.Model option
          HomePageModel: HomePage.Model option
          IsLoggedIn: bool }

    type Msg = 
        | LoginPageMsg of LoginPage.Msg
        | HomePageMsg of HomePage.Msg
        | GoToHomePage
        | GoToLoginPage

    let init () = 
      let loginModel, loginMsg = LoginPage.init
      { LoginPageModel = Some(loginModel)
        HomePageModel = None
        IsLoggedIn = false }, (Cmd.map LoginPageMsg loginMsg)

    let hangleLoginPageExternalMsg externalMsg = 
        match externalMsg with
        | LoginPage.ExternalMsg.NoOp -> Cmd.none
        | LoginPage.ExternalMsg.GoToHomePage -> Cmd.ofMsg (GoToHomePage)

    let handleHomePageExternalMsg externalMsg = 
        match externalMsg with
        | HomePage.ExternalMsg.NoOp -> Cmd.none
        | HomePage.ExternalMsg.GoToLoginPage -> Cmd.ofMsg (GoToLoginPage)

    let update (msg : Msg) model =
        match msg with
        | LoginPageMsg msg ->
            let m, cmd, externalMsg = LoginPage.update msg model.LoginPageModel
            let cmd2 = hangleLoginPageExternalMsg externalMsg
            let batchCmd = Cmd.batch [ (Cmd.map LoginPageMsg cmd); cmd2 ]
            { model with LoginPageModel = m; IsLoggedIn = true }, batchCmd
        | HomePageMsg msg ->
            let m, cmd, externalMsg = HomePage.update msg model.HomePageModel
            let cmd2 = handleHomePageExternalMsg externalMsg
            let batchCmd = Cmd.batch [ (Cmd.map HomePageMsg cmd); cmd2 ]
            { model with HomePageModel = m }, batchCmd
        | GoToHomePage ->
            let modelHomePage = HomePage.init
            { model with HomePageModel = Some (modelHomePage)}, Cmd.none
        | GoToLoginPage ->
            let modelHomePage, loginMsg = LoginPage.init
            { model with LoginPageModel = Some (modelHomePage); HomePageModel = None }, (Cmd.map LoginPageMsg loginMsg)


    let view (model: Model) dispatch =
        let allPages = 
            if model.HomePageModel.IsSome then
                //let loginPage = LoginPage.view (LoginPageMsg >> dispatch)
                let homePage = HomePage.view (HomePageMsg >> dispatch)
                //[ loginPage; homePage ]
                [ homePage ]
            else 
                let loginPage = LoginPage.view (LoginPageMsg >> dispatch)
                [ loginPage ]

        View.NavigationPage(
            //popped = (fun _ -> dispatch NavigationPopped),
            pages = allPages
        )

    // Note, this declaration is needed if you enable LiveUpdate
    let program = XamarinFormsProgram.mkProgram init update view

type App () as app = 
    inherit Application ()

    let runner = 
        App.program
#if DEBUG
        |> Program.withConsoleTrace
#endif
        |> XamarinFormsProgram.run app

#if DEBUG
    // Uncomment this line to enable live update in debug mode. 
    // See https://fsprojects.github.io/Fabulous/Fabulous.XamarinForms/tools.html#live-update for further  instructions.
    //
    do runner.EnableLiveUpdate()
#endif    

    // Uncomment this code to save the application state to app.Properties using Newtonsoft.Json
    // See https://fsprojects.github.io/Fabulous/Fabulous.XamarinForms/models.html#saving-application-state for further  instructions.
#if APPSAVE
    let modelId = "model"
    override __.OnSleep() = 

        let json = Newtonsoft.Json.JsonConvert.SerializeObject(runner.CurrentModel)
        Console.WriteLine("OnSleep: saving model into app.Properties, json = {0}", json)

        app.Properties.[modelId] <- json

    override __.OnResume() = 
        Console.WriteLine "OnResume: checking for model in app.Properties"
        try 
            match app.Properties.TryGetValue modelId with
            | true, (:? string as json) -> 

                Console.WriteLine("OnResume: restoring model from app.Properties, json = {0}", json)
                let model = Newtonsoft.Json.JsonConvert.DeserializeObject<App.Model>(json)

                Console.WriteLine("OnResume: restoring model from app.Properties, model = {0}", (sprintf "%0A" model))
                runner.SetCurrentModel (model, Cmd.none)

            | _ -> ()
        with ex -> 
            App.program.onError("Error while restoring model found in app.Properties", ex)

    override this.OnStart() = 
        Console.WriteLine "OnStart: using same logic as OnResume()"
        this.OnResume()
#endif


