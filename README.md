# MovieX

## Description 
Application for searching and recommending movies, Series and games using a number of APIs.  
Movies can be searched by title, genre or year using the popular movies page, this allows users to find the right movie for them.  
Once a movie is found a user will have the option to get more details about the specific movie and also recommendations based on the title they are currently viewing. 
using a personal profile to create journals of content you have watched and give a personal rating that friends can view. 

## Installing
Clone the repository titled main from https://github.com/yuoopwe/MovieSearchApp/tree/main 
Click the green button titled "Code" copy the link that is presented, open visual studio and select clone repository, paste the link into visual studio and select clone, this will create the project with the application. 

## Run Application 
Select the MovieSearchApp.Android and select the play icon with local machine selected, this will open the application in a window and will be ready for use.
This will run the application and display the main page and flyout page with other options.  

### Search Function
Type the desired title in the search bar and press the search button, this will then display all relevant titles that share the title you have searched. 
Once the title that you have search for is displayed you can select the movie then click the button titled Get Movie Details, this will display all details associated with the title. The movie details are displayed using the Moviedb APi to get the information to display to the user.  

### Recommendations 
The user can also get recommendations based on the title they have searched for use the tastedive API, once the button is selected the application will display reccomended movies.

### Search Type 
There is a dropdown located next to the search button which allows users to select specifically what they are searching for to avoid displaying multiple titles that are not wanted, an example of how this could be useful is if a movie has a game and a tv show that shares the same title. 

### Popular Page 
This page gives the user the ability to search for popular movies using a number of options to choose the correct title, such as the genre, the age rating, year of release and language of the movie, As displayed in the image above once the user selects genre the application will display, once the desired genres are selected press apply and then the back button to go back to the popular movies page to perform the search. 
The language filter can be toggled on and the available languages will be displayed and ready for selection, once the desired language is selected the movies will be displayed once the search has been executed. This also applies to the age filter which will display the available age ranges will be displayed and ready for selection. 

