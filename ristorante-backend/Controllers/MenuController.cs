using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ristorante_backend.Models;
using ristorante_backend.Repositories;

namespace ristorante_backend.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private readonly MenuRepository _menuRepo;

        public MenuController(MenuRepository menuRepository)
        {
            _menuRepo = menuRepository;
        }


        [HttpGet]
        public async Task<IActionResult> Get(string? name)
        {
            try
            {
                if (name == null)
                {
                    return Ok(await _menuRepo.GetAllMenus());
                }

                return Ok(await _menuRepo.GetMenuByName(name));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                Menu menu = await _menuRepo.GetMenuById(id);

                if (menu == null)
                {
                    return NotFound();
                }

                return Ok(menu);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Menu newMenu)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState.Values);
                }



                return Ok(await _menuRepo.CreateMenu(newMenu));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Menu newMenu)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState.Values);
                }

                int affectedRows = await _menuRepo.UpdateMenu(id, newMenu);

                if (affectedRows == 0)
                {
                    return NotFound();
                }

                return Ok(affectedRows);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }



        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                int affectedRows = await _menuRepo.DeleteMenu(id);

                if (affectedRows == 0)
                {
                    return NotFound();
                }

                return Ok(affectedRows);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


    }
}




