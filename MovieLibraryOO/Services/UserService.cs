using ConsoleTables;
using Microsoft.Extensions.Logging;
using MovieLibraryEntities.Context;
using MovieLibraryEntities.Dao;
using MovieLibraryEntities.Models;
using MovieLibraryOO.Dto;
using MovieLibraryOO.Mappers.OccupationMap;
using MovieLibraryOO.Mappers.UserMap;
using System;
using System.Linq;
using System.Reflection.Emit;
using System.Text.RegularExpressions;

namespace MovieLibraryOO.Services
{
    public class UserService : IUserService
    {
        private readonly ILogger<MainService> _logger;
        private readonly IUserMapper _userMapper;
        private readonly IOccupationMapper _occupationMapper;
        private readonly IRepository _repository;

        public UserService(ILogger<MainService> logger, IUserMapper userMapper, IOccupationMapper occupationMapper, IRepository repository)
        {
            _logger = logger;
            _userMapper = userMapper;
            _occupationMapper = occupationMapper;
            _repository = repository;
        }

        public void Invoke()
        {
            var menu = new Menu();

            Menu.UserMenuOptions menuChoice;
            do
            {
                menuChoice = menu.ChooseUserAction();
                switch (menuChoice)
                {
                    //The User half of the switch
                    //Allows the user to get all users
                    case Menu.UserMenuOptions.ListFromDb:
                        _logger.LogInformation("Listing users from database");
                        var allUsers = _repository.GetAllUser();
                        var users = _userMapper.Map(allUsers);
                        ConsoleTable.From<UserDto>(users).Write();
                        break;

                    //Allows the user to get all users with their occupation names included
                    case Menu.UserMenuOptions.ListFromDbWithFullOccupationName:
                        _logger.LogInformation("Listing users from database with Occupation Names");
                        _logger.LogInformation("This may take awhile!");
                        var allUsersFull = _repository.GetAllUser();
                        var usersFull = _userMapper.Map(allUsersFull);
                        var table = new ConsoleTable("Id", "Age", "Gender", "ZipCode", "Occupation");

                        using(var db = new MovieContext()) { 
                            foreach(var user in usersFull)
                            {
                                var occupationName = db.Occupations.FirstOrDefault(x => x.Id == user.OccupationId).Name;
                                table.AddRow(user.Id, user.Age, user.Gender, user.ZipCode, occupationName);
                            }
                        }

                        Console.WriteLine(table);

                        break;

                    //Allows the addition of a user
                    case Menu.UserMenuOptions.Add:
                        _logger.LogInformation("Adding a new user");

                        
                        //Gets the Users Age
                        var userAge = menu.GetUserResponse("Enter the User's", "Age:", "green");

                        //Try to parse the integer
                        try
                        {
                            var userAgeInt = int.Parse(userAge);

                            //Makes sure it isn't negative either
                            if(userAgeInt < 0)
                            {
                                Console.WriteLine("Age cannot be negative");
                            }
                            else
                            {
                                //Gets the Users Gender
                                var userGender = menu.GetUserResponse("Enter the User's", "Gender:", "green");

                                var _usZipRegEx = @"^\d{5}(?:[-\s]\d{4})?$";
                                var _caZipRegEx = @"^([ABCEGHJKLMNPRSTVXY]\d[ABCEGHJKLMNPRSTVWXYZ])\ {0,1}(\d[ABCEGHJKLMNPRSTVWXYZ]\d)$";

                                //Gets the Users Zip Code
                                var userZipCode = menu.GetUserResponse("Enter the User's", "ZipCode:", "green");

                                if ((!Regex.Match(userZipCode, _usZipRegEx).Success) && (!Regex.Match(userZipCode, _caZipRegEx).Success))
                                {
                                    Console.WriteLine("Not a valid Zip Code!");
                                }
                                else
                                {
                                    //Gets the Users Occupation
                                    var userOccupation = menu.GetUserResponse("Enter the User's", "Occupation:", "green");

                                    using (var db = new MovieContext())
                                    {
                                        var matchingOccupation = db.Occupations.FirstOrDefault(x => x.Name == userOccupation);

                                        if(matchingOccupation == null)
                                        {
                                            Console.WriteLine("No Occupation entered, or the Occupation entered isn't available!");
                                        } else { 

                                            var user = new User()
                                            {
                                                Age = userAgeInt,
                                                Gender = userGender,
                                                ZipCode = userZipCode,
                                                OccupationId = matchingOccupation.Id
                                            };

                                            db.Users.Add(user);
                                            db.SaveChanges();

                                            //Confirms that the addition of the user in the database
                                            Console.WriteLine($"({user.Id}), age: {user.Age}, gender: {user.Gender}, zipcode: {user.ZipCode}, occupation id: {user.OccupationId}");
                                        }
                                    }
                                }
                            }
                        }
                        catch(FormatException ex)
                        {
                            _logger.LogInformation(ex.Message);
                            Console.WriteLine("Age must be an integer");
                        }

                        break;
                    case Menu.UserMenuOptions.Delete:
                        _logger.LogInformation("Deleting a user");

                        //Gets the Id of the User getting DELETED
                        var deletingUserId = menu.GetUserResponse("Enter The Id of the User to", "DELETE:", "red");

                        try
                        {
                            var deletingUserIdNum = int.Parse(deletingUserId);

                            using (var db = new MovieContext())
                            {
                                var deletingUser = db.Users.FirstOrDefault(x => x.Id == deletingUserIdNum);
                                if(deletingUser != null)
                                {
                                    //Displays the details of the user thats about to be DELETED from the database
                                    Console.WriteLine($"({deletingUser.Id}), age: {deletingUser.Age}, gender: {deletingUser.Gender}, zipcode: {deletingUser.ZipCode}, occupation id: {deletingUser.OccupationId}");

                                    //Deletes the user from the database
                                    db.Users.Remove(deletingUser);
                                    db.SaveChanges();

                                    //Confirms that the program at least attempted to the delete the given user and didn't throw an error
                                    Console.WriteLine("This User has been deleted");
                                }
                                else
                                {
                                    Console.WriteLine($"A user with id {deletingUserIdNum} doesn't exist in the database");
                                    Console.WriteLine("Check your if and try again");
                                }
                            }
                        }
                        catch(FormatException ex)
                        {
                            _logger.LogInformation(ex.Message);
                            Console.WriteLine("Id must be an integer");
                        }

                        break;

                    //The Occupations half of the switch
                    case Menu.UserMenuOptions.ListOccupationsFromDb:
                        _logger.LogInformation("Listing occupations from database");
                        var allOccupations = _repository.GetAllOccupation();
                        var occupations = _occupationMapper.Map(allOccupations);
                        ConsoleTable.From<OccupationDto>(occupations).Write();
                        break;
                    case Menu.UserMenuOptions.AddOccupation:
                        _logger.LogInformation("Adding a new occupation");

                        //Gets the Occupations Name
                        var newOccupationName = menu.GetUserResponse("Enter the Occupation's", "Name:", "green");

                        using(var db = new MovieContext())
                        {
                            var overlapOccName = db.Occupations.FirstOrDefault(x => x.Name == newOccupationName);
                            if(overlapOccName != null)
                            {
                                Console.WriteLine($"An occupation with the name {newOccupationName} already exists in the database");
                                Console.WriteLine("Try adding an occupation with a different name");
                            }
                            else
                            {
                                var occupation = new Occupation()
                                {
                                    Name = newOccupationName
                                };

                                db.Occupations.Add(occupation);
                                db.SaveChanges();
                            }
                        }

                        break;
                    case Menu.UserMenuOptions.DeleteOccupation:
                        _logger.LogInformation("Deleting an occupation");

                        //Gets the Id of the Occupation getting DELETED
                        var deletingOccId = menu.GetUserResponse("Enter The Id of the Occupation to", "DELETE:", "red");

                        try
                        {
                            var deletingOccIdNum = int.Parse(deletingOccId);

                            using (var db = new MovieContext())
                            {
                                var userWithOcc = db.Users.FirstOrDefault(x => x.OccupationId == deletingOccIdNum);
                                var deletingOcc = db.Occupations.FirstOrDefault(x => x.Id == deletingOccIdNum);

                                //Ensures the occupation exists, and isn't already paired with someone else
                                if (userWithOcc == null && deletingOcc != null)
                                {
                                    //Displays the details of the occupation thats about to be DELETED from the database
                                    Console.WriteLine($"({deletingOcc.Id}), Name: {deletingOcc.Name}");

                                    //Deletes the occupation from the database
                                    db.Occupations.Remove(deletingOcc);
                                    db.SaveChanges();

                                    //Confirms that the program at least attempted to the delete the given occupation and didn't throw an error
                                    Console.WriteLine("This Occupation has been deleted");
                                }
                                //Error occurs if someone still has the occupation trying to be deleted
                                else if(userWithOcc != null)
                                {
                                    Console.WriteLine($"A user still has the occupation {deletingOcc.Name}");
                                    Console.WriteLine("Remove all instances of the the occupation within the user database first");
                                }
                                //Error occurs if the occupation trying to be deleted doesn't exist in the first place
                                else if(deletingOcc == null)
                                {
                                    Console.WriteLine($"A occupation with id {deletingOccIdNum} doesn't exist in the database");
                                    Console.WriteLine("Check your if and try again");
                                }
                            }
                        }
                        catch (FormatException ex)
                        {
                            _logger.LogInformation(ex.Message);
                            Console.WriteLine("Id must be an integer");
                        }

                        break;
                }
            }
            while (menuChoice != Menu.UserMenuOptions.BackToMovies);
        }
    }
}
