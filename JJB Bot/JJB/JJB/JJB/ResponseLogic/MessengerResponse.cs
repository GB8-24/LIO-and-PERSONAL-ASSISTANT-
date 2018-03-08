using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Net.Http.Headers;
using System.IO;
using System.Runtime.Serialization;
using Newtonsoft.Json.Linq;
using JJB.LuisModels;
using JJB.Essentials;
using JJB.Classes_Json;
using System.Data.SqlClient;
using System.Data;

namespace JJB.ResponseLogic
{
    [Serializable]
    public class MessengerResponse : IDialog<object>
    {
        string cname = "";
        string cnum = "";
        string recipe = "";
        string recipe_id = "";
        string restaurant_loc = "";
        string restaurant_name = "";
        string gender = "Male";
        string symptoms = "";
        string age = "20";

        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(this.MessageReceivedAsync);
        }
        public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = (Activity)await result;

            Item item = null;
            var reply = context.MakeMessage();
            var httpClient = new HttpClient();
            var user = reply.Recipient.Name;
            int luisflag = 0;
            var luisid = "";
            var luisEndpoint = ConfigurationManager.AppSettings["LuisApp"];
            var requestUri = string.Format(luisEndpoint, message.Text);
            var LuisJson = JsonConvert.DeserializeObject<LuisJson>(await httpClient.GetStringAsync(requestUri));
            luisid = LuisJson.Intents.First().Intent.ToLower();
            var entity_type = "";
            var entity_val = "";
            int entity_pos_1;
            int entity_pos_2;
            int entity_pos_3;
            int entity_flag = 0;


            if (luisid != "None" && luisid !="small_talk" && LuisJson.Entities.Count > 0)
            {
                var entity_types = LuisJson.Entities.Select(x => x.Type).ToArray();
                entity_pos_1 = Array.IndexOf(LuisJson.Entities.Select(x => x.Type).ToArray(), "commands");
                entity_pos_2 = Array.IndexOf(LuisJson.Entities.Select(x => x.Type).ToArray(), "device");
                entity_pos_3 = Array.IndexOf(LuisJson.Entities.Select(x => x.Type).ToArray(), "switch");


                if (entity_pos_1 != -1)
                {
                    entity_val = LuisJson.Entities[entity_pos_1].Entity;

                    if(("courierparcelitemcouriers").Contains(entity_val.ToLower()))
                    {
                        reply.Text = "Sure, can you please enter the courier name: ";
                        await context.PostAsync(reply);
                        context.Wait(this.Cname);
                        return;

                    }
                    else if(("reciperecipes").Contains(entity_val.ToLower()))
                    {
                        reply.Text = "Please enter the dish name or ingredients seperated with a ','  ";
                        await context.PostAsync(reply);
                        context.Wait(this.Recipe_id);
                        return;
                    }
                    else if (("medicalsymptomshealth").Contains(entity_val.ToLower()))
                    {
                        reply.Text = "Please enter the symptoms seperated with a ','  ";
                        await context.PostAsync(reply);
                        context.Wait(this.Medical_Gender);
                        return;
                    }
                    else if (("restaurant").Contains(entity_val.ToLower()))
                    {// restaurant wise
                        reply.Text = "Please tell me the restaurant you want to view: ";
                        await context.PostAsync(reply);
                        context.Wait(this.RESTO);
                        return;
                    }
                    else if ("restaurantsareaeateryfoodeat".Contains(entity_val.ToLower()))
                    {// area wise
                        reply.Text = "Sure, please tell me the area and i will give you all the restaurant details in no time =D\n\n";
                        await context.PostAsync(reply);
                        context.Wait(this.Zomato);
                        return;
                    }
                    else
                    {
                        reply = await GetItem(luisid, entity_val, message, context);
                        
                    }
                }
                else if (entity_pos_2 != -1)
                {
                    entity_val = LuisJson.Entities[entity_pos_2].Entity;
                    if(("hotcolddarkbrightbored").Contains(entity_val.ToLower()))
                    {
                        reply = await GetItem(luisid, entity_val, message, context);
                    }
                    else if (entity_pos_3 != -1 && !(("hotcolddarkbrightbored").Contains(entity_val.ToLower())))
                    {
                        entity_val = LuisJson.Entities[entity_pos_3].Entity;
                        reply.Text = "Please tell me what to do with the device. Try, 'Switch on the lights'.";
                    }
                    else
                    {
                        entity_val = LuisJson.Entities[entity_pos_3].Entity;
                        if (entity_val.ToLower().Equals("off"))
                        {
                            
                            var dev = LuisJson.Entities[entity_pos_2].Entity;
                            reply.Text = "We have switched off the "+dev+" for you.";
                            if (dev.ToLower() == "lights" || dev.ToLower() == "light")
                                updatedb("LightsOff");
                            else if (dev.ToLower() == "fan" || dev.ToLower() == "fans")
                                updatedb("FansOff");
                            else if (dev.ToLower() == "tv" || dev.ToLower() == "television")
                                updatedb("TvOff");
                            else if (dev.ToLower() == "ac" || dev.ToLower() == "air condition" || dev.ToLower() == "cooler")
                                updatedb("AcOff");
                            else if (dev.ToLower() == "heater")
                                updatedb("HeaterOff");
                            else if (dev.ToLower() == "washer" || dev.ToLower() == "washing machine")
                                updatedb("WashingMachineOff");
                            else
                            {
                                reply.Text = "Could not identify device, please try again";
                                await context.PostAsync(reply);
                                context.Wait(this.MessageReceivedAsync);
                                return;
                            }


                        }
                        else
                        {
                            entity_val = LuisJson.Entities[entity_pos_2].Entity;
                            reply = await GetItem(luisid, entity_val, message, context);
                            var dev = entity_val;
                            if (dev.ToLower() == "lights" || dev.ToLower() == "light")
                                updatedb("LightsOn");
                            else if (dev.ToLower() == "fan" || dev.ToLower() == "fans")
                                updatedb("FansOn");
                            else if (dev.ToLower() == "tv" || dev.ToLower() == "television")
                                updatedb("TvOn");
                            else if (dev.ToLower() == "ac" || dev.ToLower() == "air condition" || dev.ToLower() == "cooler")
                                updatedb("AcOn");
                            else if (dev.ToLower() == "heater")
                                updatedb("HeaterOn");
                            else if (dev.ToLower() == "washer" || dev.ToLower() == "washing machine")
                                updatedb("WashingMachineOn");
                            else
                            {
                                reply.Text = "Could not identify device, please try again";
                                await context.PostAsync(reply);
                                context.Wait(this.MessageReceivedAsync);
                                return;
                            }
                        }
                    }
                }
            }
            else if(luisid != "None" && luisid == "small_talk" && LuisJson.Entities.Count > 0)
            {
                var entity_types = LuisJson.Entities.Select(x => x.Type).ToArray();
                entity_pos_1 = Array.IndexOf(LuisJson.Entities.Select(x => x.Type).ToArray(), "small_talk");
                reply = await GetItem(luisid, entity_val, message, context);
            }
            else
            {
                reply = await GetSearchResult(context, message.Text);
            }

            await context.PostAsync(reply);
            context.Wait(this.MessageReceivedAsync);
        }


        #region courier
        public async Task Cname(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {   
            var message = (Activity)await argument;
            cname = message.Text;
            cname = cname.ToLower();
            var reply = context.MakeMessage();
            reply.Text = "Can you please enter the courier number?";
            await context.PostAsync(reply);
            context.Wait(this.Cnum);
        }

        public async Task Cnum(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = (Activity)await argument;
            cnum = message.Text;
            var reply = context.MakeMessage();
            HttpClient client = new HttpClient();

            Tracking1 cp = new Tracking1() { slug1 = cname, tracking_number1 = cnum };
            Rootobject2 cp2 = new Rootobject2() { tracking1 = cp };
            //cp.tracking1.slug1 = cname;
            //cp.tracking1.tracking_number1 = cnum;
            client.BaseAddress = new Uri("https://api.aftership.com/v4/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("aftership-api-key", "d330442e-e805-41df-a7c8-5f3b9b1b73dd"); //new AuthenticationHeaderValue("aftership-api-key","5a06821f-0889-434e-bc0f-b866e0645976");
                                                                                                           // var response = await client.PAsync("/api/tokens/");
            var response = await client.PostAsJsonAsync("trackings", cp2);


            //var conversation = new Converse();

            var response1 = await client.GetAsync("/v4/trackings/" + cname + "/" + cnum);

            if (response1.IsSuccessStatusCode)
            { //
                CourierTrack track = response1.Content.ReadAsAsync(typeof(CourierTrack)).Result as CourierTrack;
                var l = track.data.tracking.checkpoints.Length - 1;
                var m = track.data.tracking.checkpoints[l];
                reply.Attachments = new List<Microsoft.Bot.Connector.Attachment>();

                reply.Text = "GOT it, here is your courier result: \n\n Your courier is currently at " + m.location + "\n Status: " + m.message + "\n\n" ;

                var heroCard = new HeroCard
                {
                    Title = "Location: "+m.location + "  Staus: " + m.message,
                    Text = "For complete details click the button below: ",
                    Images = new List<CardImage>
                                    {
                                        new CardImage
                                        {
                                                Url = "https://img.clipartfest.com/ae30545a43343975ee4592531bd3fa68_i-got-it-clipart-1-got-it-clip-art_578-663.png"
                                        }
                                    }
                };
                var buttons = new List<CardAction>();
                var button = new CardAction
                {
                    Type = "openUrl",
                    Title = "View Complete Info",
                    Value = "https://track.aftership.com/" + cname + "/" + cnum 
                };
                buttons.Add(button);
                heroCard.Buttons = buttons;
                reply.Attachments.Add(heroCard.ToAttachment());
                reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;

            }
            else
            {
                reply.Text = "Here is what we found: https://track.aftership.com/" + cname + "/" + cnum;
            }
            await context.PostAsync(reply);
            context.Wait(this.MessageReceivedAsync);


        }
        #endregion

        #region Recipe
        public async Task Recipe(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = (Activity)await argument;
            recipe = message.Text;
            //call api
            RecipeResult rec1 = new RecipeResult();
            var reply = context.MakeMessage();
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //  client.DefaultRequestHeaders.Add("user-key", "2e390670a23bce1d759e970c48815629");
            string uri = "http://food2fork.com/api/search?key=1d2514ae39ac1c3a5326688fc4e1ca7d&q=" + recipe;
            HttpResponseMessage msgrecipe = await client.GetAsync(uri);
            if (msgrecipe.IsSuccessStatusCode)
            {
                var jsonResponse3 = await msgrecipe.Content.ReadAsStringAsync();
                rec1 = JsonConvert.DeserializeObject<RecipeResult>(jsonResponse3);
            }

            reply.Text = "This is what we found XD : \n\n\n\n";
            var i = 0;
            while (i < rec1.count && i < 10)
            {
                reply.Text += "*" + rec1.recipes[i].title + "*\n\n" + "Recipe Id is *" + rec1.recipes[i].recipe_id + "*\n\n" + "[Click here for zomato page](" + rec1.recipes[i].source_url + ")\n\n" + "![](" + rec1.recipes[i].image_url + ")\n\n";
                i++;
            }
            reply.Text += "\n\n Please enter the recipe ID you want to know about from the above list";
            reply.Text = "Please enter the recipe id you want to know about from the list above: ";
            await context.PostAsync(reply);
            context.Wait(this.Recipe_id);

        }

        public async Task Recipe_id(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = (Activity)await argument;
            recipe = message.Text;
            RecipeResult rec2 = new RecipeResult();
            var reply = context.MakeMessage();
            //call api
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
              client.DefaultRequestHeaders.Add("user-key", "2e390670a23bce1d759e970c48815629");
            string uri = "http://food2fork.com/api/get?key=1d2514ae39ac1c3a5326688fc4e1ca7d&rId=" + recipe;
            HttpResponseMessage msgrecipe = await client.GetAsync(uri);
            if (msgrecipe.IsSuccessStatusCode)
            {
                var jsonResponse3 = await msgrecipe.Content.ReadAsStringAsync();
                rec2 = JsonConvert.DeserializeObject<RecipeResult>(jsonResponse3);
            }
            reply.Text = "This is it XD : \n\n\n\n";
            if (rec2.count != 0)
                reply.Text += "*" + rec2.recipes[0].title + "*\n\n" + "\n\n" + "[Click here for Source page](" + rec2.recipes[0].source_url + ")\n\n" + "![](" + rec2.recipes[0].image_url + ")\n\n\n\nIngredients: \n\n";
            else
                reply = await GetSearchResult(context,recipe);
            var i = 1;
            while (i < rec2.count && i< 10)
            {
                reply.Text += "*" + rec2.recipes[i].title + "*\n\n" + "\n\n" + "[Click here for Source page](" + rec2.recipes[i].source_url + ")\n\n" + "![](" + rec2.recipes[i].image_url + ")\n\n\n\nIngredients: \n\n";
                i++;
            }
            await context.PostAsync(reply);
            context.Wait(this.MessageReceivedAsync);
        }
        #endregion

        #region Zomato_API

        public async Task RESTO(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = (Activity)await argument;
            restaurant_name = message.Text;
            var reply = context.MakeMessage();
            reply.Text = "We need to filter the search so can you please enter the city: ";
            await context.PostAsync(reply);
            context.Wait(this.Resto_loc);
        }

        public async Task Zomato(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = (Activity)await argument;
            var reply = context.MakeMessage();
            restaurant_loc = message.Text;
            //call api
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("user-key", "2e390670a23bce1d759e970c48815629");
            string uri = "https://developers.zomato.com/api/v2.1/locations?query=" + restaurant_loc;
            HttpResponseMessage msg = await client.GetAsync(uri);
            if (msg.IsSuccessStatusCode)
            {
                //LocationAPI loc = msg.Content.ReadAsAsync(typeof(LocationAPI)).Result as LocationAPI;
                var jsonResponse = await msg.Content.ReadAsStringAsync();
                LocationAPI loc2 = JsonConvert.DeserializeObject<LocationAPI>(jsonResponse);
                string URI2 = "https://developers.zomato.com/api/v2.1/location_details?entity_id=" + loc2.location_suggestions.ElementAt(0).entity_id + "&entity_type=" + loc2.location_suggestions.ElementAt(0).entity_type;
                HttpResponseMessage msg2 = await client.GetAsync(URI2);
                if (msg2.IsSuccessStatusCode)
                {
                    //loc1 = msg.Content.ReadAsAsync(typeof(LocationDetails)).Result as LocationDetails;
                    var jsonResponse1 = await msg2.Content.ReadAsStringAsync();
                    LocationDetails loc1 = JsonConvert.DeserializeObject<LocationDetails>(jsonResponse1);
                    
                    if (loc1.best_rated_restaurant.Length == 0)
                    {
                        reply.Text = "\n\n Sorry Restaurant not registered under zomato database ";
                        //google search
                    }
                    else
                    {
                        reply.Attachments = new List<Microsoft.Bot.Connector.Attachment>();
                        var x = loc1.best_rated_restaurant[0].restaurant;
                        var y = loc1.best_rated_restaurant.Length;
                        var i = 0;
                        while (i < y && i <= 5)
                        {
                            x = loc1.best_rated_restaurant[i].restaurant;
                            
                            var heroCard = new HeroCard
                            {
                                Title = x.name+","+x.location.city,
                                Text = x.cuisines+" User Rating: "+x.user_rating.aggregate_rating,
                                Images = new List<CardImage>
                                    {
                                        new CardImage
                                        {
                                                Url = x.featured_image
                                        }
                                    }
                            };
                            var buttons = new List<CardAction>();
                            var button = new CardAction
                            {
                                Type = "openUrl",
                                Title = "View Restaurant",
                                Value = x.url
                            };
                            buttons.Add(button);
                            heroCard.Buttons = buttons;
                            reply.Attachments.Add(heroCard.ToAttachment());

                            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;

                            i++;
                        }
                    }



                }
                else
                { reply = await GetSearchResult(context, message.Text); }     //google search
            }
            else
            { reply = await GetSearchResult(context, message.Text); }//google search
            await context.PostAsync(reply);
            context.Wait(this.MessageReceivedAsync);
        }

        public async Task Resto_loc(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = (Activity)await argument;
            var area = message.Text;
            var reply = context.MakeMessage();
            //call api
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("user-key", "2e390670a23bce1d759e970c48815629");
            string uri = "https://developers.zomato.com/api/v2.1/locations?query=" + area;
            HttpResponseMessage msg = await client.GetAsync(uri);
            if (msg.IsSuccessStatusCode)
            {
                var jsonResponse = await msg.Content.ReadAsStringAsync();
                LocationAPI loc3 = JsonConvert.DeserializeObject<LocationAPI>(jsonResponse);
                // HttpClient client = new HttpClient();
                // client.DefaultRequestHeaders.Accept.Clear();
                // client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                // client.DefaultRequestHeaders.Add("user-key", "2e390670a23bce1d759e970c48815629");
                uri = "https://developers.zomato.com/api/v2.1/search?entity_id=" + loc3.location_suggestions.ElementAt(0).entity_id + "&entity_type=" + loc3.location_suggestions.ElementAt(0).entity_type + "&q=" + restaurant_name;
                HttpResponseMessage msg3 = await client.GetAsync(uri);
                if (msg3.IsSuccessStatusCode)
                {
                    //         loc = msg3.Content.ReadAsAsync(typeof(SpecificSearch)).Result as SpecificSearch;
                    jsonResponse = await msg3.Content.ReadAsStringAsync();
                    SpecificSearch loc = JsonConvert.DeserializeObject<SpecificSearch>(jsonResponse);
                    if (loc.restaurants.Length == 0)
                    {
                        reply.Text = "\n\n Sorry Restaurant not registered under zomato database ";
                        //google search
                    }
                    else
                    {
                        reply.Attachments = new List<Microsoft.Bot.Connector.Attachment>();
                        var x = loc.restaurants[0].restaurant;
                        var y = loc.results_found;
                        var i = 0;
                        while (i < y && i <= 5)
                        {
                            x = loc.restaurants[i].restaurant;
                            
                            var heroCard = new HeroCard
                            {
                                Title = x.name+","+x.location.city,
                                Text = x.cuisines+" User Rating: "+x.user_rating.aggregate_rating,
                                Images = new List<CardImage>
                                    {
                                        new CardImage
                                        {
                                                Url = x.featured_image
                                        }
                                    }
                            };
                            var buttons = new List<CardAction>();
                            var button = new CardAction
                            {
                                Type = "openUrl",
                                Title = "View Restaurant",
                                Value = x.url
                            };
                            buttons.Add(button);
                            heroCard.Buttons = buttons;
                            reply.Attachments.Add(heroCard.ToAttachment());

                            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;

                            i++;
                        }
                    }
                }
            }
            else
            {
                reply = await GetSearchResult(context, message.Text);  //google search
            }
            await context.PostAsync(reply);
            context.Wait(this.MessageReceivedAsync);
        }



        #endregion

        #region Medical

        public async Task Medical_Gender(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = (Activity)await argument;
            symptoms = message.Text;
            var reply = context.MakeMessage();
            reply.Text = "Please enter your gender: ";
            await context.PostAsync(reply);
            context.Wait(this.Medical_age);

        }

        public async Task Medical_age(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = (Activity)await argument;
            gender = message.Text;
            var reply = context.MakeMessage();
            reply.Text = "Please enter your age: ";
            await context.PostAsync(reply);
            context.Wait(this.Medical);

        }

        public async Task Medical(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = (Activity)await argument;
            age = message.Text;
            var reply = context.MakeMessage();
            //call api



        }
        #endregion

        #region misc
        private async Task<IMessageActivity> GetItem(string id, string entity, Activity activity, IDialogContext context)
        {
            var items = JsonConvert.DeserializeObject<List<Item>>(QuestionAndAnswers.Json);
            var filteredItems = items.Where(x => x.id == id);
            var valid = false;
            foreach (var item in filteredItems)
            {
                if (item.Entity.Contains(entity))
                {
                    
                    return await GetAttachments(context, item, activity);
                    valid = true;
                }
            }
            if (valid == false)
            {
                return await GetSearchResult(context, activity.Text);
            }
            return null;
        }

        private async Task<IMessageActivity> GetAttachments(IDialogContext context, Item item, Activity activity)
        {
            var reply = context.MakeMessage();
            var user = reply.Recipient.Name;
            user = user.Split(' ').First();
            reply.Attachments = new List<Attachment>();

            if (item != null && item.Attachment != null)
            {
                Attachment attachment = new Attachment();
                attachment.ContentUrl = item.Attachment.ContentUrl;
                attachment.ContentType = item.Attachment.ContentType;
                reply.Attachments.Add(attachment);
            }
            if (item != null && item.Answer != null)
            {
                if (item.Answer == "hi")
                {
                    item.Answer = $"Hi {user}! Nice to meet you.";
                }
                reply.Text = item.Answer;
            }
            if (item != null && item.Buttons != null)
            {
                var card = new HeroCard();
                card.Buttons = new List<CardAction>();

                foreach (var button in item.Buttons)
                {
                    var cardAction = new CardAction
                    {
                        Type = button.Type,
                        Title = button.Title,
                        Value = button.Value
                    };

                    card.Buttons.Add(cardAction);
                }
                reply.Attachments.Add(card.ToAttachment());
                reply.AttachmentLayout = AttachmentLayoutTypes.List;
            }
            if (item != null && item.Cards != null)
            {
                foreach (var card in item.Cards)
                {
                    var heroCard = new HeroCard
                    {
                        Title = card.Title,
                        Subtitle = card.Subtitle,
                        Text = card.Text,
                        Images = new List<CardImage>
                            {
                                new CardImage
                                {
                                    Url = card.Image
                                }
                            }
                    };
                    var buttons = new List<CardAction>();
                    if (card.Buttons != null)
                    {
                        foreach (var btn in card.Buttons)
                        {
                            var button = new CardAction
                            {
                                Type = btn.Type,
                                Title = btn.Title,
                                Value = btn.Value
                            };

                            buttons.Add(button);
                        }
                    }

                    heroCard.Buttons = buttons;
                    reply.Attachments.Add(heroCard.ToAttachment());
                    reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
                    
                }
            }
            return reply;
        }


        private async Task<IMessageActivity> GetSearchResult(IDialogContext context, string message)
        {
            googleSearch g = new googleSearch();
            googleSearchResponse result = await g.search(message);

            var reply = context.MakeMessage();
            reply.Attachments = new List<Microsoft.Bot.Connector.Attachment>();

            foreach (var r in result.items)
            {
                var heroCard = new HeroCard
                {
                    Title = r.title,
                    Text = r.snippet,
                    Images = new List<CardImage>
                                    {
                                        new CardImage
                                        {
                                                Url = r.pagemap.cse_thumbnail == null ? "http://img00.deviantart.net/481e/i/2012/143/6/1/bulb_thinking_by_joanedesign-d50u5yw.jpg" :r.pagemap.cse_thumbnail[0].src
                                        }
                                    }
                };
                var buttons = new List<CardAction>();
                var button = new CardAction
                {
                    Type = "openUrl",
                    Title = r.title,
                    Value = r.link
                };
                buttons.Add(button);

                heroCard.Buttons = buttons;
                reply.Attachments.Add(heroCard.ToAttachment());
            }
            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            return reply;
        }
        #endregion


        #region IOT

        public void updatedb(string v)
        {

            // Create the connectionString
            // Trusted_Connection is used to denote the connection uses Windows Authentication
            //var connection = new SqlConnection(
              //  "Server=tcp:pavanintern.database.windows.net,1433;Database=pavanintern;User ID=pavanintern;Password=9787082757Msd;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
                //);

            var connection = new SqlConnection("Server = tcp:pavanintern.database.windows.net,1433; Initial Catalog = pavanintern; Persist Security Info = False; User ID = pavanintern; Password = 9787082757Msd; MultipleActiveResultSets = False; Encrypt = True; TrustServerCertificate = False; Connection Timeout = 30;");

            connection.Open();
            //Console.WriteLine("Connected successfully.");
            string x = v;
            InsertRows(connection, x);

            //Console.WriteLine("Press any key to finish...");
            //Console.ReadKey(true);
        }

        public void InsertRows(SqlConnection connection, string x)
        {
            SqlParameter parameter;

            using (var command = new SqlCommand())
            {
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = @"INSERT INTO dbo.Msjarvis (device) VALUES (@device);";

                parameter = new SqlParameter("@device", SqlDbType.NVarChar, 50);
                parameter.Value = x;
                command.Parameters.Add(parameter);
                command.BeginExecuteNonQuery();
                //if (h.IsCompleted)
                //connection.Close();

                //throw new NotImplementedException();
            }
        }
        #endregion

    }
}