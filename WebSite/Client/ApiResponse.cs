namespace FoodApp.Client {
    public class ApiResponse<T> 
    {
        public bool Success { get; set; }
        public T Model { get; set; }

        public string Error { get; set; }

        public System.Net.HttpStatusCode StatusCode { get; set; }
        public System.Net.WebHeaderCollection Headers { get; set; }
    }
}