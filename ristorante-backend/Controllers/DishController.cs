using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ristorante_backend.Models;
using ristorante_backend.Repositories;

namespace ristorante_backend.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DishController : ControllerBase
    {
        private readonly DishRepository _dishRepo;

        public DishController(DishRepository dishRepo)
        {
            _dishRepo = dishRepo;
        }


        [HttpGet]
        public async Task<IActionResult> Get(string? nome)
        {
            try
            {
                if (nome == null)
                {
                    return Ok(await _dishRepo.GetAllDishes());
                }

                return Ok(await _dishRepo.GetDishesByName(nome));
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
                Dish dish = await _dishRepo.GetDish(id);

                if (dish == null)
                {
                    return NotFound();
                }

                return Ok(dish);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Dish newDish)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState.Values);
                }

                

                return Ok(await _dishRepo.CreateDish(newDish));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Dish newDish)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState.Values);
                }

                (int affectedRows,int addedMenuIds,int deletedMenuIds) updatedRows = await _dishRepo.UpdateDish2(id, newDish);
                
                if (updatedRows.affectedRows == 0)
                {
                    return NotFound();
                }

                return Ok(@$"E' stata aggiunto {updatedRows.affectedRows} piatto
                             Sono stati aggiunti {updatedRows.addedMenuIds} riferimenti
                             Sono stati eliminati {updatedRows.deletedMenuIds} riferimenti");
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
                int affectedRows = await _dishRepo.DeleteDish(id);

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
