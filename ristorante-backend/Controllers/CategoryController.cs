using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ristorante_backend.Models;
using ristorante_backend.Repositories;

namespace ristorante_backend.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly CategoryRepository _categoryRepo;

        public CategoryController(CategoryRepository categoryRepository)
        {
            _categoryRepo = categoryRepository;
        }


        [HttpGet]
        public async Task<IActionResult> Get(string? name)
        {
            try
            {
                if (name == null)
                {
                    return Ok(await _categoryRepo.GetAllCategories());
                }

                return Ok(await _categoryRepo.GetCategoriesByName(name));
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
                Category category = await _categoryRepo.GetCategoriesById(id);

                if (category == null)
                {
                    return NotFound();
                }

                return Ok(category);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Category newCategory)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState.Values);
                }



                return Ok(await _categoryRepo.CreateCategory(newCategory));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Category newCategory)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState.Values);
                }

                int affectedRows = await _categoryRepo.UpdateCategory(id, newCategory);

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
                int affectedRows = await _categoryRepo.DeleteCategory(id);

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

