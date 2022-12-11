using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuperHeroController : ControllerBase
    {
        private readonly DataContext dataContext;

        public SuperHeroController(DataContext dataContext) {
            this.dataContext = dataContext;
        }

        [HttpGet]
        public async Task<ActionResult<List<MySuperHero>>> Get()
        {
            // return Ok(heroes); old method
            return Ok(await this.dataContext.SuperHeroes.ToListAsync());
        }
        [HttpPost]

        // this will add a new superhero or an item to the main array
        public async Task<ActionResult<List<MySuperHero>>> AddHero(MySuperHero Hero)
        {
            this.dataContext.SuperHeroes.Add(Hero); // we change something in the database table and now we have to add async 
            await this.dataContext.SaveChangesAsync(); //  this commits to the database 
            return Ok(await this.dataContext.SuperHeroes.ToListAsync());
        }
        [HttpGet("{id}")]

        // i want only one item from the main array and therefore this method is implemented
        // this returns only one item from the array and returns bad request if the item is not found
        public async Task<ActionResult<MySuperHero>> Get(int id)
        {
            var hero = await this.dataContext.SuperHeroes.FindAsync(id);
            if (hero == null)
            {
                return BadRequest("Hero not found!");
            }
            return Ok(hero);
        }

        [HttpPut]
        // now we want to update the items in the original item or hero

        public async Task<ActionResult<List<MySuperHero>>> UpdateHero(MySuperHero Request)
        {
            var DBhero = await this.dataContext.SuperHeroes.FindAsync(Request.Id);
            if (DBhero == null)
            {
                return BadRequest("Hero not found!");
            }
            DBhero.Name = Request.Name;
            DBhero.FirstName = Request.FirstName;
            DBhero.LastName = Request.LastName;
            DBhero.Place= Request.Place;
            await this.dataContext.SaveChangesAsync();
            return Ok(await this.dataContext.SuperHeroes.ToListAsync());
        }
        

        [HttpDelete("{id}")]

        // removes only 1 item from the main object
        // this returns only one item from the array and returns bad request if the item is not found
        public async Task<ActionResult<MySuperHero>> DeleteItem(int id)
        {
            var dbHero = await this.dataContext.SuperHeroes.FindAsync(id);
            if (dbHero == null)
            {
                return BadRequest("Hero not found!");
            }
            this.dataContext.SuperHeroes.Remove(dbHero);
            await this.dataContext.SaveChangesAsync();
            return Ok(await this.dataContext.SuperHeroes.ToListAsync());
        }
    }
}
