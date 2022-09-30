using System;
using System.Net;
using System.Text;
using System.Web;
using System.IO;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace BestRouteHome
{
    public static class dataHandler
    {
        public static string key;

        #region Methods
        //Open a WebClient and request JSON data using the trip
        public static void getJSONFromWeb(Trip trip)
        {
            try
            {
                if (String.IsNullOrEmpty(trip.key))
                {
                    trip.key = key;
                    trip.buildWebAddress();
                }

                //Read all data from page
                WebClient client = new WebClient();
                Stream data = client.OpenRead(trip.webAddress);
                StreamReader reader = new StreamReader(data);
                string rawText = reader.ReadToEnd();

                //Close reader objects
                data.Close();
                reader.Close();

                dynamic tripDataJSON = JsonConvert.DeserializeObject(rawText); //Convert rawText string to JSON object

                trip.setTripDataJSON(tripDataJSON); //Save JSON data to trip object
            }
            catch (Exception ex)
            {
                Console.WriteLine($"There was an error accessing the API: {ex.Message}");
            } 
        }

        //Parse travel duration and instructions from JSON
        public static void parseDataFromJSON(Trip trip)
        {   
            if ( trip.getTripDataJSON() != null && (string)trip.getTripDataJSON().status == "OK")
            {
                int travelTime;
                if (trip.mode == Trip.modes.driving) //duration_in_traffic is only specified in 'driving' mode
                {
                    travelTime = trip.getTripDataJSON().routes[0].legs[0].duration_in_traffic.value; //Get travelTime from JSON
                }
                else
                {
                    travelTime = trip.getTripDataJSON().routes[0].legs[0].duration.value;
                }
                
                trip.setDuration(travelTime); //Store travelTime in the trip object

                if (trip.getRoute().Count > 0) trip.clearRoute(); //If the trip is refreshed, the route must be cleared so that it doesn't duplicate

                foreach (var step in trip.getTripDataJSON().routes[0].legs[0].steps)
                {
                    if (step.duration.value >= 0) //Only print steps that take more than 0 seconds to complete
                    {
                        string htmlText = WebUtility.HtmlDecode(step.html_instructions.ToString()); //Raw HTML-formatted string

                        //Replace any changes in style with spaces
                        htmlText = htmlText.Replace("<div style=", ". <div style=");
                        Regex reg = new Regex("<[^>]+>"); //Search for '<' followed by 1 or more (characters that are not a '>'), followed by a '>'
                        var stripped = reg.Replace(htmlText, ""); //Replace any found instances with null
                        trip.appendRoute(stripped); //Append the step instruction to the trip object
                    }
                }
            }
            else
            {
                Console.WriteLine($"Invalid Parameters: {trip.getTripDataJSON().status}");
            }
        }
        #endregion Methods
    }
}
