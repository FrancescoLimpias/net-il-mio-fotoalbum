using il_mio_fotoalbum.Models;
using il_mio_fotoalbum.Utilities;
using Microsoft.EntityFrameworkCore;

namespace il_mio_fotoalbum.Seeders
{
    public class PhotoSeeder : Seeder<ValueTuple<string, string>, Photo>
    {

        public PhotoSeeder() : base(new()
        {
            ("Serenity", "A peaceful moment by the lake."),
            ("Urban Jungle", "Exploring the concrete wilderness."),
            ("Golden Glow", "Basking in the warm sunset hues."),
            ("Captivating Cosmos", "Stargazing under a clear night sky."),
            ("City Rhythms", "Vibrant streets bustling with life."),
            ("Tranquil Oasis", "Discovering a hidden paradise."),
            ("Whispering Woods", "Nature's symphony among the trees."),
            ("Vintage Charm", "Nostalgic treasures from the past."),
            ("Waves of Joy", "Embracing the refreshing ocean waves."),
            ("Enchanting Twilight", "Mystical moments as day turns into night."),
            ("Dazzling Delights", "Sparkling lights illuminating the city."),
            ("Rustic Elegance", "Timeless beauty in rural surroundings."),
            ("Harmony in Motion", "Graceful dance in perfect synchronization."),
            ("Floating Dreams", "Drifting away on a cloud of imagination."),
            ("Whimsical Wonder", "A whimsical journey through imagination."),
            ("Majestic Peaks", "Conquering the summit with awe-inspiring views."),
            ("Silent Symphony", "The beauty of music in serene silence."),
            ("Lost in Time", "Exploring forgotten places with untold stories."),
            ("Burst of Colors", "Vibrant hues painting the world with joy."),
            ("Soulful Stroll", "Walking the path of self-discovery."),
            ("Candid Moments", "Authentic expressions frozen in time."),
            ("Eternal Beauty", "Timeless allure captured in a single frame."),
            ("Whirling Dervish", "A captivating dance of tradition and grace."),
            ("Glorious Harvest", "Celebrating the bounties of nature's yield."),
            ("Aerial Marvels", "Discovering the world from new heights."),
            ("Reflections of Tranquility", "Mirror-like waters calming the soul."),
            ("Urban Vibes", "Embracing the energy of the city streets."),
            ("Gentle Glimmer", "Soft sunlight filtering through the leaves."),
            ("Magical Encounters", "Unforgettable moments of enchantment."),
            ("Melodies of Nature", "The symphony of birdsong and rustling leaves."),
            ("Cultural Treasures", "Unveiling the richness of heritage and tradition."),
            ("Whispering Sands", "Footprints on the shore, memories in the heart."),
            ("Wandering Spirit", "Roaming freely, embracing the unknown."),
            ("Whimsical Wonders", "Discovering the magic in everyday life."),
            ("Misty Mornings", "Ethereal beauty emerging from the mist."),
            ("Captured Essence", "Preserving fleeting emotions and memories."),
            ("Infinite Horizons", "Endless possibilities stretching beyond the horizon."),
            ("Symmetry in Chaos", "Finding order in the midst of a bustling world."),
            ("Untamed Beauty", "Nature's raw and untouched magnificence."),
            ("Urban Euphoria", "A city alive with vibrant energy."),
            ("Serenading Strings", "The melodic harmony of violin strings."),
            ("Spectrum of Emotions", "Expressions that paint a thousand words."),
            ("Dancing Shadows", "Graceful movements beneath the moonlight."),
            ("Whispered Secrets", "Unveiling hidden stories and untold tales."),
            ("Splashes of Joy", "Pure bliss found in playful moments."),
            ("Luminous Trails", "Leaving traces of light in the darkness."),
            ("Celestial Beauty", "Gazing at stars that hold the universe's secrets."),
            ("Gentle Awakening", "A new day dawning with tranquil serenity."),
            ("Fragments of Time", "Pieces of history waiting to be discovered."),
            ("Wild Wanderlust", "Adventuring into the unknown with untamed spirit."),
        })
        { }

        readonly Random random = new Random();
        public override async Task<Photo> GenerateElementFromRawData(ValueTuple<string, string> rawData)
        {

            Photo newPhoto = new(
                rawData.Item1,
                rawData.Item2,
                "",
                (random.Next(0, 2) == 0 ? true : false),
                null
                );
            newPhoto.Location = await UploadsManager.Download($"https://picsum.photos/seed/{newPhoto.GetHashCode()}/200/300");
            //newPhoto.Location = await UploadsManager.Download("https://fastly.picsum.photos/id/866/200/300.jpg?hmac=rcadCENKh4rD6MAp6V_ma-AyWv641M4iiOpe1RyFHeI");


            // Attach album if available
            var albums = context.Albums.ToList();
            if (albums.Count() > 0)
            {
                int randomAlbumIndex = random.Next(albums.Count() - 1);
                newPhoto.AlbumId = albums[randomAlbumIndex].AlbumId;
            }

            // Attach categories if available
            var categories = context.Categories.ToList();
            if (categories.Count() > 0)
            {
                int numberOfCategories = random.Next(2, 6);
                for (int i = 0; i < numberOfCategories; i++)
                {
                    Category randomCategory;

                    do
                    {
                        int randomCategoryIndex = random.Next(categories.Count() - 1);
                        randomCategory = categories[randomCategoryIndex];
                    } while (newPhoto.Categories.Any(category => category == randomCategory));

                    newPhoto.Categories.Add(randomCategory);
                }
            }

            return newPhoto;
        }

        public override DbSet<Photo> GetDbSet(PhotoAlbumContext context)
        {
            return context.Photos;
        }
    }
}
