using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Utilities;

namespace BestRouteHome
{
    public class Trip
    {
        public enum modes { driving, walking, bicycling, transit };

        #region Fields
        public string webAddress; //HTTP-formatted API query
        public modes mode; //Mode of transportation
        public string source;
        public string destination;
        public string key;
        private string configPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\BestRouteHome.Config.xml";
        private int duration; 
        private List<string> route = new List<string>(); //List of complete text-formatted instructions
        private dynamic tripDataJSON; //JSON response from API
        #endregion Fields

        #region Constructors
        public Trip()
        {

        }

        public Trip(string sourceAddress, string destinationAddress) : this(sourceAddress, destinationAddress, modes.driving)
        {

        }

        public Trip(string sourceAddress, string destinationAddress, modes transportMode)
        {
            source = sourceAddress;
            destination = destinationAddress;
            mode = transportMode;
            webAddress = buildWebAddress();
        }
        #endregion Constructors

        #region Methods
        //Build a trip object based on user inputs
        public void getTripInfo()
        {
            Console.WriteLine(new string('-', 50));

            //Query for travel parameters
            source = ConsoleHelpers.ReadString("Enter the starting address: ");
            destination = ConsoleHelpers.ReadString("Enter the destination address: ");
            mode = ConsoleHelpers.ReadEnum<Trip.modes>("How would you like to get there?");
            webAddress = buildWebAddress();
        }

        public void serializeData()
        {
            XmlSerializer writer = new XmlSerializer(typeof(Trip));

            System.IO.FileStream file = System.IO.File.Create(configPath);

            writer.Serialize(file, this);
            file.Close();
        }

        public Trip deserializeData()
        {
            XmlSerializer reader = new XmlSerializer(typeof(Trip));
            System.IO.StreamReader file = new System.IO.StreamReader(configPath);
            Trip deserializedURL = (Trip)reader.Deserialize(file);

            if (String.IsNullOrEmpty(deserializedURL.key))
            {
                Console.WriteLine($"Google Maps API Key missing. Specify a valid key in {configPath}");
                Console.WriteLine("Press <ENTER> to exit");
                Console.ReadLine();
                return null;
            }

            dataHandler.key = deserializedURL.key;

            deserializedURL.webAddress = deserializedURL.buildWebAddress(); //deserializedURL
            file.Close();
            return deserializedURL; 
        }

        public string buildWebAddress()
        {
            string address = $"https://maps.googleapis.com/maps/api/directions/json?mode={mode}&origin={source}&destination={destination}&traffic_model=best_guess&departure_time=now&key={key}";
            webAddress = address;
            return address;
        }

        //Print Trip details
        public void printTrip()
        {
            double travelHours = duration / 3600.0;
            int travelMinutes = duration / 60;

            Console.WriteLine(new string('-', 50));
            Console.WriteLine($"\nOrigin:         {source}");
            Console.WriteLine($"Destination:    {destination}");
            Console.WriteLine();
            Console.WriteLine($"Travel Time:    {travelMinutes} minutes ({travelHours:F2} hours).\n");
            Console.WriteLine(new string('-', 50));   
        }

        //Print full step-by-step instructions
        public void printLongDirections()
        {
            Console.WriteLine(new string('-', 50));
            Console.WriteLine("Full Directions:");
            foreach(string instruction in route)
            {
                Console.WriteLine($"    -{instruction}");
            }
            Console.WriteLine();
            Console.WriteLine(new string('-', 50));
        }

        public void setDuration(int input)
        {
            duration = input;
        }

        public int getDuration()
        {
            return duration;
        }

        public void appendRoute(string input)
        {
            route.Add(input);
        }

        public List<string> getRoute()
        {
            return route;
        }

        public void clearRoute()
        {
            route.Clear();
        }

        public void setTripDataJSON(dynamic tripData)
        {
            tripDataJSON = tripData;
        }

        public dynamic getTripDataJSON()
        {
            return tripDataJSON;
        }
        #endregion Methods
    }
}
