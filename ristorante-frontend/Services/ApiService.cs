using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ristorante_frontend.Models;
using System.Net.Http.Json;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using ristorante_frontend.NewFolder;
using System.Net.Http.Headers;

namespace ristorante_frontend.Services
{
    public class ApiService
    {
        public const string API_URL = "https://localhost:7205";
        public static string Email { get; set; }
        public static string Password { get; set; }

        public enum ApiServiceResultType
        {
            Success,
            Error
        }


        public static async Task<ApiServiceResult<bool>> Register()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var httpResult = await client.PostAsync($"{API_URL}/Account/Register",
                        JsonContent.Create(new { Email = Email, Password = Password }));
                    var resultBody = await httpResult.Content.ReadAsStringAsync();
                    var data = httpResult.IsSuccessStatusCode;
                    return new ApiServiceResult<bool>(data);
                }

            }
            catch (Exception e)
            {
                return new ApiServiceResult<bool>(e);
            }
        }

        public static async Task<ApiServiceResult<Jwt>> GetJwtToken()
        {
            try
            {
                using HttpClient client = new HttpClient();
                // PostAsync() prende due parametri: URL e body della richesta
                var httpResult = await client.PostAsync($"{API_URL}/Account/Login", JsonContent.Create(new { Email = Email, Password = Password }));
                // Se la chiamata ha successo avremo in resultBody il JSON del body della risposta HTTP: in questo caso { token: ..., expirationUtc: ... }
                var resultBody = await httpResult.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<Jwt>(resultBody);
                if (data.Token == null)
                {
                    return new ApiServiceResult<Jwt>(new Exception("Login fallito"));
                }
                AddRolesToJwt(data);
                return new ApiServiceResult<Jwt>(data);
            }
            catch (Exception e)
            {
                return new ApiServiceResult<Jwt>(e);
            }
        }
        private static void AddRolesToJwt(Jwt jwt)
        {
            try
            {
                // Decodifichiamo il JWT
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(jwt.Token);

                // Vediamo se ci sono ruoli nel JWT
                var roles = jwtToken.Claims
                    .Where((Claim c) => c.Type == "role")
                    .Select(c => c.Value).ToList();

                // Aggiungiamoli nel nostro DTO (data transfer object) rappresentante il JWT
                jwt.Roles = roles;
            }
            catch { } // Se succede qualcosa non facciamo nulla
        }


        public static async Task<ApiServiceResult<List<Dish>>> GetDishes()
        {
            try
            {
                using HttpClient client = new HttpClient();
                var httpResult = await client.GetAsync($"{API_URL}/Dish");
                var resultBody = await httpResult.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<List<Dish>>(resultBody);
                return new ApiServiceResult<List<Dish>>(data);
            }
            catch (Exception e)
            {
                return new ApiServiceResult<List<Dish>>(e);
            }
        }

        /// <summary>
        /// Richiama l'API per creare un dish e ne ritorna, in caso di successo, un interno che rappresenta l'ID del nuovo dish
        /// </summary>
        /// <param name="newDish"></param>
        /// <param name="jwt"></param>
        /// <returns></returns>
        public static async Task<ApiServiceResult<int>> CreateDish(Dish newDish, Jwt jwt)
        {
            try
            {
                using HttpClient httpClient = new HttpClient();
                // Devo aggiungere il JWT
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt.Token);

                var httpResult = await httpClient.PostAsync($"{API_URL}/Dish", JsonContent.Create(newDish));
                var resultBody = await httpResult.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<int>(resultBody); // L'API ritorna un intero che rappresenta il nuovo ID del dish
                return new ApiServiceResult<int>(data);
            }
            catch (Exception e)
            {
                return new ApiServiceResult<int>(e);
            }
        }
        /// <summary>
        /// Richiamo l'API per aggiornare un dish. Ritorna il numero di righe interessate
        /// </summary>
        public static async Task<ApiServiceResult<int>> UpdateDish(Dish dish, Jwt token)
        {
            try
            {
                using HttpClient httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Token);

                var httpResult = await httpClient.PutAsync($"{API_URL}/Dish/{dish.ID_Dish}", JsonContent.Create(dish));
                var resultBody = await httpResult.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<int>(resultBody);
                return new ApiServiceResult<int>(data);
            }
            catch (Exception e)
            {
                return new ApiServiceResult<int>(e);
            }
        }
        public static async Task<ApiServiceResult<int>> DeleteDish(int dishId, Jwt token)
        {
            try
            {
                using HttpClient httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Token);

                var httpResult = await httpClient.DeleteAsync($"{API_URL}/Dish/{dishId}");
                var resultBody = await httpResult.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<int>(resultBody);
                return new ApiServiceResult<int>(data);
            }
            catch (Exception e)
            {
                return new ApiServiceResult<int>(e);
            }
        }
    }
}
