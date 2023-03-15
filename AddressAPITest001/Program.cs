using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AddressAPITest001
{
    class Program
    {
        static void Main(string[] args)
        {
            SaveToDatabase(); return;

            var filename = @"C:\Tmp\31\r_streetsByLocality.json";
            var json2 = File.ReadAllText(filename);
            var dstreets = JsonConvert.DeserializeObject<StreetQueryData>(json2);
            var streets = dstreets.data.query;

            var streets2 = streets.Where(q => q.history != null && q.history.Any()).OrderByDescending(q => q.history.Length).ToArray();

            var token = GetToken();

            //марьяненко
            //322a290a-370f-11e7-9674-000c29ff5864

            foreach (var street in streets)
            {
                BuildAddressByStreet(street.id, token);

            }
            return;

            var httpClient = new HttpClient();
            //HttpWebRequest.pr

            var request = @"{ query: countries(match:""Укра"", locale:""ua"")  { 
	id 
	name {	fullName shortName fullToponym shortToponym isToponymBeforeName } 
	codeIsoAlpha2 
	codeIsoAlpha3
	history {fullName shortName fullToponym shortToponym isToponymBeforeName } 
	incorrect { fullName shortName fullToponym shortToponym isToponymBeforeName } 
	locale 
} }
";

            var req2 = @"{
  query: localitiesByCountry(
    match: """"
    ofCountry: ""1437c9b6-370f-11e7-8ed7-000c29ff5864""
    locale: ""ua""
  ) {
    id
    name {
      fullName
      shortName
      fullToponym
      shortToponym
      isToponymBeforeName
    }
    koatuu
    isRegionCenter
    isSubregionCenter
    isStateLevel
    ofRegion {
      id
      name {
        fullName
      }
    }
  }
}
";

            var req4 = @"{
  addressById(id: ""54185882-371b-11e7-99b9-000c29ff5864"", component: address_object, locale: ""UA"") {
    addressedObject {
      id
    }
    asString
  }
}
";

            var req3 = @"{ query: countries(match:""Укра"", locale:""ua"")  { 
	id 
	name {	fullName shortName fullToponym shortToponym isToponymBeforeName } 
	codeIsoAlpha2 
	codeIsoAlpha3
	history {fullName shortName fullToponym shortToponym isToponymBeforeName } 
	incorrect { fullName shortName fullToponym shortToponym isToponymBeforeName } 
	locale 
} }
";
            var req5 = @"{ ""query"": ""{ countries { code name } } "" }";

            var req6 = @"{ ""query"": ""{ countries { id } } "" }";
            //var req6 = @"{ ""query"": ""{ countries { code name } } "" }";


            var req7 = @"
{""query"":""{ query: countryById(id: \""1437c9b6-370f-11e7-8ed7-000c29ff5864\"", locale: \""\"") { \n\tid \n\tname {\tfullName shortName fullToponym shortToponym isToponymBeforeName } \n    \tcodeIsoAlpha2 \n    \tcodeIsoAlpha3\n\thistory {fullName shortName fullToponym shortToponym isToponymBeforeName } \n\tincorrect { fullName shortName fullToponym shortToponym isToponymBeforeName } \n\tlocale \n} }\n"",""variables"":{}}

";
            var treq_ = @"{ query: countryById(id: ""1437c9b6-370f-11e7-8ed7-000c29ff5864"", locale: """") { 
	id 
	name {	fullName shortName fullToponym shortToponym isToponymBeforeName } 
    	codeIsoAlpha2 
    	codeIsoAlpha3
	history {fullName shortName fullToponym shortToponym isToponymBeforeName } 
	incorrect { fullName shortName fullToponym shortToponym isToponymBeforeName } 
	locale 
} }";

            var treq = @"
{ query: countries(locale:""ua"")  { 

    id
    name { fullName shortName fullToponym shortToponym isToponymBeforeName }
            codeIsoAlpha2
            codeIsoAlpha3

    history { fullName shortName fullToponym shortToponym isToponymBeforeName }
            incorrect { fullName shortName fullToponym shortToponym isToponymBeforeName }
            locale
}
    }

";

            var ukrainaId = "1437c9b6-370f-11e7-8ed7-000c29ff5864";
            var locationKievId = "538d7492-371b-11e7-b112-000c29ff5864";

            treq = File.ReadAllText(@"C:\Tmp\31\query.txt");

            var obj = new QueryClass { query = treq };
            var qjson = JsonConvert.SerializeObject(obj);

            //token = "eyJ4NXQiOiJNell4TW1Ga09HWXdNV0kwWldObU5EY3hOR1l3WW1NNFpUQTNNV0kyTkRBelpHUXpOR00wWkdSbE5qSmtPREZrWkRSaU9URmtNV0ZoTXpVMlpHVmxOZyIsImtpZCI6Ik16WXhNbUZrT0dZd01XSTBaV05tTkRjeE5HWXdZbU00WlRBM01XSTJOREF6WkdRek5HTTBaR1JsTmpKa09ERmtaRFJpT1RGa01XRmhNelUyWkdWbE5nX1JTMjU2IiwiYWxnIjoiUlMyNTYifQ.eyJzdWIiOiJES1YiLCJhdXQiOiJBUFBMSUNBVElPTiIsImF1ZCI6ImJEVWhBZFNsZjdZZjQ0d3JrcW1id2hqMXZMMGEiLCJuYmYiOjE2Nzg4NDc4NDcsImF6cCI6ImJEVWhBZFNsZjdZZjQ0d3JrcW1id2hqMXZMMGEiLCJzY29wZSI6ImRlZmF1bHQiLCJpc3MiOiJodHRwczpcL1wvYXBpLWFkbWluLmt5aXZjaXR5Lmdvdi51YTo0NDNcL29hdXRoMlwvdG9rZW4iLCJleHAiOjE2Nzg4NTE0NDcsImlhdCI6MTY3ODg0Nzg0NywianRpIjoiYTdlZWRhNjYtZmQ5NC00YjJlLWIyMmYtMGU4ZDAxNTRiNzkwIn0.nNnD8wPJMFOAfW3BgGFtU4GTBQcBVwnclNR-4_1MPNRffeIXrf-de0VnHOOenqz0N0RGns9UkuQ9qfkf9GXw1A4Swa8_9ReCERDuSqSlmIm_omw8WUtxw3M_8UWqSDhn4guLaYaOaUQ9VaVb5FJ7ynreG828XR3t9bNCHrzoZYFa0trGSyftolz-E9LrqPU1rJkcJDj6V04OHwviIhSzI782-tySB61BC_oqh5MWwg31vcW9wts_rz-Z0JgOAY9R8IP7VpuJ9YjSDXJDBIaPr07XiH6Wtz7lO__gV8Zm0OoXxhutKrZ4mkFFvQUSQHj8xxHdeU2sT4YYUDyFWe-_kw";
            httpClient.DefaultRequestHeaders.Add("Authorization", @"Bearer " + token);


            //var content = new StringContent(req7);
            var content = new StringContent(qjson);

            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            //content.Headers.Add("Authorization", "Bearer xxxxxxxxxxx");

            
            var baseURL = @"https://api.kyivcity.gov.ua/urbio-adreses/1.0";
			//var baseURL = @"https://address-stage.kyivcity.gov.ua/address";
			var response = httpClient.PostAsync(baseURL, content).Result;
			var responseText = response.Content.ReadAsStringAsync().Result;
            File.WriteAllText(@"C:\Tmp\31\response.txt", responseText);
            //Console.WriteLine(responseText);
		}

        

        static void BuildAddressByStreet(Guid streetId, string token)
        {
            //var token = GetToken();
            var httpClient = new HttpClient();

            var treq = @"
{ query: aosByStreet(L1match: ""*"", L2match: """", L3match: """", ofStreet: ""323917bc-370f-11e7-9a1a-000c29ff5864"", locale: ""UA"") { 
	id 
	name { 
		ofFirstLevel { fullName shortName fullToponym shortToponym isToponymBeforeName } 
		ofSecondLevel { fullName shortName fullToponym shortToponym isToponymBeforeName } 
		ofThirdLevel { fullName shortName fullToponym shortToponym isToponymBeforeName } } 
	category { fullText shortText } 
	zip 
	geolocation { lat lon } 
	history { 
		ofFirstLevel { fullName shortName fullToponym shortToponym isToponymBeforeName } 
		ofSecondLevel { fullName shortName fullToponym shortToponym isToponymBeforeName }
		ofThirdLevel { fullName shortName fullToponym shortToponym isToponymBeforeName } }
	incorrect { 
		ofFirstLevel { fullName shortName fullToponym shortToponym isToponymBeforeName } 
		ofSecondLevel { fullName shortName fullToponym shortToponym isToponymBeforeName } 
		ofThirdLevel { fullName shortName fullToponym shortToponym isToponymBeforeName } } 
	ofDistrict { 
		id 
		name { fullName shortName fullToponym shortToponym isToponymBeforeName } }
	ofStreet { 
		id 
		name { fullName shortName fullToponym shortToponym isToponymBeforeName } }
	asString 
	locale 
} }

";

            treq = treq.Replace("323917bc-370f-11e7-9a1a-000c29ff5864", streetId.ToString());

            var obj = new QueryClass { query = treq };
            var qjson = JsonConvert.SerializeObject(obj);

            //token = "eyJ4NXQiOiJNell4TW1Ga09HWXdNV0kwWldObU5EY3hOR1l3WW1NNFpUQTNNV0kyTkRBelpHUXpOR00wWkdSbE5qSmtPREZrWkRSaU9URmtNV0ZoTXpVMlpHVmxOZyIsImtpZCI6Ik16WXhNbUZrT0dZd01XSTBaV05tTkRjeE5HWXdZbU00WlRBM01XSTJOREF6WkdRek5HTTBaR1JsTmpKa09ERmtaRFJpT1RGa01XRmhNelUyWkdWbE5nX1JTMjU2IiwiYWxnIjoiUlMyNTYifQ.eyJzdWIiOiJES1YiLCJhdXQiOiJBUFBMSUNBVElPTiIsImF1ZCI6ImJEVWhBZFNsZjdZZjQ0d3JrcW1id2hqMXZMMGEiLCJuYmYiOjE2Nzg4NDc4NDcsImF6cCI6ImJEVWhBZFNsZjdZZjQ0d3JrcW1id2hqMXZMMGEiLCJzY29wZSI6ImRlZmF1bHQiLCJpc3MiOiJodHRwczpcL1wvYXBpLWFkbWluLmt5aXZjaXR5Lmdvdi51YTo0NDNcL29hdXRoMlwvdG9rZW4iLCJleHAiOjE2Nzg4NTE0NDcsImlhdCI6MTY3ODg0Nzg0NywianRpIjoiYTdlZWRhNjYtZmQ5NC00YjJlLWIyMmYtMGU4ZDAxNTRiNzkwIn0.nNnD8wPJMFOAfW3BgGFtU4GTBQcBVwnclNR-4_1MPNRffeIXrf-de0VnHOOenqz0N0RGns9UkuQ9qfkf9GXw1A4Swa8_9ReCERDuSqSlmIm_omw8WUtxw3M_8UWqSDhn4guLaYaOaUQ9VaVb5FJ7ynreG828XR3t9bNCHrzoZYFa0trGSyftolz-E9LrqPU1rJkcJDj6V04OHwviIhSzI782-tySB61BC_oqh5MWwg31vcW9wts_rz-Z0JgOAY9R8IP7VpuJ9YjSDXJDBIaPr07XiH6Wtz7lO__gV8Zm0OoXxhutKrZ4mkFFvQUSQHj8xxHdeU2sT4YYUDyFWe-_kw";
            httpClient.DefaultRequestHeaders.Add("Authorization", @"Bearer " + token);


            var content = new StringContent(qjson);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var baseURL = @"https://api.kyivcity.gov.ua/urbio-adreses/1.0";
            var response = httpClient.PostAsync(baseURL, content).Result;
            var json = response.Content.ReadAsStringAsync().Result;
            File.WriteAllText(@"C:\Tmp\31\Addr\" + streetId + ".json", json);

            var addrobj = JsonConvert.DeserializeObject<AddressedObjectData>(json);
        }

        public class AddressedObject
        {
            public Guid id { get; set; }
            public AddressedObjectName name { get; set; }
            public Classifier category { get; set; }
            public String zip { get; set; }
            public GeoPoint[] geolocation { get; set; }
            public AddressedObjectName[] history { get; set; }
            public AddressedObjectName[] incorrect { get; set; }
            public AddressUnit ofDistrict { get; set; }
            public AddressUnit ofStreet { get; set; }
            public String asString { get; set; }
            public String locale { get; set; }
        }

        public class AddressedObjectQuery
        {
            public AddressedObject[] query { get; set; }
        }

        public class AddressedObjectData
        {
            public AddressedObjectQuery data { get; set; }
        }

        public class AddressedObjectName
        {
            public TopographicName ofFirstLevel { get; set; }
            public TopographicName ofSecondLevel { get; set; }
            public TopographicName ofThirdLevel { get; set; }
        }


        public class QueryClass
        {
            public string query { get; set; }
        }

        public class TopographicName
        {
            public string fullName { get; set; }
            public string shortName { get; set; }
            public string fullToponym { get; set; }
            public string shortToponym { get; set; }
            public bool? isToponymBeforeName { get; set; }
        }

        public class Classifier
        {
            public String fullText { get; set; }
            public String shortText { get; set; }
        }

        public class GeoPoint
        {
            public decimal lat { get; set; }
            public decimal lon { get; set; }
        }

        public class AddressUnit
        {
            public Guid? id { get; set; }
            public TopographicName name { get; set; }
        }


        public class Street
        {
            public Guid id { get; set; }
            public TopographicName name { get; set; }
            public String cadastreCode { get; set; }
            public Classifier uniqueMarker { get; set; }
            public GeoPoint[] geolocation { get; set; }
            public TopographicName[] history { get; set; }
            public TopographicName[] incorrect { get; set; }
            public AddressUnit ofLocality { get; set; }
            public AddressUnit[] ofDistrict { get; set; }
            public Classifier description { get; set; }
            public String asString { get; set; }
            public String locale { get; set; }
        }

        public class StreetQuery
        {
            public Street[] query { get; set; }
        }

        public class StreetQueryData
        {
            public StreetQuery data { get; set; }
        }



        static string GetToken()
        {
            /*
            var client = new RestClient();
            var request = new RestRequest(@"https://api-admin.kyivcity.gov.ua/oauth2/token", Method.Post);
            //request.AddHeader("cache-control", "no-cache");
            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            request.AddParameter("application/x-www-form-urlencoded", "grant_type=client_credentials", ParameterType.RequestBody);
            request.AddHeader("Authorization", @"Bearer YkRVaEFkU2xmN1lmNDR3cmtxbWJ3aGoxdkwwYTppNVpFSGd3MHZLdXlScFNqaVBoWHptcnlvYndh");
            var response = client.Execute(request);

            var gg = response.Content;

            return "";
            */

            
            var httpClient = new HttpClient();
            
            httpClient.DefaultRequestHeaders.Add("Authorization", @"Basic YkRVaEFkU2xmN1lmNDR3cmtxbWJ3aGoxdkwwYTppNVpFSGd3MHZLdXlScFNqaVBoWHptcnlvYndh");

            var content = new StringContent(@"grant_type=client_credentials");

            content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
            //content.Headers.Add("Authorization", "Bearer xxxxxxxxxxx");


            var baseURL = @"https://api-admin.kyivcity.gov.ua/oauth2/token";
            var response = httpClient.PostAsync(baseURL, content).Result;
            var json = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine(json);

            var info = JsonConvert.DeserializeObject<TokenInfo>(json);

            return info.access_token;
        }

        public class TokenInfo
        {
            public string access_token { get; set; }
        }

        static void SaveToDatabase()
        {
            //SaveToDatabaseStreets();
            SaveToDatabaseAddressAll();
        }

        static bool SkipStreet(Guid street_id)
        {
            return (street_id == new Guid("32138CFE-370F-11E7-9256-000C29FF5864"));
        }

        static void SaveToDatabaseAddressAll()
        {
            var db = new GUKVEntities();

            int npp = 0;
            var files = Directory.GetFiles(@"C:\Tmp\31\Addr");
            foreach(var filename in files)
            {
                npp++;
                Console.WriteLine("adr=" + npp + "/" + files.Length);

                var street_id = Guid.Parse(Path.GetFileNameWithoutExtension(filename));
                if (SkipStreet(street_id)) continue;
                
                SaveToDatabaseAddress(filename, street_id);
            }
        }

        static void SaveToDatabaseAddress(string filename, Guid street_id)
        {
            var db = new GUKVEntities();

            var json2 = File.ReadAllText(filename);
            var jobject = JsonConvert.DeserializeObject<AddressedObjectData>(json2);
            var adrlist = jobject.data.query;

            var list_adresesUrbio_address = new List<adresesUrbio_address>();
            var list_adresesUrbio_address_geolocation = new List<adresesUrbio_address_geolocation>();
            var list_adresesUrbio_address_history = new List<adresesUrbio_address_history>();
            var list_adresesUrbio_address_incorrect = new List<adresesUrbio_address_incorrect>();

            
            foreach (var adr in adrlist)
            {
                var adresesUrbio_address = new adresesUrbio_address
                {
                    address_id = adr.id,
                    fullName = adr.name.ofFirstLevel.fullName,
                    shortName = adr.name.ofFirstLevel.shortName,
                    fullToponym = adr.name.ofFirstLevel.fullToponym,
                    shortToponym = adr.name.ofFirstLevel.shortToponym,
                    isToponymBeforeName = adr.name.ofFirstLevel.isToponymBeforeName.Value,
                    secondLevel_fullName = adr.name.ofSecondLevel?.fullName,
                    secondLevel_shortName = adr.name.ofSecondLevel?.shortName,
                    secondLevel_fullToponym = adr.name.ofSecondLevel?.fullToponym,
                    secondLevel_shortToponym = adr.name.ofSecondLevel?.shortToponym,
                    secondLevel_isToponymBeforeName = adr.name.ofSecondLevel?.isToponymBeforeName ?? true,
                    thirdLevel_fullName = adr.name.ofThirdLevel?.fullName,
                    thirdLevel_shortName = adr.name.ofThirdLevel?.shortName,
                    thirdLevel_fullToponym = adr.name.ofThirdLevel?.fullToponym,
                    thirdLevel_shortToponym = adr.name.ofThirdLevel?.shortToponym,
                    thirdLevel_isToponymBeforeName = adr.name.ofThirdLevel?.isToponymBeforeName ?? true,
                    category_fullText = adr.category?.fullText,
                    category_shortText = adr.category?.shortText,
                    zip = adr.zip,
                    street_id = adr.ofStreet.id,
                    street_fullName = adr.ofStreet.name.fullName,
                    street_shortName = adr.ofStreet.name.shortName,
                    street_fullToponym = adr.ofStreet.name.fullToponym,
                    street_shortToponym = adr.ofStreet.name.shortToponym,
                    street_isToponymBeforeName = adr.ofStreet.name.isToponymBeforeName.Value,
                    district_id = adr.ofDistrict.id,
                    district_fullName = adr.ofDistrict.name.fullName,
                    district_shortName = adr.ofDistrict.name.shortName,
                    district_fullToponym = adr.ofDistrict.name.fullToponym,
                    district_shortToponym = adr.ofDistrict.name.shortToponym,
                    district_isToponymBeforeName = adr.ofDistrict.name.isToponymBeforeName.Value,
                    asString = adr.asString,
                    locale = adr.locale,
                };
                list_adresesUrbio_address.Add(adresesUrbio_address);
                ////db.SaveChanges();
                
                if (street_id != adresesUrbio_address.street_id)
                {
                    throw new Exception();
                }

                if (adr.geolocation != null)
                {
                    var rownpp = 0;
                    foreach (var geolocation in adr.geolocation)
                    {
                        rownpp++;
                        list_adresesUrbio_address_geolocation.Add(new adresesUrbio_address_geolocation
                        {
                            address_id = adr.id,
                            rownpp = rownpp,
                            lat = geolocation.lat,
                            lon = geolocation.lon,
                        });
                    }
                    ////db.SaveChanges();
                }

                if (adr.history != null)
                {
                    var rownpp = 0;
                    foreach (var tname in adr.history)
                    {
                        rownpp++;
                        list_adresesUrbio_address_history.Add(new adresesUrbio_address_history
                        {
                            address_id = adr.id,
                            rownpp = rownpp,

                            fullName = tname.ofFirstLevel.fullName,
                            shortName = tname.ofFirstLevel.shortName,
                            fullToponym = tname.ofFirstLevel.fullToponym,
                            shortToponym = tname.ofFirstLevel.shortToponym,
                            isToponymBeforeName = tname.ofFirstLevel.isToponymBeforeName.Value,

                            secondLevel_fullName = tname.ofSecondLevel?.fullName,
                            secondLevel_shortName = tname.ofSecondLevel?.shortName,
                            secondLevel_fullToponym = tname.ofSecondLevel?.fullToponym,
                            secondLevel_shortToponym = tname.ofSecondLevel?.shortToponym,
                            secondLevel_isToponymBeforeName = tname.ofSecondLevel?.isToponymBeforeName ?? true,

                            thirdLevel_fullName = tname.ofThirdLevel?.fullName,
                            thirdLevel_shortName = tname.ofThirdLevel?.shortName,
                            thirdLevel_fullToponym = tname.ofThirdLevel?.fullToponym,
                            thirdLevel_shortToponym = tname.ofThirdLevel?.shortToponym,
                            thirdLevel_isToponymBeforeName = tname.ofThirdLevel?.isToponymBeforeName ?? true,
                        });
                    }
                    ////db.SaveChanges();
                }

                if (adr.incorrect != null)
                {
                    var rownpp = 0;
                    foreach (var tname in adr.incorrect)
                    {
                        rownpp++;
                        list_adresesUrbio_address_incorrect.Add(new adresesUrbio_address_incorrect
                        {
                            address_id = adr.id,
                            rownpp = rownpp,

                            fullName = tname.ofFirstLevel.fullName,
                            shortName = tname.ofFirstLevel.shortName,
                            fullToponym = tname.ofFirstLevel.fullToponym,
                            shortToponym = tname.ofFirstLevel.shortToponym,
                            isToponymBeforeName = tname.ofFirstLevel.isToponymBeforeName.Value,

                            secondLevel_fullName = tname.ofSecondLevel.fullName,
                            secondLevel_shortName = tname.ofSecondLevel.shortName,
                            secondLevel_fullToponym = tname.ofSecondLevel.fullToponym,
                            secondLevel_shortToponym = tname.ofSecondLevel.shortToponym,
                            secondLevel_isToponymBeforeName = tname.ofSecondLevel.isToponymBeforeName.Value,

                            thirdLevel_fullName = tname.ofThirdLevel.fullName,
                            thirdLevel_shortName = tname.ofThirdLevel.shortName,
                            thirdLevel_fullToponym = tname.ofThirdLevel.fullToponym,
                            thirdLevel_shortToponym = tname.ofThirdLevel.shortToponym,
                            thirdLevel_isToponymBeforeName = tname.ofThirdLevel.isToponymBeforeName.Value,
                        });
                    }
                    ////db.SaveChanges();
                }
            }

            db.adresesUrbio_address.AddRange(list_adresesUrbio_address);
            db.adresesUrbio_address_geolocation.AddRange(list_adresesUrbio_address_geolocation);
            db.adresesUrbio_address_history.AddRange(list_adresesUrbio_address_history);
            db.adresesUrbio_address_incorrect.AddRange(list_adresesUrbio_address_incorrect);

            db.SaveChanges();
        }

        static void SaveToDatabaseStreets()
        {
            var db = new GUKVEntities();

            var filename = @"C:\Tmp\31\r_streetsByLocality.json";
            var json2 = File.ReadAllText(filename);
            var dstreets = JsonConvert.DeserializeObject<StreetQueryData>(json2);
            var streets = dstreets.data.query;

            var list_adresesUrbio_street = new List<adresesUrbio_street>();
            var list_adresesUrbio_street_geolocation = new List<adresesUrbio_street_geolocation>();
            var list_adresesUrbio_street_district = new List<adresesUrbio_street_district>();
            var list_adresesUrbio_street_history = new List<adresesUrbio_street_history>();
            var list_adresesUrbio_street_incorrect = new List<adresesUrbio_street_incorrect>();


            int npp = 0;
            foreach (var street in streets)
            {
                npp++;
                Console.WriteLine("street=" + npp + "/" + streets.Length);

                if (SkipStreet(street.id)) continue;

                var adresesUrbio_street = new adresesUrbio_street
                {
                    street_id = street.id,
                    fullName = street.name.fullName,
                    shortName = street.name.shortName,
                    fullToponym = street.name.fullToponym,
                    shortToponym = street.name.shortToponym,
                    isToponymBeforeName = street.name.isToponymBeforeName.Value,
                    cadastreCode = street.cadastreCode,
                    uniqueMarker_fullText = street.uniqueMarker?.fullText,
                    uniqueMarker_shortText = street.uniqueMarker?.shortText,
                    locality_id = street.ofLocality.id,
                    locality_fullName = street.ofLocality.name.fullName,
                    locality_shortName = street.ofLocality.name.shortName,
                    locality_fullToponym = street.ofLocality.name.fullToponym,
                    locality_shortToponym = street.ofLocality.name.shortToponym,
                    locality_isToponymBeforeName = street.ofLocality.name.isToponymBeforeName.Value,
                    description_fullText = street.description?.fullText,
                    description_shortText = street.description?.shortText,
                    asString = street.asString,
                    locale = street.locale,
                };
                list_adresesUrbio_street.Add(adresesUrbio_street);
                ////db.SaveChanges();


                if (street.geolocation != null)
                {
                    var rownpp = 0;
                    foreach (var geolocation in street.geolocation)
                    {
                        rownpp++;
                        list_adresesUrbio_street_geolocation.Add(new adresesUrbio_street_geolocation
                        {
                            street_id = street.id,
                            rownpp = rownpp,
                            lat = geolocation.lat,
                            lon = geolocation.lon,
                        });
                    }
                    ////db.SaveChanges();
                }

                if (street.history != null)
                {
                    var rownpp = 0;
                    foreach (var tname in street.history)
                    {
                        rownpp++;
                        list_adresesUrbio_street_history.Add(new adresesUrbio_street_history
                        {
                            street_id = street.id,
                            rownpp = rownpp,
                            fullName = tname.fullName,
                            shortName = tname.shortName,
                            fullToponym = tname.fullToponym,
                            shortToponym = tname.shortToponym,
                            isToponymBeforeName = tname.isToponymBeforeName.Value,
                        });
                    }
                    ////db.SaveChanges();
                }

                if (street.incorrect != null)
                {
                    var rownpp = 0;
                    foreach (var tname in street.incorrect)
                    {
                        rownpp++;
                        list_adresesUrbio_street_incorrect.Add(new adresesUrbio_street_incorrect
                        {
                            street_id = street.id,
                            rownpp = rownpp,
                            fullName = tname.fullName,
                            shortName = tname.shortName,
                            fullToponym = tname.fullToponym,
                            shortToponym = tname.shortToponym,
                            isToponymBeforeName = tname.isToponymBeforeName.Value,
                        });
                    }
                    ////db.SaveChanges();
                }

                if (street.ofDistrict != null)
                {
                    var rownpp = 0;
                    foreach (var district in street.ofDistrict)
                    {
                        rownpp++;
                        list_adresesUrbio_street_district.Add(new adresesUrbio_street_district
                        {
                            street_id = street.id,
                            rownpp = rownpp,
                            fullName = district.name.fullName,
                            shortName = district.name.shortName,
                            fullToponym = district.name.fullToponym,
                            shortToponym = district.name.shortToponym,
                            isToponymBeforeName = district.name.isToponymBeforeName.Value,
                        });
                    }
                    ////db.SaveChanges();
                }
            }

            db.adresesUrbio_street.AddRange(list_adresesUrbio_street);
            db.adresesUrbio_street_geolocation.AddRange(list_adresesUrbio_street_geolocation);
            db.adresesUrbio_street_district.AddRange(list_adresesUrbio_street_district);
            db.adresesUrbio_street_history.AddRange(list_adresesUrbio_street_history);
            db.adresesUrbio_street_incorrect.AddRange(list_adresesUrbio_street_incorrect);

            Console.WriteLine("SaveChanges - before");
            db.SaveChanges();
            Console.WriteLine("SaveChanges - after");
        }
    }
}
