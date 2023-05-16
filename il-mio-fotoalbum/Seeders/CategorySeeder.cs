using il_mio_fotoalbum.Models;
using Microsoft.EntityFrameworkCore;

namespace il_mio_fotoalbum.Seeders
{
    public class CategorySeeder : Seeder<Category>
    {

        public CategorySeeder() : base(new()
            {
                new("InstaLove", "heart", "#FF0000", false),
                new("PicOfTheDay", "camera", "#000000", false),
                new("InstaGood", "thumbs-up", "#00FF00", false),
                new("PhotoOfTheDay", "image", "#000000", false),
                new("InstaLife", "globe", "#0000FF", false),
                new("Beautiful", "star", "#FFD700", false),
                new("Fashionista", "shopping-bag", "#800080", false),
                new("TravelGram", "plane", "#008000", false),
                new("FoodPorn", "utensils", "#FFA500", false),
                new("FitnessMotivation", "dumbbell", "#FF4500", false),
                new("SelfieTime", "camera-retro", "#000000", false),
                new("NatureLovers", "tree", "#008000", false),
                new("Wanderlust", "compass", "#800080", false),
                new("InstaFashion", "diamond", "#FFD700", false),
                new("Artistic", "palette", "#0000FF", false),
                new("SunsetLovers", "sun", "#FFA500", false),
                new("PetsOfInstagram", "paw", "#800080", false),
                new("InstaQuote", "quote-right", "#FF0000", false),
                new("HappyMoments", "smile", "#FF4500", false),
                new("CreativeMinds", "lightbulb", "#FFFF00", false),
                new("Favorite", "heart", "#FF0000", true),
                new("Premium", "crown", "#FFD700", true),
                new("AdventureTime", "mountain", "#008000", false),
                new("ThrowbackThursday", "clock", "#000000", false),
                new("InstaFriends", "users", "#0000FF", false),
            })
        { }

        public override DbSet<Category> GetDbSet(PhotoAlbumContext context)
        {
            return context.Categories;
        }
    }
}
