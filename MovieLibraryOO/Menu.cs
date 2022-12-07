using System;
using Spectre.Console;

namespace MovieLibraryOO
{
    // This is just a fun implementation of Spectre.Console to present more interesting menus
    // Feel free to use it and read more at https://spectreconsole.net if you would like
    // If not, just use your own regular Console.Writeline menus as we have in the past
    public class Menu
    {
        public enum MenuOptions
        {
            ListFromDb,
            ListFromFile,
            Add,
            Update,
            Delete,
            Search,
            ToUsers,
            ToRatings,
            Exit
        }

        public enum UserMenuOptions
        {
            ListFromDb,
            ListFromDbWithFullOccupationName,
            Add,
            Delete,
            ListOccupationsFromDb,
            AddOccupation,
            DeleteOccupation,
            BackToMovies
        }

        public enum RatingMenuOptions
        {
            ListFromDb,
            AddRating,
            DeleteRating,
            BackToMovies
        }

        public Menu() // default constructor
        {
        }

        public MenuOptions ChooseAction()
        {
            var menuOptions = Enum.GetNames(typeof(MenuOptions));

            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Choose your [green]menu action[/]?")
                    .AddChoices(menuOptions));

            return (MenuOptions) Enum.Parse(typeof(MenuOptions), choice);
        }

        public UserMenuOptions ChooseUserAction()
        {
            var menuOptions = Enum.GetNames(typeof(UserMenuOptions));

            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Choose your [green]user menu action[/]?")
                    .AddChoices(menuOptions));

            return (UserMenuOptions)Enum.Parse(typeof(UserMenuOptions), choice);
        }

        public RatingMenuOptions ChooseRatingAction()
        {
            var menuOptions = Enum.GetNames(typeof(RatingMenuOptions));

            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Choose your [green]rating menu action[/]?")
                    .AddChoices(menuOptions));

            return (RatingMenuOptions)Enum.Parse(typeof(RatingMenuOptions), choice);
        }

        public void Exit()
        {
            AnsiConsole.Write(
                new FigletText("Thanks!")
                    .LeftAligned()
                    .Color(Color.Green));
        }

        public string GetUserResponse(string question, string highlightedText, string highlightedColor)
        {
            return AnsiConsole.Ask<string>($"{question} [{highlightedColor}]{highlightedText}[/]");
        }

        //A simplified user response that doesn't require a color or 2nd text parameter
        public string GetUserResponseSimple(string question)
        {
            return AnsiConsole.Ask<string>($"{question}");
        }

        #region examples
        // example - not currently used - see https://spectreconsole.net for more fun!
        public void GetUserInput()
        {
            var name = AnsiConsole.Ask<string>("What is your [green]name[/]?");
            var semester = AnsiConsole.Prompt(
                new TextPrompt<string>("For which [green]semester[/] are you registering?")
                    .InvalidChoiceMessage("[red]That's not a valid semester[/]")
                    .DefaultValue("Spring 2022")
                    .AddChoice("Fall 2022")
                    .AddChoice("Spring 2023"));
            var classes = AnsiConsole.Prompt(
                new MultiSelectionPrompt<string>()
                    .Title("For which [green]classes[/] are you registering?")
                    .PageSize(10)
                    .MoreChoicesText("[grey](Move up and down to reveal more classes)[/]")
                    .InstructionsText(
                        "[grey](Press [blue]<space>[/] to toggle a class, " +
                        "[green]<enter>[/] to accept)[/]")
                    .AddChoices("History", "English", "Spanish", "Math", "Computer", "Literature", "Science",
                        "Chemistry", "Economics"));
        }
        #endregion
    }
}