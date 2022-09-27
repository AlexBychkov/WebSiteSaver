# WebSiteSaver
## About the application
1. The application takes the url from the appsettings.json file.
2. Takes all the links from the web pages recursively
3. Takes all the content from these links
4. Get the pages html content and put it into a folder specified in the application.json file

## Appsettings.json
{<br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"Main": {<br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"BaseWebSiteURL": "https://tretton37.com", - <b>the base url to start traversing from</b><br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"MaxDegreeOfParrallelism": 3, - <b>number of maximum streams to parallel work</b><br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"OutputFolderName": "1337" - <b>name of the folder to put downloaded pages into</b><br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}<br />
}<br />

## Running locally
To run locally
1. Clone the repo with 
```
git clone https://github.com/AlexBychkov/WebSiteSaver.git
```
2. Open the project in VS
3. Set the startup project to "WebSiteSaver.ConsoleApp" and run the app
