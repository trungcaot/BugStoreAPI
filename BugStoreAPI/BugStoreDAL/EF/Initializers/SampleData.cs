using System;
using BugStoreModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace BugStoreDAL.EF.Initializers
{
    public static class SampleData
    {
        public static IEnumerable<Category> GetCategories() => new List<Category>
        {
            new Category {Name = "Iphone"},
            new Category {Name = "Samsung"},
        };

        public static IList<Product> GetProducts(IList<Category> categories)
        {
            var products = new List<Product>();
            foreach (var category in categories)
            {
                switch (category.Name)
                {
                    case "Iphone":
                        products.AddRange(new List<Product>
                        {
                            new Product
                            {
                                Category = category,
                                CategoryId = category.Id,
                                ProductImage = "product-image.png",
                                ProductImageLarge = "product-image-lg.png",
                                ProductImageThumb = "product-thumb.png",
                                ModelNumber = "RED1",
                                ModelName = "Communications Device",
                                UnitCost = 49.99M,
                                CurrentPrice = 49.99M,
                                Description =
                                    "Subversively stay in touch with this miniaturized wireless communications device. Speak into the pointy end and listen with the other end! Voice-activated dialing makes calling for backup a breeze. Excellent for undercover work at schools, rest homes, and most corporate headquarters. Comes in assorted colors.",
                                UnitsInStock = 2,
                                IsFeatured = true
                            },
                            new Product
                            {
                                Category = category,
                                CategoryId = category.Id,
                                ProductImage = "product-image.png",
                                ProductImageLarge = "product-image-lg.png",
                                ProductImageThumb = "product-thumb.png",
                                ModelNumber = "LK4TLNT",
                                ModelName = "Persuasive Pencil",
                                UnitCost = 1.99M,
                                CurrentPrice = 1.99M,
                                Description =
                                    "Persuade anyone to see your point of view!  Captivate your friends and enemies alike!  Draw the crime-scene or map out the chain of events.  All you need is several years of training or copious amounts of natural talent. You're halfway there with the Persuasive Pencil. Purchase this item with the Retro Pocket Protector Rocket Pack for optimum disguise.",
                                UnitsInStock = 5,
                            },
                            new Product
                            {
                                Category = category,
                                CategoryId = category.Id,
                                ProductImage = "product-image.png",
                                ProductImageLarge = "product-image-lg.png",
                                ProductImageThumb = "product-thumb.png",
                                ModelNumber = "LSRPTR1",
                                ModelName = "Nonexplosive Cigar",
                                UnitCost = 29.99M,
                                CurrentPrice = 29.99M,
                                Description =
                                    "Contrary to popular spy lore, not all cigars owned by spies explode! Best used during mission briefings, our Nonexplosive Cigar is really a cleverly-disguised, top-of-the-line, precision laser pointer. Make your next presentation a hit.",
                                UnitsInStock = 5,
                            },
                            new Product
                            {
                                Category = category,
                                CategoryId = category.Id,
                                ProductImage = "product-image.png",
                                ProductImageLarge = "product-image-lg.png",
                                ProductImageThumb = "product-thumb.png",
                                ModelNumber = "TCKLR1",
                                ModelName = "Fake Moustache Translator",
                                UnitCost = 599.99M,
                                CurrentPrice = 599.99M,
                                Description =
                                    "Fake Moustache Translator attaches between nose and mouth to double as a language translator and identity concealer. Sophisticated electronics translate your voice into the desired language. Wriggle your nose to toggle between Spanish, English, French, and Arabic. Excellent on diplomatic missions.",
                                UnitsInStock = 5,
                                IsFeatured = true
                            },
                            new Product
                            {
                                Category = category,
                                CategoryId = category.Id,
                                ProductImage = "product-image.png",
                                ProductImageLarge = "product-image-lg.png",
                                ProductImageThumb = "product-thumb.png",
                                ModelNumber = "JWLTRANS6",
                                ModelName = "Interpreter Earrings",
                                UnitCost = 459.99M,
                                CurrentPrice = 459.99M,
                                Description =
                                    "The simple elegance of our stylish monosex earrings accents any wardrobe, but their clean lines mask the sophisticated technology within. Twist the lower half to engage a translator function that intercepts spoken words in any language and converts them to the wearer's native tongue. Warning: do not use in conjunction with our Fake Moustache Translator product, as the resulting feedback loop makes any language sound like Pig Latin.",
                                UnitsInStock = 5,
                            },
                        });
                        break;
                    case "Samsung":
                        products.AddRange(new List<Product>
                        {
                            new Product
                            {
                                Category = category,
                                CategoryId = category.Id,
                                ProductImage = "product-image.png",
                                ProductImageLarge = "product-image-lg.png",
                                ProductImageThumb = "product-thumb.png",
                                ModelNumber = "DNTGCGHT",
                                ModelName = "Counterfeit Creation Wallet",
                                UnitCost = 999.99M,
                                CurrentPrice = 999.99M,
                                Description =
                                    "Don't be caught penniless in Prague without this hot item! Instantly creates replicas of most common currencies! Simply place rocks and water in the wallet, close, open up again, and remove your legal tender!",
                                UnitsInStock = 5,
                                IsFeatured = true
                            },
                            new Product
                            {
                                Category = category,
                                CategoryId = category.Id,
                                ProductImage = "product-image.png",
                                ProductImageLarge = "product-image-lg.png",
                                ProductImageThumb = "product-thumb.png",
                                ModelNumber = "CITSME9",
                                ModelName = "Cloaking Device",
                                UnitCost = 9999.99M,
                                CurrentPrice = 9999.99M,
                                Description =
                                    "Worried about detection on your covert mission? Confuse mission-threatening forces with this cloaking device. Powerful new features include string-activated pre-programmed phrases such as \"Danger! Danger!\", \"Reach for the sky!\", and other anti-enemy expressions. Hyper-reactive karate chop action deters even the most persistent villain.",
                                UnitsInStock = 5,
                            },
                            new Product
                            {
                                Category = category,
                                CategoryId = category.Id,
                                ProductImage = "product-image.png",
                                ProductImageLarge = "product-image-lg.png",
                                ProductImageThumb = "product-thumb.png",
                                ModelNumber = "BME007",
                                ModelName = "Indentity Confusion Device",
                                UnitCost = 6.99M,
                                CurrentPrice = 6.99M,
                                Description =
                                    "Never leave on an undercover mission without our Identity Confusion Device! If a threatening person approaches, deploy the device and point toward the oncoming individual. The subject will fail to recognize you and let you pass unnoticed. Also works well on dogs.",
                                UnitsInStock = 5,
                            },
                            new Product
                            {
                                Category = category,
                                CategoryId = category.Id,
                                ProductImage = "product-image.png",
                                ProductImageLarge = "product-image-lg.png",
                                ProductImageThumb = "product-thumb.png",
                                ModelNumber = "BHONST93",
                                ModelName = "Correction Fluid",
                                UnitCost = 1.99M,
                                CurrentPrice = 1.99M,
                                Description =
                                    "Disguised as typewriter correction fluid, this scientific truth serum forces subjects to correct anything not perfectly true. Simply place a drop of the special correction fluid on the tip of the subject's nose. Within seconds, the suspect will automatically correct every lie. Effects from Correction Fluid last approximately 30 minutes per drop. WARNING: Discontinue use if skin appears irritated.",
                                UnitsInStock = 5,
                                IsFeatured = true
                            },
                            new Product
                            {
                                Category = category,
                                CategoryId = category.Id,
                                ProductImage = "product-image.png",
                                ProductImageLarge = "product-image-lg.png",
                                ProductImageThumb = "product-thumb.png",
                                ModelNumber = "THNKDKE1",
                                ModelName = "Hologram Cufflinks",
                                UnitCost = 799.99M,
                                CurrentPrice = 799.99M,
                                Description =
                                    "Just point, and a turn of the wrist will project a hologram of you up to 100 yards away. Sneaking past guards will be child's play when you've sent them on a wild goose chase. Note: Hologram adds ten pounds to your appearance.",
                                UnitsInStock = 5,
                            },
                        });
                        break;
                }
            }
            return products;
        }
        public static IList<Order> GetCart(StoreContext context)
        {
            var prod1 = context.Categories.OrderBy(x => x.Name).Skip(1)
                .Include(c => c.Products).FirstOrDefault()?
                .Products.Skip(1).FirstOrDefault();
            return new List<Order>
            {
                new Order {
                    CustomerId = 1, DateCreated = DateTime.Now,
                    Product = prod1, Quantity = 1}
            };
        }
    }
}
