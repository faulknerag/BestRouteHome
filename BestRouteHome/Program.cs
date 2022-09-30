using System;
using Utilities;

namespace BestRouteHome
{
    class Program
    {
        public enum @switch { start , options, updateConfig, ChangeRoute, Refresh, dataProcess, Quit }; //One item for each switch selection + Quit
        public enum userSelection { Refresh, ChangeRoute, updateConfig, Quit } //These are the options that the user can select

        static void Main(string[] args)
        {
            Trip trip = new Trip(); //All state data is stored in this Trip object

            @switch action = @switch.start; //Will be used to control logical flow throw the main switch structure

            #region Main Menu
            do
            {
                switch (action.ToString().ToLower())
                {
                    //Attempts to de-serialize from file. 
                    case "start":
                        try
                        {
                            trip = trip.deserializeData(); //Read saved data from config file

                            if (trip == null) return;
                            
                            action = @switch.dataProcess; //Process the data
                        }
                        catch (Exception ex) //If that fails, we need to create a new config file
                        {
                            Console.WriteLine($"There was an error de-serializing: {ex.Message}");

                            action = @switch.ChangeRoute; //Query user for trip inputs
                        }
                        break;

                    //Query API for data, process, and print
                    case "refresh":
                    case "dataprocess":
                        dataHandler.getJSONFromWeb(trip);
                        dataHandler.parseDataFromJSON(trip);
                        trip.printTrip();
                        trip.printLongDirections();

                        action = @switch.options; //Present user with menu options
                        break;

                    //Present user with menu options
                    case "options": //Redirects to the options pane
                        Console.WriteLine();
                        action = (@switch)Enum.Parse(typeof(@switch), ConsoleHelpers.ReadEnum<userSelection>("Select an Option: ").ToString()); //Display options from 'userSelection' enum, but save into an 'options' enum
                        Console.WriteLine();
                        break;

                    //Serialize the current Trip object to file
                    case "updateconfig":
                        try
                        {
                            trip.serializeData(); //Write data to file

                            Console.WriteLine();
                            Console.WriteLine("Config file successfully updated!");
                            Console.WriteLine();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine();
                            Console.WriteLine($"There was an error serializing: {ex.Message}");
                        }

                        action = @switch.options; //Present user with menu options
                        break;

                    //Query user for trip inputs
                    case "changeroute":
                    default:
                        trip = new Trip(); //Erase all user-defined properties
                        trip.getTripInfo(); //Query user for trip details

                        action = @switch.dataProcess; //Process the data
                        break;
                }
            }
            while (action.ToString().ToLower() != @switch.Quit.ToString().ToLower());
        }
        #endregion Main Menu
    }
}
