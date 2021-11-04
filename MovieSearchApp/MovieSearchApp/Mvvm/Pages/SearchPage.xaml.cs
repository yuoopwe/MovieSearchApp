using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Newtonsoft.Json;
using System.Diagnostics;


namespace MovieSearchApp.Mvvm.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchPage : ContentPage
    {

        private string input;
        private string encodedTerm; //variable to deal with spaces in url " "->"%20"
        private int page = 1;
        private string jsonTitleString;
        private string jsonDetailsString;
        private int _totalResults;
        private bool noMorePages;
        private int currentIndex;

        private List<FilmSearchResults> filmList = new List<FilmSearchResults>();

        private InitialSearchJSON.Rootobject initialRootObject;
        private InitialSearchJSON.Search initialSearch;
        private DetailedResultsJSON.Rootobject detailedRootObject;
        private DetailedResultsJSON.Rating detailedRatings;


        public SearchPage()
        {
            InitializeComponent();

        }
        private void ButtonClick_Clicked(object sender, EventArgs e)
        {

        }
        private async void SearchBar_SearchButtonPressed(object sender, EventArgs e)
        {
            if (searchBar1.Text != "")
            {
                filmList.Clear();
                listView1.ItemsSource = filmList;
                input = searchBar1.Text;
                encodedTerm = Uri.EscapeUriString(input); //Uri.EscapeUriString() will take care of " " to "%"20" for URL

                for (int i = 1; i < 101; i++)
                {
                    searchBar1.IsEnabled = false;
                    if (noMorePages)
                    {
                        searchBar1.Text = $"Search completed! {_totalResults} results were found!";
                        noMorePages = false;
                        searchBar1.IsEnabled = true;
                        break;
                    }
                    page = i;
                    await APICall();
                }
                List<string> titlesToDisplay = new List<string>();

                for (int x = 0; x < filmList.Count; x++)
                {
                    titlesToDisplay.Add($"{filmList[x].Title} ({filmList[x].Year})");
                }
                listView1.ItemsSource = titlesToDisplay;
            }

        }

        private void SearchBar_OnTextChanged(object sender, TextChangedEventArgs textChangedEventArg)
        {
            if (textChangedEventArg.NewTextValue == string.Empty)
            {
                Debug.WriteLine("empty?");
            }
        }

        private async Task APICall()
        {
            var client = new HttpClient();

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"http://www.omdbapi.com/?apikey=42740dd9&s={encodedTerm}&page={page}"),
            };

            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                jsonTitleString = await response.Content.ReadAsStringAsync();
                initialRootObject = JsonConvert.DeserializeObject<InitialSearchJSON.Rootobject>(jsonTitleString);
                initialSearch = JsonConvert.DeserializeObject<InitialSearchJSON.Search>(jsonTitleString);
                //Debug.WriteLine(body);

                if (initialRootObject.totalResults != null || initialRootObject.totalResults != "0")
                {

                    if (initialRootObject.Search != null)
                    {
                        _totalResults = Int32.Parse(initialRootObject.totalResults);

                        foreach (var result in initialRootObject.Search)
                        {
                            searchBar1.Text = result.Title;
                            string imdbID = result.imdbID;
                            await FetchFilmInformation(imdbID);
                            detailedRootObject = JsonConvert.DeserializeObject<DetailedResultsJSON.Rootobject>(jsonDetailsString);
                            detailedRatings = JsonConvert.DeserializeObject<DetailedResultsJSON.Rating>(jsonDetailsString);
                            string title = detailedRootObject.Title;
                            string poster = detailedRootObject.Poster;

                            string year = detailedRootObject.Year;

                            int runtime = 0; //detailedRootObject.Runtime;
                            if (int.TryParse(detailedRootObject.Runtime, out int runTimeVal))
                                runtime = Int32.Parse(detailedRootObject.Runtime);

                            string genre = detailedRootObject.Genre;
                            string director = detailedRootObject.Director;
                            string writer = detailedRootObject.Writer;
                            string actor = detailedRootObject.Actors;
                            string plot = detailedRootObject.Plot;
                            string language = detailedRootObject.Language;
                            string country = detailedRootObject.Country;
                            string awards = detailedRootObject.Awards;
                            string metascore = detailedRootObject.Metascore;
                            string imdbRating = detailedRootObject.imdbRating;
                            string imdbVoteCount = detailedRootObject.imdbVotes;
                            string boxOffice = detailedRootObject.BoxOffice;
                            string production = detailedRootObject.Production;
                            string website = detailedRootObject.Website;

                            List<string> quickList = new List<string> {imdbID,title,poster,year,genre,director,writer,actor,plot,language
                            ,country,awards,metascore,imdbRating, imdbVoteCount, boxOffice, production,website};

                            for (int i = 0; i < quickList.Count; i++)
                            {
                                if (quickList[i] == "" || quickList[i] == "N/A")
                                {
                                    quickList[i] = "Empty/Nothing/Test";
                                }
                            }

                            filmList.Add(new FilmSearchResults()
                            {
                                Title = title,
                                ImdbID = imdbID,
                                Year = year,
                                Poster = poster,
                                Runtime = runtime,
                                Genre = genre,
                                Director = director,
                                Writer = writer,
                                Actor = actor,
                                Plot = plot,
                                Language = language,
                                Country = country,
                                Awards = awards,
                                Metascore = metascore,
                                ImdbRating = imdbRating,
                                ImdbVoteCount = imdbVoteCount,
                                BoxOffice = boxOffice,
                                Production = production,
                                Website = website
                            });
                            Debug.WriteLine($"MOVIE: {title} POSTER LINK: {poster} YEAR RELEASE: {year}");
                        }
                    }
                    else
                    {
                        noMorePages = true;
                    }
                }
            }
        }

        private async Task FetchFilmInformation(string tempID)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("http://www.omdbapi.com/?i=" + tempID + "&apikey=42740dd9"),
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                jsonDetailsString = await response.Content.ReadAsStringAsync();
                // Debug.WriteLine(body);
            }
        }

        private void listView1_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            UpdateFilm();
        }

        private void UpdateFilm()
        {
            currentIndex = (listView1.ItemsSource as List<string>).IndexOf(listView1.SelectedItem.ToString());



            titleLabel.Text = $"{filmList[currentIndex].Title}";
            descriptionLabel.Text = $"{filmList[currentIndex].Plot}";
            ratingLabel.Text = $"{filmList[currentIndex].ImdbRating}/10 ({filmList[currentIndex].ImdbVoteCount} votes)";
            directorLabel.Text = $"{filmList[currentIndex].Director}";
            if (filmList[currentIndex].Poster == "empty")
            {
                posterImage.Source = $"https://theweekendcouchpotato.files.wordpress.com/2015/08/question-mark.gif?w=471&h=659&crop=";
            }
            else
            {
                posterImage.Source = $"{filmList[currentIndex].Poster}";
            }
            
            posterImage.Source = $"{filmList[currentIndex].Poster}";
            Debug.Write(filmList[currentIndex].Poster);

        }
    }
}

