# BestRouteHome
BestRouteHome is a console application for interacting with the Google Maps API to query route and time information: 

![image](https://user-images.githubusercontent.com/20804273/193421531-f5733113-e441-4924-99bd-a0b3a16becb0.png)

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

![image](https://user-images.githubusercontent.com/20804273/193422248-195eae47-17c6-4136-9dba-1b45da9d34b5.png)

**updateConfig**  
Persist the current route information to BestRouteHome.Config.xml. This route information will be the default when the application starts. 

**Quit**  
Exit and close. 
