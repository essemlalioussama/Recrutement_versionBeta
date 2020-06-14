using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using WebApplication1.Models;

[assembly: OwinStartupAttribute(typeof(WebApplication1.Startup))]
namespace WebApplication1
{
    public partial class Startup
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            StartUpApplication();
        }

        public void StartUpApplication()
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            IdentityRole role = new IdentityRole();
            if(!roleManager.RoleExists("Administrateur"))
            {
                role.Name = "Administrateur";
                roleManager.Create(role);
                ApplicationUser user = new ApplicationUser();
                user.UserName = "Zebiri";
                user.Email = "W.Zebiri@gmail.com";
                var verifier = userManager.Create(user, "Walid&01");
                if (verifier.Succeeded)
                {
                    userManager.AddToRole(user.Id, "Administrateur");
                }
            }


        }
    }
}
