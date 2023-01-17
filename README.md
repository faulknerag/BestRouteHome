# BestRouteHome
BestRouteHome is a console application for interacting with the Google Maps API to query route and time information: 

![image](Images/BestRouteHome.png?raw=true)

# Installation Instructions
1. Download repository
2. Copy BestRouteHome.Config.xml to 'My Documents'
3. Get an API Key for use in the Google Maps [Directions API](https://console.cloud.google.com/apis/library/directions-backend.googleapis.com)
4. Add your API Key to BestRouteHome.Config.xml as the value of the \<key\> section. E.g.:
  
    > \<key\>MyGoogleMapsDirectionsAPIKey\</key\>
5. Build and run the application

# Menu Options
**Refresh**  
Resend the current query to get the most up-to-date information from the API.

**ChangeRoute**  
Enter new Origin, Destination, and/or Travel Method:

![image](Images/NavigationOptions.png?raw=true)

**updateConfig**  
Persist the current route information to BestRouteHome.Config.xml. This route information will be the default when the application starts. 

**Quit**  
Exit and close. 
