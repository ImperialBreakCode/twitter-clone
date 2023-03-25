using System.ComponentModel.DataAnnotations.Schema;
using TwitterUni.Data.Entities;

namespace TwitterUni.Services.ModelData
{
    public class UserData
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
