using Practice.Models;

namespace Practice.Responses
{
    public class updateResponse
    {
        public bool success {  get; set; }
        public Product product { get; set; }=new Product();
    }
}
