using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Annotations;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;


namespace ConsoleAppIdentity1
{
    class Program
    {
        static void Main(string[] args)
        {
            var userStore = new UserStore<IdentityUser>();
            var userManager = new UserManager<IdentityUser>(userStore);

            string username = "farid@wizlearn.com";
            string password = "Password123!";

            //CreateUser(userManager, username, password);
            
            var user = userManager.FindByName(username);
            string key = "given_name", value = "Farid";
            // AddClaim(userManager, user, key, value);

            var isMatch = userManager.CheckPassword(user, password);
            Console.WriteLine("CheckPass Res: {0}", isMatch);

           
        }

        private static void AddClaim(UserManager<IdentityUser> userManager, IdentityUser user, string key, string value)
        {
            var claimRes = userManager.AddClaim(user.Id,
               new Claim("given_name", "Farid"));
            Console.WriteLine("Claim: {0}", claimRes.Succeeded);
        }

        private static void CreateUser(UserManager<IdentityUser> userManager, string username, string password)
        {
            // creates a user using asp.net identity
            //  this crete user using their build in functions.
            var creationResult = userManager.Create(
                new IdentityUser(username), password);

            Console.WriteLine("Created: {0}", creationResult.Succeeded);
        }



    }

    public class CustomUser : IUser<int>
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
    }


    public class CustomerUserDbContext: DbContext
    {
        public CustomerUserDbContext() : base("DefaultConnection") { }

        public DbSet<CustomUser> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var user = modelBuilder.Entity<CustomUser>();
            user.ToTable("Users");
            user.HasKey(x => x.Id);
            user.Property(x => x.Id).IsRequired()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            user.Property(x => x.UserName).IsRequired().HasMaxLength(256)
                .HasColumnAnnotation("Index", 
                        new IndexAnnotation(
                            new IndexAttribute("UserNameIndex") { IsUnique = true }));

            base.OnModelCreating(modelBuilder);
        }
    }

}
